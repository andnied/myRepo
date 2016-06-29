//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity.Core;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Reflection;
//using System.Security.Principal;
//using System.Text;
//using System.Web;
//using ComplaintTool.Common.Config;
//using ComplaintTool.Common.Enum;
//using ComplaintTool.Common.Extensions;
//using ComplaintTool.Common.Utils;
//using NLog;
//using NLog.Config;

//namespace ComplaintTool.Common
//{
//    public static class Logging
//    {
//        private static readonly ConcurrentStack<Exception> _errors = new ConcurrentStack<Exception>();
//        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
//        private const string MailSubject = "ComplaintTool Logging";

//        #region Configuration

//        public static void LoadConfiguration()
//        {
//            if (LogManager.Configuration == null)
//            {
//                var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NLog.config");
//                LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);
//            }
//        }

//        #endregion

//        #region Errors Cache

//        public static IEnumerable<Exception> GetErrors()
//        {
//            var errors = new List<Exception>();
//            while (!_errors.IsEmpty)
//            {
//                Exception ex = null;
//                if (!_errors.TryPop(out ex))
//                    break;
//                errors.Add(ex);
//            }
//            return errors;
//        }

//        #endregion

//        #region NLog Extensions

//        public static void LogComplaintEvent(this ILogger logger, int msgEventNumber, params object[] args)
//        {
//            Guard.ThrowIf<ArgumentNullException>(logger == null, "logger");

//            NotificationDefinition definition = null;
//            if (!ComplaintConfig.Instance.Notifications.TryGetValue(msgEventNumber, out definition))
//            {
//                logger.Fatal(string.Format("LogComplaintEvent - notification definition with ID:{0} not exists!", msgEventNumber));
//                return;
//            }

//            string message = null;
//            try
//            {
//                if (args != null && args.Length > 0)
//                    message = string.Format(definition.MessageText, args);
//                else
//                    message = definition.MessageText;
//            }
//            catch (Exception ex)
//            {
//                logger.Fatal(ex);
//                message = definition.MessageText;
//            }

//            if (!definition.MailingGroups.IsEmpty())
//                SendEmail(message, definition.MailingGroups, GetMailPriority(definition.MessageType));

//            if (definition.SendNotification.GetValueOrDefault())
//                AddNotification(message, (MessageType)definition.MessageType);

//            logger.Log(typeof(Logging), BuildLogEvent(message, GetLevel(definition.MessageType), logger.Name));
//        }

//        public static void LogComplaintException(this ILogger logger, Exception ex)
//        {
//            Guard.ThrowIf<ArgumentNullException>(logger == null, "logger");
//            Guard.ThrowIf<ArgumentNullException>(ex == null, "ex");

//            string message = BuildMessage(ex);
//            logger.Log(typeof(Logging), BuildLogEvent(message, GetLevel(ex), logger.Name));
//            _errors.Push(ex);
//        }

//        #endregion

//        #region AddNotification

//        public static void AddNotification(string message, MessageType messageType)
//        {
//            Guard.ThrowIf<ArgumentNullException>(message.IsEmpty(), "message");

//            try
//            {
//                using (var ctx = new ComplaintConfigEntities(ComplaintConfig.Instance.GetInternalEntityConnectionString()))
//                {
//                    if (message.Length > 512)
//                        message = message.Substring(0, 512);

//                    var note = new Notification
//                    {
//                        MessageDate = DateTime.UtcNow,
//                        Message = message,
//                        MessageType = messageType.ToString()
//                    };
//                    ctx.Notification.Add(note);
//                    ctx.SaveChanges();
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.Fatal(ex);
//            }
//        }

//        #endregion

//        #region SendEmail

//        public static string SendEmail(string body, string mailingGroups, MailPriority mailPriority)
//        {
//            return SendEmail(MailSubject, body, mailingGroups, mailPriority);
//        }

//        public static string SendEmail(string subject, string body, string mailingGroups, MailPriority mailPriority)
//        {
//            Guard.ThrowIf<ArgumentNullException>(subject.IsEmpty(), "subject");
//            Guard.ThrowIf<ArgumentNullException>(body.IsEmpty(), "body");
//            Guard.ThrowIf<ArgumentNullException>(mailingGroups.IsEmpty(), "mailingGroups");

//            try
//            {
//                // @dsmaza Refactoring - przenoszę z ComplaintTool.Shell.Common.Tool.SendMail - robię tylko kosmetyczne zmiany
//                var host = ComplaintConfig.Instance.Parameters["EmailHost"].ParameterValue;
//                var fromAddress = ComplaintConfig.Instance.Parameters["EmailFromAddress"].ParameterValue;
//                var fromDisplay = ComplaintConfig.Instance.Parameters["EmailFromDisplay"].ParameterValue;
//                var credentialUser = ComplaintConfig.Instance.Parameters["EmailCredentialUser"].ParameterValue;
//                var credentialPassword = Encryption.Decrypt(ComplaintConfig.Instance.Parameters["EmailCredentialPassword"].ParameterValue);
//                var port = Int32.Parse(ComplaintConfig.Instance.Parameters["EmailPort"].ParameterValue);

