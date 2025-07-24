using Common;
using Common.Web;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            try
            {
                InitValidation();
                if (IsPostBack)
                {
                    await ResetPwd();
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
                cvEmail,
                "txtEmail",
                "Địa chỉ Email không hợp lệ",
                true,
                null,
                customValidation.ValidateEmail
            );
        }

        private void ValidateData()
        {
            cvEmail.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvEmail.IsValid;
        }

        private string GetEmailAddress()
        {
            return Request.Form["txtEmail"];
        }

        private async Task ResetPwd()
        {
            if (!IsValidData())
                return;

            string email = GetEmailAddress();
            using (UserService userService = NinjectWebCommon.Kernel.Get<UserService>())
            {
                ExecResult<UserDto> result = await userService.GetUserByEmailAsync(email);
                if (result.Status != ExecStatus.Success)
                {
                    enableShowResult = true;
                    stateDetail = result.Message;
                    return;
                }

                ConfirmCode confirmCode = new ConfirmCode();
                Session["confirmCode"] = confirmCode.Send(result.Data.Email);
                string confirmToken = confirmCode.CreateToken();
                Session["confirmToken"] = confirmToken;

                Response.RedirectToRoute("Account_Confirm", new
                {
                    userId = result.Data.ID,
                    confirmToken = confirmToken,
                    type = "reset-password"
                });
            }
        }
    }
}