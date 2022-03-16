using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Confirm : System.Web.UI.Page
    {
        private CustomValidation customValidation;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            try
            {
                InitHyperlink();
                InitValidation();
                if (Session["confirmCode"] == null || Session["confirmToken"] == null)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else if (!IsValidConfirmToken())
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    if (IsPostBack)
                        await ConfirmAccount();
                    else
                        await ReSendConfirmCode();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
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
            customValidation.Init(
                cvConfirmCode,
                "txtConfirmCode",
                "Không được để trống, từ 6 đến 20 ký tự số",
                true,
                null,
                customValidation.ValidateConfirmCode
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
            if (IsReConfirm())
            {
                UserInfo userInfo;
                using(UserBLL userBLL = new UserBLL())
                {
                    userInfo = await userBLL.GetUserAsync(GetUserId());
                }

                if (userInfo == null)
                    Response.RedirectToRoute("Notification_Error");
                else
                    Session["confirmCode"] = new ConfirmCode().Send(userInfo.email);
            }
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
                UserBLL.ActiveUserState activeUserState;
                using(UserBLL userBLL = new UserBLL())
                {
                    activeUserState = await userBLL.ActiveUserAsync(userId);
                }

                Session["confirmCode"] = null;
                Session["confirmToken"] = null;
                if (activeUserState == UserBLL.ActiveUserState.Success)
                {
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
                else if (activeUserState == UserBLL.ActiveUserState.NotExists)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    Response.RedirectToRoute("Notification_Error", null);
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