using MSSQL.Connection;
using System;

using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using Web.App_Start;
using Web.Models;
using Web.Shared;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SqlConnectInfo.ReadFromConfigFile("MovieDB");
            Installer.RunIfNotInstalled();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            PageVisitor.Add();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Installer.IsAppSettingsMissing)
            {
                string currentUrl = Context.Request.Url.AbsolutePath;

                if (!currentUrl.EndsWith("/Install/AppSettings.aspx"))
                {
                    Context.Response.Redirect("~/Install/AppSettings.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                string url = HttpContext.Current.Request.RawUrl.ToLower();
                if (url.EndsWith(".aspx"))
                {
                    HttpContext.Current.Response.StatusCode = 404;
                    HttpContext.Current.Response.End();
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    string[] roles = ticket.UserData.Split(',');
                    GenericIdentity identity = new GenericIdentity(ticket.Name);
                    HttpContext.Current.User = new GenericPrincipal(identity, roles);
                }
            }
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            IPrincipal principal = HttpContext.Current.User;
            string path = HttpContext.Current.Request.Path.ToLower();

            if (!path.StartsWith("/admin"))
                return;

            if (HttpContext.Current.User?.Identity?.IsAuthenticated != true)
            {
                HttpContext.Current.Response.RedirectToRoute("Account_Login");
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!principal.IsInRole("Admin"))
            {
                HttpContext.Current.Response.StatusCode = 403;
                HttpContext.Current.Response.End();
                return;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };

            Server.Transfer("~/Notification/Error.aspx", preserveForm: false);
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