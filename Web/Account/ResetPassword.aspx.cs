using Common.Web;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
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
            catch(Exception ex)
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
            if (IsValidData())
            {
                string email = GetEmailAddress();
                UserInfo userInfo;
                using(UserBLL userBLL = new UserBLL())
                {
                    userInfo = await userBLL.GetUserByEmailAsync(email);
                }

                if(userInfo == null)
                {
                    enableShowResult = true;
                    stateDetail = "Không tồn tại tài khoản có địa chỉ Email này";
                }
                else
                {
                    ConfirmCode confirmCode = new ConfirmCode();
                    Session["confirmCode"] = confirmCode.Send(userInfo.email);
                    string confirmToken = confirmCode.CreateToken();
                    Session["confirmToken"] = confirmToken;

                    Response.RedirectToRoute("Account_Confirm", new
                    {
                        userId = userInfo.ID,
                        confirmToken = confirmToken,
                        type = "reset-password"
                    });
                }
            }
        }
    }
}