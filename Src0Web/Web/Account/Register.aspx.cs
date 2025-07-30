using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Register : System.Web.UI.Page
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
                await RegisterAccount();
            }
        }

        private bool CheckLoggedIn()
        {
            return (Session["userSession"] != null);
        }

        private void InitHyperLink()
        {
            hylnkResetPassword.NavigateUrl = GetRouteUrl("Account_ResetPassword", null);
            hylnkLogin.NavigateUrl = GetRouteUrl("Account_Login", null);
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
                cvEmail,
                "txtEmail",
                "Không được để trống và phải hợp lệ",
                true,
                null,
                customValidation.ValidateEmail
            );

            customValidation.Init(
                cvPhoneNumber,
                "txtPhoneNumber",
                "Số điện thoại không hợp lệ",
                false,
                null,
                customValidation.ValidatePhoneNumber
            );

            customValidation.Init(
                cvPassword,
                "txtPassword",
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                customValidation.ValidatePassword
            );

            cmpRePassword.ControlToValidate = "txtPassword";
            cmpRePassword.ControlToCompare = "txtRePassword";
            cmpRePassword.ErrorMessage = "Không khớp với mật khẩu mà bạn đã nhập";
        }

        private void ValidateData()
        {
            cvUsername.Validate();
            cvEmail.Validate();
            cvPhoneNumber.Validate();
            cvPassword.Validate();
            cmpRePassword.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return (
                cvUsername.IsValid && cvEmail.IsValid && cvPhoneNumber.IsValid && cvPassword.IsValid
                && cmpRePassword.IsValid
            );
        }

        private CreateUserDto InitCreateUserDto()
        {
            return new CreateUserDto
            {
                UserName = Request.Form["txtUsername"],
                Email = Request.Form["txtEmail"],
                PhoneNumber = Request.Form["txtPhoneNumber"],
                Password = Request.Form["txtPassword"],
            };
        }

        private async Task RegisterAccount()
        {
            if (!IsValidData())
                return;

            //UserCreation userCreation = GetUserRegister();
            //UserDao.RegisterState registerState = await userBLL.RegisterAsync(userCreation);

            //if (registerState == UserDao.RegisterState.Failed || registerState == UserDao.RegisterState.AlreadyExist)
            //{
            //    if (registerState == UserDao.RegisterState.Failed)
            //        stateDetail = "Đăng ký tài khoản thất bại";
            //    else
            //        stateDetail = "Đã tồn tại tài khoản có thông tin này";

            //    enableShowResult = true;
            //}
            //else
            //{
            //    ConfirmCode confirmCode = new ConfirmCode();
            //    Session["confirmCode"] = confirmCode.Send(userCreation.email);
            //    string confirmToken = confirmCode.CreateToken();
            //    Session["confirmToken"] = confirmToken;

            //    UserInfo userInfo = await userBLL.GetUserByUserNameAsync(userCreation.userName);

            //    if (registerState == UserDao.RegisterState.Success)
            //        Response.RedirectToRoute("Account_Confirm", new
            //        {
            //            userId = userInfo.ID,
            //            confirmToken = confirmToken,
            //            type = "register"
            //        });
            //    else
            //        Response.RedirectToRoute("Account_Confirm", new
            //        {
            //            userId =userInfo.ID,
            //            confirmToken = confirmToken,
            //            type = "register_no-payment-info"
            //        });
            //}
        }
    }
}