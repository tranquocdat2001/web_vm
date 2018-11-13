using System;
using System.Net.Mail;

namespace Utilities
{
    public class GoogleMail
    {
        public static string SendMail(string toList, string from, string subject, string body, string emailNoReply, string passEmailNoReply)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            string msg = string.Empty;

            try
            {
                MailAddress fromAddress = new MailAddress(from);
                message.From = fromAddress;
                message.To.Add(toList);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailNoReply, passEmailNoReply);
                smtpClient.Send(message);
                msg = "true";
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", toList, ex));
            }

            return msg;
        }

    }

    public class AmazoneEmail
    {
        private string _userName = "";
        private string _password = "";
        private string _host = "";
        private int _port = 587;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private static AmazoneEmail _instance;

        private static object lockObject = new object();

        public static AmazoneEmail Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (lockObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new AmazoneEmail();
                        }
                    }
                }
                return _instance;
            }
        }

        public bool SendMail(AmazoneEmailMessage message)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                try
                {
                    SMTPServer obj1 = new SMTPServer()
                    {
                        UserName = UserName,
                        Password = Password,
                        Host = Host,
                        Port = Port
                    };
                    smtp.Host = obj1.Host;
                    smtp.Port = obj1.Port;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential(obj1.UserName, obj1.Password);
                    smtp.Timeout = 60 * 1000;
                    smtp.ServicePoint.ConnectionLimit = 20;
                    var mail = new MailMessage
                    {
                        From = new MailAddress(message.FromEmail, message.FromName)
                    };
                    mail.To.Add(message.To);
                    mail.Subject = message.Subject;
                    mail.Body = message.Body;
                    mail.IsBodyHtml = true;
                    smtp.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
            return false;
        }
    }

    public class AmazoneEmailMessage
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    internal struct SMTPServer
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool IsSecure { get; set; }
    }
}
