using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using ComplaintTool.Common.Properties;
using ComplaintTool.Common.Config;

namespace ComplaintTool.Common.Utils
{
    public static class Tool
    {
        private static readonly EventLog EventLog;

        static Tool()
        {
            if (!EventLog.SourceExists("ComplaintTool.Shell"))
                EventLog.CreateEventSource("ComplaintTool.Shell", "Application");
            EventLog = new EventLog { Log = "Application", Source = "ComplaintTool.Shell" };
        }

        public static string ExceptionLog(int messageEventNamber, object[] messageParam, bool toMaile, bool toEventLog)
        {
            var message = string.Empty;
            var notificationDefinition = ComplaintConfig.Instance.Notifications[messageEventNamber];
            if (notificationDefinition == null) return message;
            message = messageParam.Any() ? string.Format(notificationDefinition.MessageText, messageParam) : notificationDefinition.MessageText;
            if (toEventLog)
            {
                EventLog.WriteEntry(message, EventLogEntryType.Error, notificationDefinition.MessageEventNamber);
            }

            if (!toMaile) return message;
            var emails = SendEmail("Error info.", message, notificationDefinition.MailingGroups, MailPriority.High);
            EventLog.WriteEntry(string.Format(Resources._5002,notificationDefinition.MessageEventNamber, emails), EventLogEntryType.Error, notificationDefinition.MessageEventNamber);
            return message;
        }

        public static string NotyficationLog(int messageEventNamber, object[] messageParam, bool toMaile, bool toEventLog)
        {
            var message = string.Empty;
            var notificationDefinition = ComplaintConfig.Instance.Notifications[messageEventNamber];
            if (notificationDefinition == null) return message;
            message = messageParam.Any() ? string.Format(notificationDefinition.MessageText, messageParam) : notificationDefinition.MessageText;
            if (toEventLog)
            {
                if (message.Length >= 32766)
                    message = message.Substring(0, 32760);
                EventLog.WriteEntry(message, EventLogEntryType.Information, notificationDefinition.MessageEventNamber);
            }

            if (!toMaile) return message;
            var emails = SendEmail("Work info.", message, notificationDefinition.MailingGroups, MailPriority.Normal);
            EventLog.WriteEntry(string.Format(Resources._5002, notificationDefinition.MessageEventNamber, emails), EventLogEntryType.Information, notificationDefinition.MessageEventNamber);
            return message;
        }

        public static string SendEmail(string subject, string body, string mailingGroups, MailPriority mailPriority)
        {
            var config = ComplaintConfig.Instance;
            if (!string.IsNullOrEmpty(mailingGroups))
            {
                var toGroups = mailingGroups.Split(';');

                var host = config.Parameters["EmailHost"].ParameterValue;
                var fromAddress = config.Parameters["EmailFromAddress"].ParameterValue;
                var fromDisplay = config.Parameters["EmailFromDisplay"].ParameterValue;
                var credentialUser = config.Parameters["EmailCredentialUser"].ParameterValue;
                var credentialPassword =
                    Encryption.Decrypt(config.Parameters["EmailCredentialPassword"].ParameterValue);
                var port = int.Parse(config.Parameters["EmailPort"].ParameterValue);

                try
                {
                    var mail = new MailMessage
                    {
                        IsBodyHtml = false,
                        Body = body,
                        From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8),
                        Subject = string.Format("{0} - {1}", Environment.MachineName, subject),
                        SubjectEncoding = Encoding.UTF8,
                        Priority = mailPriority
                    };

                    foreach (var group in toGroups)
                    {
                        var paramAddreses = config.Parameters[group];

                        if (paramAddreses == null) continue;

                        var addresses = paramAddreses.ParameterValue.Split(';');
                        foreach (var addressStr in addresses)
                        {
                            var address = new MailAddress(addressStr);
                            if (!mail.To.Contains(address))
                                mail.To.Add(address);
                        }
                    }

                    var smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(credentialUser, credentialPassword),
                        Host = host,
                        Port = port,
                        Timeout = 10000,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    try
                    {
                        smtpClient.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(string.Format(Resources._5006, ex), EventLogEntryType.Error, 5006);
                    }
                    return mail.To.ToString();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(string.Format(Resources._5003, ex), EventLogEntryType.Error, 5003);
                    return null;
                }
            }
            else
            {
                var message = string.Format(Resources._5004, subject, body);
                EventLog.WriteEntry(message, EventLogEntryType.Error, 500);
                return null;
            }
        }
    }
}
