using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using System.Web;
using Web.Shared.Result;

namespace Web.Account
{
    public partial class Register : GeneralPage
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
                await RegisterAccount();
            }
        }

        private bool CheckLoggedIn()
        {
            return !(HttpContext.Current.User?.Identity?.IsAuthenticated != true);
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
                UserName = txtUsername.Text,
                Email = txtEmail.Text,
                PhoneNumber = txtPhoneNumber.Text,
                Password = txtPassword.Text,
            };
        }

        private async Task RegisterAccount()
        {
            if (!IsValidData())
                return;

            CreateUserDto user = InitCreateUserDto();
            UserService userService = Inject<UserService>();

            ExecResult commandResult = await userService.RegisterAsync(user);
            if(commandResult.Status == ExecStatus.Success)
            {
                Response.RedirectToRoute("Account_Login");
                return;
                
            }

            notifControl.Set(commandResult);
        }
    }
}