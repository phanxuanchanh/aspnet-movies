using Common;
using Common.Web;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Web.App_Start;
using Web.Models;
using Web.Shared.Result;
using Web.Validation;

namespace Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateDetail = null;

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
            return (Session["userSession"] != null);
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
                ExecResult result = await userService.LoginAsync(userLogin);
                if (result.Status != ExecStatus.Success)
                {
                    //Chèn uc thông báo
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
                    ConfirmCode confirmCode = new ConfirmCode();
                    Session["confirmCode"] = confirmCode.Send(user.Email);
                    string confirmToken = confirmCode.CreateToken();
                    Session["confirmToken"] = confirmToken;

                    Response.RedirectToRoute("Account_Confirm", new
                    {
                        userId = user.ID,
                        confirmToken = confirmToken,
                        type = "login_unconfirmed"
                    });
                }

                if (user.Role.Name == "User")
                    Response.RedirectToRoute("User_Home", null);
                else
                    Response.RedirectToRoute("Admin_Overview", null);
            }
        }

    }
}