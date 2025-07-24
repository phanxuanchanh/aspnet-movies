﻿using Common;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class NewPassword : System.Web.UI.Page
    {
        private CustomValidation customValidation;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            try
            {
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
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
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
            ExecResult commandResult = null;
            using (UserService userService = NinjectWebCommon.Kernel.Get<UserService>())
            {
                commandResult = await userService.CreateNewPasswordAsync(userId, password);
            }

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