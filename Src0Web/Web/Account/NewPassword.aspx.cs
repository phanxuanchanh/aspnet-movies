using Data.Services;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Shared.Result;

namespace Web.Account
{
    public partial class NewPassword : GeneralPage
    {
        private CustomValidation customValidation;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();

            InitValidation();

            if (Session["newPasswordToken"] == null)
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else if (!IsValidNewPasswordToken())
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else
            {
                if (IsPostBack)
                {
                    await CreateNewPassword();
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvPassword,
                "txtNewPassword",
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                customValidation.ValidatePassword
            );
        }

        private void ValidateData()
        {
            cvPassword.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvPassword.IsValid;
        }

        private string GetUserId()
        {
            return (string)Page.RouteData.Values["userId"];
        }

        private string GetNewPassword()
        {
            return Request.Form["txtNewPassword"];
        }

        private string GetNewPasswordToken()
        {
            return (string)Page.RouteData.Values["newPasswordToken"];
        }

        private bool IsValidNewPasswordToken()
        {
            return GetNewPasswordToken() == Session["newPasswordToken"] as string;
        }

        private async Task CreateNewPassword()
        {
            if (!IsValidData())
                return;

            string userId = GetUserId();
            string password = GetNewPassword();

            UserService userService = Inject<UserService>();

            ExecResult commandResult = await userService.CreateNewPasswordAsync(userId, password);

            Session["newPasswordToken"] = null;
            if (commandResult.Status == ExecStatus.Success)
                Response.RedirectToRoute("Account_Login", null);
            else if (commandResult.Status == ExecStatus.NotFound)
                Response.RedirectToRoute("User_Home", null);
            else
                Response.RedirectToRoute("Notification_Error", null);
        }
    }
}