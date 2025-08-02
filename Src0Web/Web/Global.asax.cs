using Common.Web;
using MSSQL.Connection;
using System;
using System.Web.Routing;
using Web.App_Start;
using Web.Install;
using Web.Models;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SqlConnectInfo.ReadFromConfigFile("MovieDB");

            //EmailConfig.RegisterEmail();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            PageVisitor.Add();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Installer.RunIfNotInstalled(Context);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };

            Response.RedirectToRoute("Notification_Error", null);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            PageVisitor.Remove();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}