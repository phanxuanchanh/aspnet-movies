using Common.Hash;
using Data.DAL;
using Data.Models;
using Data.Services;
using Ninject;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Web.App_Start;

namespace Web.Install
{
    public partial class AppSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            //AddUser();
            await AddAppSettings();

            Installer.Completed = true;
            Response.RedirectToRoute("User_Home");
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

            AppSettingService appSettingService = NinjectWebCommon.Kernel.Get<AppSettingService>();
            await appSettingService.AddAsync(setting);
        }

        private void AddUser()
        {
            string salt = HashFunction.MD5_Hash(new Random().NextString(25));

            Data.DAL.User user = new Data.DAL.User
            {
            };

            using (DBContext db = new DBContext())
            {
                long recordNumber = db.Users.Count();
                if (recordNumber == 0)
                {
                    Role role = db.Roles.Select(s => new { s.Id }).SingleOrDefault(x => x.Name == "Admin");
                    user.RoleId = role.Id;

                    int affected = db.Users.Insert(user);
                }
            }
        }
    }
}