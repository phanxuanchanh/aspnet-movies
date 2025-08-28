using Common.Web;
using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Shared.Result;

namespace Web.Account
{
    public partial class Confirm : GeneralPage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            InitHyperlink();
            InitValidation();
            if (Session["confirmCode"] == null || Session["confirmToken"] == null)
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            if (!IsValidConfirmToken())
            {
                Response.RedirectToRoute("User_Home", null);
                return;
            }

            if (IsPostBack)
                await ConfirmAccount();
            else
                await ReSendConfirmCode();
        }

        private void InitHyperlink()
        {
            hylnkReConfirm.NavigateUrl = GetRouteUrl("Account_Confirm", new
            {
                userId = GetUserId(),
                confirmToken = GetConfirmToken(),
                type = "re-confirm"
            });
        }

        private void InitValidation()
        {
            cvConfirmCode.SetValidator(
                nameof(txtConfirmCode),
                "Không được để trống, từ 6 đến 20 ký tự số",
                true,
                null,
                CustomValidation.ValidateConfirmCode
            );
        }

        private void ValidateData()
        {
            cvConfirmCode.Validate();
        }

        public bool IsValidData()
        {
            ValidateData();
            return cvConfirmCode.IsValid;
        }

        private bool IsReConfirm()
        {
            string status = (string)Page.RouteData.Values["type"];
            return (status == "re-confirm");
        }

        private bool IsResetPassword()
        {
            string status = (string)Page.RouteData.Values["type"];
            return (status == "reset-password" || status == "reset-password-failed");
        }

        private async Task ReSendConfirmCode()
        {
            if (!IsReConfirm())
                return;

            UserService userService = Inject<UserService>();

            ExecResult<UserDto> result = await userService.GetUserAsync(GetUserId());
            if (result.Status != ExecStatus.Success)
            {
                Response.RedirectToRoute("Notification_Error");
                return;
            }

            Session["confirmCode"] = new ConfirmCode().Send(result.Data.Email);
        }

        private bool IsValidConfirmToken()
        {
            return GetConfirmToken() == Session["confirmToken"] as string;
        }

        private bool CheckConfirmCode()
        {
            string confirmCode = Request.Form["txtConfirmCode"];
            return (confirmCode == Session["confirmCode"] as string);
        }

        private string GetUserId()
        {
            return (string)Page.RouteData.Values["userId"];
        }

        private string GetConfirmToken()
        {
            return (string)Page.RouteData.Values["confirmToken"];
        }

        private async Task ConfirmAccount()
        {
            if (IsValidData() && CheckConfirmCode())
            {
                string userId = GetUserId();
                ExecResult commandResult = null;
                UserService userService = Inject<UserService>();

                commandResult = await userService.ActiveUserAsync(userId);

                Session["confirmCode"] = null;
                Session["confirmToken"] = null;

                if (commandResult.Status == ExecStatus.NotFound)
                {
                    Response.RedirectToRoute("User_Home", null);
                    return;
                }

                if (commandResult.Status != ExecStatus.Success)
                {
                    Response.RedirectToRoute("Notification_Error", null);
                    return;
                }

                if (IsResetPassword())
                {
                    ConfirmCode confirmCode = new ConfirmCode();
                    string newPasswordToken = confirmCode.CreateToken();
                    Session["newPasswordToken"] = newPasswordToken;
                    Response.RedirectToRoute("Account_NewPassword", new { userId = userId, newPasswordToken = newPasswordToken });
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
            }
            else if (IsResetPassword())
            {

                Response.RedirectToRoute("Account_Confirm", new
                {
                    userId = GetUserId(),
                    type = "reset-password-failed"
                });
            }
            else
            {
                Response.RedirectToRoute("Account_Confirm", new
                {
                    userId = GetUserId(),
                    type = "confirm-failed"
                });
            }
        }
    }
}