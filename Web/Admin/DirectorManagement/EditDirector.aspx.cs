using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Validation;

namespace Web.Admin.DirectorManagement
{
    public partial class EditDirector : AdminPage
    {
        private CustomValidation customValidation;
        protected bool isCreateAction;

        protected async void Page_Load(object sender, EventArgs e)
        {
            string action = Request.QueryString["action"];
            if (string.IsNullOrEmpty(action))
            {
                Response.StatusCode = 400;
                Response.ContentType = "text/plain";
                Response.Write("Invalid or missing parameters.");
                Context.ApplicationInstance.CompleteRequest();
            }

            isCreateAction = action == "create";
            btnSubmit.Text = isCreateAction ? "Create" : "Update";

            customValidation = new CustomValidation();
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
            InitValidation();

            if (!CheckLoggedIn())
            {
                Response.RedirectToRoute("Account_Login", null);
                return;
            }

            if (IsPostBack)
            {
                if (isCreateAction)
                    await Create();
                else
                    await Update();
            }
            else
            {
                if (!isCreateAction)
                    await LoadDirector(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : long.Parse(Request.QueryString["Id"]));
            }
        }

        private async Task LoadDirector(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
                return;
            }

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult<DirectorDto> result = await peopleService.GetDirectorAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    hdDirectorId.Value = result.Data.ID.ToString();
                    txtDirectorName.Text = result.Data.Name;
                    txtDirectorDescription.Text = result.Data.Description;
                }
                else
                {
                    Response.RedirectToRoute("Admin_DirectorList", null);
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvDirectorName,
                "txtDirectorName",
                "Tên đạo diễn không hợp lệ",
                true,
                null,
                customValidation.ValidateDirectorName
            );
        }

        private void ValidateData()
        {
            cvDirectorName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvDirectorName.IsValid;
        }

        private CreateDirectorDto InitCreateDirectorDto()
        {
            return new CreateDirectorDto
            {
                Name = Request.Form[txtDirectorName.UniqueID],
                Description = Request.Form[txtDirectorDescription.UniqueID]
            };
        }

        private UpdateDirectorDto InitUpdateDirectorDto()
        {
            return new UpdateDirectorDto
            {
                ID = long.Parse(Request.Form[hdDirectorId.UniqueID]),
                Name = Request.Form[txtDirectorName.UniqueID],
                Description = Request.Form[txtDirectorDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateDirectorDto director = InitCreateDirectorDto();
            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult<DirectorDto> commandResult = await peopleService.AddDirectorAsync(director);
                notifControl.Set<DirectorDto>(commandResult);
            }
        }

        private async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateDirectorDto director = InitUpdateDirectorDto();
            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                ExecResult<DirectorDto> commandResult = await peopleService.UpdateDirectorAsync(director);
                notifControl.Set<DirectorDto>(commandResult);
            }
        }
    }
}