using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Web.Shared.Helpers;
using Web.Shared.Result;

namespace Web.Account
{
    public partial class Login : GeneralPage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            InitHyperLink();
            InitValidation();
            if (CheckLoggedIn())
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            if (IsPostBack)
            {
                await LoginToAccount();
            }
        }

        private void InitHyperLink()
        {
            hylnkResetPassword.NavigateUrl = GetRouteUrl("Account_ResetPassword", null);
            hylnkRegister.NavigateUrl = GetRouteUrl("Account_Register", null);
        }

        private void InitValidation()
        {
            cvUsername.SetValidator(
                nameof(txtUsername), 
                "Không được trống, chỉ chứa a-z, 0-9, _ và -", 
                true, null, 
                CustomValidation.ValidateUsername
            );

            cvPassword.SetValidator(
                nameof(txtPassword),
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true, null,
                CustomValidation.ValidatePassword
            );
        }

        private bool CheckLoggedIn()
        {
            return !(HttpContext.Current.User?.Identity?.IsAuthenticated != true);
        }

        private UserLogin GetUserLogin()
        {
            return new UserLogin
            {
                UserName = txtUsername.Text,
                Password = txtPassword.Text
            };
        }

        private async Task LoginToAccount()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UserLogin userLogin = GetUserLogin();
            UserService userService = Inject<UserService>();

            ExecResult<UserDto> commandResult = await userService.LoginAsync(userLogin);
            if (commandResult.Status != ExecStatus.Success)
            {
                notifControl.Set(commandResult);
                return;
            }

            UserDto user = commandResult.Data;
            if (!user.Activated)
            {
                string token = ActivationToken.Generate();

                commandResult.Message = "User is not activated. Please check the email we sent.";
                notifControl.Set(commandResult);
                return;
            }

            string userData = $"{string.Join(",", user.Role.Name)}";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(30),
                false, userData, FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
                Expires = ticket.Expiration
            };

            Response.Cookies.Add(cookie);
            Response.RedirectToRoute("User_Home", null);
        }

    }
}