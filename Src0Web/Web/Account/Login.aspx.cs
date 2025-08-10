using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Web.App_Start;
using Web.Shared.Result;
using Web.Validation;

namespace Web.Account
{
    public partial class Login : GeneralPage
    {
        private CustomValidation customValidation;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();

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
            customValidation.Init(
                cvUsername,
                "txtUsername",
                "Không được trống, chỉ chứa a-z, 0-9, _ và -",
                true,
                null,
                customValidation.ValidateUsername
            );
            customValidation.Init(
                cvPassword,
                "txtPassword",
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                customValidation.ValidatePassword
            );
        }

        private void ValidateData()
        {
            cvUsername.Validate();
            cvPassword.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return (cvUsername.IsValid && cvPassword.IsValid);
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
            if (!IsValidData())
                return;

            UserLogin userLogin = GetUserLogin();
            using (UserService userService = NinjectWebCommon.Kernel.Get<UserService>())
            {
                ExecResult commandResult = await userService.LoginAsync(userLogin);
                if (commandResult.Status != ExecStatus.Success)
                {
                    notifControl.Set(commandResult);
                    return;
                }

                UserDto user = (await userService.GetUserByUsernameAsync(userLogin.UserName)).Data;
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

                if (!user.Activated)
                {
                    //Xử lý gửi mã kích hoạt
                }

                if (user.Role.Name == "User")
                    Response.RedirectToRoute("User_Home", null);
                else
                    Response.RedirectToRoute("Admin_Overview", null);
            }
        }

    }
}