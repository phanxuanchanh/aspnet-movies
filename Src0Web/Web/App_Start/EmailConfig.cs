using Common.Mail;
using System.Configuration;
using System.Net.Mail;

namespace Web.App_Start
{
    public class EmailConfig
    {
        public static void RegisterEmail()
        {
            EMail.Address = ConfigurationManager.AppSettings["EmailAddress"];
            EMail.Password = ConfigurationManager.AppSettings["EmailPassword"];
        }
    }
}