//                string[] toGroups = mailingGroups.Split(';');
//                var mail = new MailMessage
//                {
//                    IsBodyHtml = false,
//                    Body = body,
//                    From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8),
//                    Subject = String.Format("{0} - {1}", Environment.MachineName, subject),
//                    SubjectEncoding = Encoding.UTF8,
//                    Priority = mailPriority
//                };

//                foreach (var group in toGroups)
//                {
//                    var paramAddreses = ComplaintConfig.Instance.Parameters[group];
//                    if (paramAddreses == null) continue;

//                    var addresses = paramAddreses.ParameterValue.Split(';');
//                    foreach (var addressStr in addresses)
//                    {
//                        var address = new MailAddress(addressStr);
//                        if (!mail.To.Contains(address))
//                            mail.To.Add(address);
//                    }
//                }

//                var smtpClient = new SmtpClient
//                {
//                    UseDefaultCredentials = false,
//                    Credentials = new NetworkCredential(credentialUser, credentialPassword),
//                    Host = host,
//                    Port = port,
//                    Timeout = 10000,
//                    DeliveryMethod = SmtpDeliveryMethod.Network
//                };

//                try
//                {
//                    // @dsmaza Refactoring - daje jako asynchroniczne, nie ma potrzeby czekac na wyslanie maile... 
//                    smtpClient.SendMailAsync(mail);
//                }
//                catch (Exception ex)
//                {
//                    _logger.Fatal(ex);
//                }
//                return mail.To.ToString();
//            }
//            catch (Exception ex)
//            {
//                _logger.Fatal(ex);
//                return null;
//            }
//        }

//        #endregion

//        #region Helpers

//        private static LogEventInfo BuildLogEvent(string message, LogLevel level, string loggerName)
//        {
//            var logEvent = LogEventInfo.Create(level, loggerName, message);
//            logEvent.Properties["ComplaintUser"] = GetCurrentUser();

//            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
//            if (assembly != null)
//            {
//                var assemblyName = assembly.GetName();
//                logEvent.Properties["ComplaintApp"] = assemblyName.Name;
//                logEvent.Properties["ComplaintVersion"] = assemblyName.Version;
//                logEvent.Properties["ComplaintArchitecture"] = assemblyName.ProcessorArchitecture;
//            }
//            return logEvent;
//        }

//        private static string BuildMessage(Exception ex)
//        {
//            if (ex is DbEntityValidationException)
//            {
//                var errorMessages = ((DbEntityValidationException)ex).EntityValidationErrors
//                    .SelectMany(x => x.ValidationErrors)
//                    .Select(x => x.ErrorMessage);
//                var fullErrorMessage = string.Join("; ", errorMessages);
//                return string.Concat(ex.ToString(), " ---> The validation errors are: ", fullErrorMessage);
//            }
//            else if (ex is DbUpdateException)
//            {
//                var updEx = (DbUpdateException)ex;
//                var errorEntities = updEx.Entries.Select(x => x.Entity.GetType().Name);
//                var fullErrorMessage = string.Join("; ", errorEntities);
//                return string.Concat(ex.ToString(), " ---> The error entities are: ", fullErrorMessage);
//            }
//            else
//            {
//                return ex.ToString();
//            }
//        }

//        private static string GetCurrentUser()
//        {
//            string userName = null;
//            try
//            {
//                userName = HttpContext.Current.User.Identity.Name;
//            }
//            catch { }

//            if (userName.IsEmpty())
//            {
//                try
//                {
//                    userName = WindowsIdentity.GetCurrent().Name;
//                }
//                catch { }
//            }
//            return userName;
//        }

//        private static LogLevel GetLevel(int messageType)
//        {
//            var t = (MessageType)messageType;
//            switch (t)
//            {
//                case MessageType.Information:
//                case MessageType.Success:
//                    return LogLevel.Info;
//                case MessageType.Error:
//                    return LogLevel.Error;
//                case MessageType.Warning:
//                    return LogLevel.Warn;
//                case MessageType.Failure:
//                    return LogLevel.Fatal;
//                default:
//                    return LogLevel.Info;
//            }
//        }

//        private static LogLevel GetLevel(Exception ex)
//        {
//            // TODO mapowania typow wyjatkow na level w logu
//            if (ex is EntityException) // bazowy wyjatek dla wszystkich rzucanych przez EntityFramework
//                return LogLevel.Fatal;
//            else if (ex is DataException)
//                return LogLevel.Fatal;
//            else
//                return LogLevel.Error;
//        }

//        private static MailPriority GetMailPriority(int messageType)
//        {
//            return GetMailPriority(GetLevel(messageType));
//        }

//        private static MailPriority GetMailPriority(LogLevel level)
//        {
//            if (level == LogLevel.Info || level == LogLevel.Warn)
//                return MailPriority.Normal;
//            else if (level == LogLevel.Error || level == LogLevel.Fatal)
//                return MailPriority.High;
//            else
//                return MailPriority.Low;
//        }

//        #endregion
//    }
//}
