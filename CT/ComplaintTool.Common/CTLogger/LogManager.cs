using ComplaintTool.Common.Config;
using ComplaintTool.Common.Utils;
using ComplaintTool.Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using ComplaintTool.Models;

namespace ComplaintTool.Common.CTLogger
{
    public sealed class LogManager : ILogger
    {
        #region Fields

        private const string Source = "ComplaintService";
        private const string Log = "Application";
        private const string MailSubject = "ComplaintTool Logging";
        private static readonly ConcurrentStack<Exception> Errors = new ConcurrentStack<Exception>();
        private readonly Type _currentClassType;
        private readonly EventLog _logger;

        #endregion

        #region Ctor_Factory

        public static ILogger GetLogger()
        {
            return new LogManager(new StackFrame(1).GetMethod().DeclaringType);
        }

        private LogManager(Type type)
        {
            _currentClassType = type;
            _logger = new EventLog { Log = Log, Source = Source };
        }

        #endregion

        #region ILogger

        public void LogComplaintEvent(int msgEventNumber, params object[] args)
        {
            Guard.ThrowIf<ArgumentNullException>(_logger == null, "logger");

            NotificationDefinition definition;
            if (!ComplaintConfig.Instance.Notifications.TryGetValue(msgEventNumber, out definition))
            {
                LogComplaintException(new Exception(string.Format("LogComplaintEvent - notification definition with ID: {0} does not exist!", msgEventNumber)));
                return;
            }

            string message;
            try
            {
                if (args != null && args.Length > 0)
                    message = string.Format(definition.MessageText, args);
                else
                    message = definition.MessageText;
            }
            catch (Exception ex)
            {
                LogComplaintException(ex);
                message = definition.MessageText;
            }

            message = _currentClassType.FullName + ": " + message;

            if (!definition.MailingGroups.IsEmpty())
                SendEmail(message, definition.MailingGroups);

            if (_logger != null) _logger.WriteEntry(message, EventLogEntryType.Information, msgEventNumber);
        }

        public void LogComplaintException(Exception ex, int errNumber = 500)
        {
            Guard.ThrowIf<ArgumentNullException>(_logger == null, "logger");
            Guard.ThrowIf<ArgumentNullException>(ex == null, "ex");
            
            var message = BuildMessage(ex);
            if (_logger != null) _logger.WriteEntry(message, EventLogEntryType.Error, errNumber);
            Errors.Push(ex);
        }

        #endregion

        #region Helper

        private static string BuildMessage(Exception ex)
        {
            var exception = ex as DbEntityValidationException;
            if (exception != null)
            {
                var errorMessages = exception.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                return string.Concat(exception.ToString(), " ---> The validation errors are: ", fullErrorMessage);
            }
            var updateException = ex as DbUpdateException;
            if (updateException != null)
            {
                var updEx = updateException;
                var errorEntities = updEx.Entries.Select(x => x.Entity.GetType().Name);
                var fullErrorMessage = string.Join("; ", errorEntities);
                return string.Concat(updateException.ToString(), " ---> The error entities are: ", fullErrorMessage);
            }
            return ex.ToString();
        }

        public string SendEmail(string body, string mailingGroups)
        {
            Guard.ThrowIf<ArgumentNullException>(body.IsEmpty(), "body");
            Guard.ThrowIf<ArgumentNullException>(mailingGroups.IsEmpty(), "mailingGroups");

            try
            {
                // @dsmaza Refactoring - przenoszę z ComplaintTool.Shell.Common.Tool.SendMail - robię tylko kosmetyczne zmiany
                var host = ComplaintConfig.Instance.Parameters["EmailHost"].ParameterValue;
                var fromAddress = ComplaintConfig.Instance.Parameters["EmailFromAddress"].ParameterValue;
                var fromDisplay = ComplaintConfig.Instance.Parameters["EmailFromDisplay"].ParameterValue;
                var credentialUser = ComplaintConfig.Instance.Parameters["EmailCredentialUser"].ParameterValue;
                var credentialPassword = Encryption.Decrypt(ComplaintConfig.Instance.Parameters["EmailCredentialPassword"].ParameterValue);
                var port = int.Parse(ComplaintConfig.Instance.Parameters["EmailPort"].ParameterValue);

                var toGroups = mailingGroups.Split(';');
                using (var mail = new MailMessage
                {
                    IsBodyHtml = false,
                    Body = body,
                    From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8),
                    Subject = string.Format("{0} - {1}", Environment.MachineName, MailSubject),
                    SubjectEncoding = Encoding.UTF8
                })
                {                
                    foreach (var group in toGroups)
                    {
                        var paramAddreses = ComplaintConfig.Instance.Parameters[group];
                        if (paramAddreses == null) continue;

                        var addresses = paramAddreses.ParameterValue.Split(';');
                        foreach (var addressStr in addresses)
                        {
                            var address = new MailAddress(addressStr);
                            if (!mail.To.Contains(address))
                                mail.To.Add(address);
                        }
                    }

                    using (var smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(credentialUser, credentialPassword),
                        Host = host,
                        Port = port,
                        Timeout = 10000,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    })
                    {
                        try
                        {
                            // @dsmaza Refactoring - daje jako asynchroniczne, nie ma potrzeby czekac na wyslanie maile... 
                            smtpClient.SendMailAsync(mail);
                        }
                        catch (Exception ex)
                        {
                            LogComplaintException(ex);
                        }
                    }

                    return mail.To.ToString();
                }
            }
            catch (Exception ex)
            {
                LogComplaintException(ex);
                return null;
            }
        }

        public static IEnumerable<Exception> GetErrors()
        {
            var errors = new List<Exception>();

            while (!Errors.IsEmpty)
            {
                Exception ex;
                if (!Errors.TryPop(out ex))
                    break;
                errors.Add(ex);
            }

            return errors;
        }

        #endregion
    }
}
