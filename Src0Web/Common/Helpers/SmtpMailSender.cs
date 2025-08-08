using System.Net;
using System.Net.Mail;

namespace Web.Shared
{
    public class SmtpMailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromAddress;
        private readonly string _fromDisplayName;

        public SmtpMailSender(string host, string username, string password, string fromAddress, string fromDisplayName = null, int port = 587, bool enableSsl = true)
        {
            _host = host;
            _username = username;
            _password = password;
            _fromAddress = fromAddress;
            _fromDisplayName = fromDisplayName ?? fromAddress;
            _port = port;
            _enableSsl = enableSsl;
        }

        public void Send(string toEmail, string subject, string body, bool isHtml = true)
        {
            MailAddress mailFrom = new MailAddress(_fromAddress, _fromDisplayName);
            MailAddress mailTo = new MailAddress(toEmail);

            MailMessage message = new MailMessage(mailFrom, mailTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            using (SmtpClient smtp = new SmtpClient(_host, _port))
            {
                smtp.EnableSsl = _enableSsl;
                smtp.Credentials = new NetworkCredential(_username, _password);
                smtp.Send(message);
            }
        }
    }
}
