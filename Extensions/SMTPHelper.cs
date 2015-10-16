using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Extensions
{
    public class SMTPHelper
    {
        private string Server;
        private int ServerPort;
        private bool HasSSL;
        private string UserMail;
        private string PasswordMail;

        private SmtpClient Client;

        //public SMTPHelper()
        //{
        //    Server = ConfigurationManager.AppSettings["SmtpServer"];
        //    ServerPort = ConfigurationManager.AppSettings["SmtpServerPort"].TryParseAsInt();
        //    HasSSL = ConfigurationManager.AppSettings["SmtpSecure"].TryParseAsBool();
        //    UserMail = ConfigurationManager.AppSettings["UserMail"];
        //    PasswordMail = ConfigurationManager.AppSettings["PassMail"];

        //    CreateSMTP();
        //}

        public SMTPHelper(string smtpServer, int smtpServerPort, bool hasSSL, string userMail, string passwordMail)
        {
            Server = smtpServer;
            ServerPort = smtpServerPort;
            HasSSL = hasSSL;
            UserMail = userMail;
            PasswordMail = passwordMail;

            CreateSMTP();
        }

        private void CreateSMTP()
        {
            Client = new SmtpClient(Server, ServerPort)
            {
                EnableSsl = HasSSL,
                Credentials = new NetworkCredential(UserMail, PasswordMail),

            };
        }

        public void Send(MailMessage mail)
        {
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.DeliveryFormat = SmtpDeliveryFormat.International;
            Client.Send(mail);
        }
    }
}
