using Data.DTO;
using Data.Models;
using Data.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Install
{
    public partial class AppSettings : GeneralPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitValidation();
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            await AddUser();
            await AddAppSettings();
            Response.RedirectToRoute("User_Home");
        }

        private void InitValidation()
        {
            
        }

        private async Task AddAppSettings()
        {
            object value = new
            {
                CdnHost = txtCDNHost.Text.Trim(),
                ClientId = txtCDNClientId.Text.Trim(),
                SecretKey = txtCDNSecretKey.Text.Trim(),
            };

            AppSetting setting = new AppSetting
            {
                Name = "cdn-server",
                Value = JsonSerializer.Serialize(value),
            };

            AppSettingService appSettingService = Inject<AppSettingService>();
            await appSettingService.AddAsync(setting);
        }

        private async Task AddUser()
        {
            CreateUserDto user = new CreateUserDto
            {
                UserName = txtAdminUsername.Text.Trim(),
                Password = txtAdminPassword.Text.Trim(),
                Email = txtAdminEmail.Text.Trim()
            };

            UserService userService = Inject<UserService>();
            RoleService roleService = Inject<RoleService>();

            ExecResult<RoleDto> roleResult = await roleService.GetRoleByNameAsync("Admin");

            await userService.RegisterAsync(user);
            await userService.ActiveUserAsync(user.UserName);
        }
    }
}