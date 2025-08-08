using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Shared.Result;
using Web.Shared.WebForms;
using Web.Validation;

namespace Web.Admin.RoleManagement
{
    public partial class EditRole : AdminPage
    {
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
            InitValidation();

            if (IsPostBack)
            {
                await Create();
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvRoleName,
                "txtRoleName",
                "Tên vai trò không hợp lệ",
                true,
                null,
                customValidation.ValidateRoleName
            );
        }

        private void ValidateData()
        {
            cvRoleName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvRoleName.IsValid;
        }

        private CreateRoleDto InitCreateRoleDto()
        {
            return new CreateRoleDto
            {
                Name = Request.Form[txtRoleName.UniqueID],
            };
        }

        private UpdateRoleDto InitUpdateRoleDto()
        {
            return new UpdateRoleDto
            {
                Name = Request.Form[txtRoleName.UniqueID],
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateRoleDto createRoleDto = InitCreateRoleDto();
            using (RoleService roleService = NinjectWebCommon.Kernel.Get<RoleService>())
            {
                ExecResult<RoleDto> commandResult = await roleService.CreateAsync(createRoleDto);
                notifControl.Set<RoleDto>(commandResult);
            }
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            CreateRoleDto createRoleDto = InitUpdateRoleDto();
            using (RoleService roleService = NinjectWebCommon.Kernel.Get<RoleService>())
            {
                ExecResult<RoleDto> commandResult = await roleService.UpdateAsync(createRoleDto);
                notifControl.Set<RoleDto>(commandResult);
            }
        }
    }
}