using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.RoleManagement
{
    public partial class EditRole : AdminPage
    {
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
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
            cvRoleName.SetValidator(
                nameof(txtRoleName),
                "Tên vai trò không được để trống",
                true,
                null,
                CustomValidation.ValidateRoleName
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateRoleDto createRoleDto = InitCreateRoleDto();
            RoleService roleService = Inject<RoleService>();
            ExecResult<RoleDto> commandResult = await roleService.CreateAsync(createRoleDto);
            notifControl.Set<RoleDto>(commandResult);
        }

        public async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateRoleDto createRoleDto = InitUpdateRoleDto();
            RoleService roleService = Inject<RoleService>();

            ExecResult commandResult = await roleService.UpdateAsync(createRoleDto);
            notifControl.Set(commandResult);
        }
    }
}