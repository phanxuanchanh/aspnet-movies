using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.DirectorManagement
{
    public partial class EditDirector : AdminPage
    {
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
            InitValidation();

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

            PeopleService peopleService = Inject<PeopleService>();

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

        private void InitValidation()
        {
            cvDirectorName.SetValidator(
                nameof(txtDirectorName),
                "Tên đạo diễn không hợp lệ",
                true,
                null,
                CustomValidation.ValidateDirectorName
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateDirectorDto director = InitCreateDirectorDto();
            PeopleService peopleService = Inject<PeopleService>();

            ExecResult<DirectorDto> commandResult = await peopleService.AddDirectorAsync(director);
            notifControl.Set<DirectorDto>(commandResult);
        }

        private async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UpdateDirectorDto director = InitUpdateDirectorDto();
            PeopleService peopleService = Inject<PeopleService>();

            ExecResult<DirectorDto> commandResult = await peopleService.UpdateDirectorAsync(director);
            notifControl.Set<DirectorDto>(commandResult);
        }
    }
}