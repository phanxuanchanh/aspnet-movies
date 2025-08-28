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
        protected async void Page_Load(object sender, EventArgs e)
        {
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
            cvUsername.SetValidator(
                nameof(txtUsername),
                "Không được trống, chỉ chứa a-z, 0-9, _ và -",
                true,
                null,
                CustomValidation.ValidateUsername
            );

            cvEmail.SetValidator(
                nameof(txtEmail),
                "Không được để trống và phải hợp lệ",
                true,
                null,
                CustomValidation.ValidateEmail
            );

            cvPhoneNumber.SetValidator(
                nameof(txtPhoneNumber),
                "Số điện thoại không hợp lệ",
                false,
                null,
                CustomValidation.ValidatePhoneNumber
            );

            cvPassword.SetValidator(
                nameof(txtPassword),
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                CustomValidation.ValidatePassword
            );

            cmpRePassword.ControlToValidate = "txtPassword";
            cmpRePassword.ControlToCompare = "txtRePassword";
            cmpRePassword.ErrorMessage = "Không khớp với mật khẩu mà bạn đã nhập";
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
            Page.Validate();

            if(!Page.IsValid)
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