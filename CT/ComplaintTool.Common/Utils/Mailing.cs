//using System;
//using System.Diagnostics;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using ComplaintTool.Common.Config;
//using ComplaintTool.Common.CTLogger;

//namespace ComplaintTool.Common.Utils
//{
//    public static class Mailing
//    {
//        private static readonly ILogger Logger = LogManager.GetLogger();

//        public static void SendEmail(string subject, string body, string mailingGroups)
//        {
//            var eventLog = new EventLog { Log = "Application", Source = "ComplaintService" };
//            if (!string.IsNullOrEmpty(mailingGroups))
//            {
//                var toGroups = mailingGroups.Split(';');
//                var host = ComplaintConfig.Instance.Parameters["EmailHost"].ParameterValue;
//                var fromAddress = ComplaintConfig.Instance.Parameters["EmailFromAddress"].ParameterValue;
//                var fromDisplay = ComplaintConfig.Instance.Parameters["EmailFromDisplay"].ParameterValue;
//                var credentialUser = ComplaintConfig.Instance.Parameters["EmailCredentialUser"].ParameterValue;
//                var credentialPassword =
//                    Encryption.Decrypt(ComplaintConfig.Instance.Parameters["EmailCredentialPassword"].ParameterValue);
//                var port = int.Parse(ComplaintConfig.Instance.Parameters["EmailPort"].ParameterValue);

//                try
//                {
//                    var mail = new MailMessage
//                    {
//                        IsBodyHtml = false,
//                        Body = body,
//                        From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8),
//                        Subject = string.Format("{0} - {1}", Environment.MachineName, subject),
//                        SubjectEncoding = Encoding.UTF8,
//                        Priority = MailPriority.Normal
//                    };

//                    foreach (var group in toGroups)
//                    {
//                        var paramAddreses = ComplaintConfig.Instance.Parameters[group];

//                        if (paramAddreses == null) continue;

//                        var addresses = paramAddreses.ParameterValue.Split(';');
//                        foreach (var addressStr in addresses)
//                        {
//                            var address = new MailAddress(addressStr);
//                            if (!mail.To.Contains(address))
//                                mail.To.Add(address);
//                        }
//                    }

//                    var smtpClient = new SmtpClient
//                    {
//                        UseDefaultCredentials = false,
//                        Credentials = new NetworkCredential(credentialUser, credentialPassword),
//                        Host = host,
//                        Port = port,
//                        Timeout = 10000,
//                        DeliveryMethod = SmtpDeliveryMethod.Network
//                    };
//                    smtpClient.Send(mail);
//                    Logger.Info("Send mail to: " + mail.To);
//                }
//                catch (Exception ex)
//                {
//                    eventLog.WriteEntry(string.Format("Send mail error. {0}", ex), EventLogEntryType.Error,500);
//                    Logger.Error(ex);
//                }
//            }
//            else
//            {
//                var message = string.Format(@"No definition of mailing groups. Subject: {0}; Body: {1}",subject,body);
//                eventLog.WriteEntry(message, EventLogEntryType.Error, 500);
//                Logger.Error(message);
//            }
//        }
//    }
//}
