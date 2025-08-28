using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.ActorManagement
{
    public partial class EditActor : AdminPage
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_ActorList", null);
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
                    await LoadActor(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : long.Parse(Request.QueryString["Id"]));
            }
        }

        private async Task LoadActor(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_ActorList", null);
                return;
            }

            PeopleService peopleService = Inject<PeopleService>();

            ExecResult<ActorDto> result = await peopleService.GetActorAsync(id);
            if (result.Status == ExecStatus.Success)
            {
                hdActorId.Value = result.Data.ID.ToString();
                txtActorName.Text = result.Data.Name;
                txtActorDescription.Text = result.Data.Description;
            }
            else
            {
                Response.RedirectToRoute("Admin_ActorList", null);
            }
        }

        private void InitValidation()
        {
            cvActorName.SetValidator(
                nameof(txtActorName),
                "Tên diễn viên không hợp lệ",
                true,
                null,
                CustomValidation.ValidateCastName
            );
        }

        private void ValidateData()
        {
            cvActorName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvActorName.IsValid;
        }

        private CreateActorDto InitCreateActorDto()
        {
            return new CreateActorDto
            {
                Name = Request.Form[txtActorName.UniqueID],
                Description = Request.Form[txtActorDescription.UniqueID]
            };
        }

        private UpdateActorDto InitUpdateActorDto()
        {
            return new UpdateActorDto
            {
                ID = long.Parse(Request.Form[hdActorId.UniqueID]),
                Name = Request.Form[txtActorName.UniqueID],
                Description = Request.Form[txtActorDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateActorDto actor = InitCreateActorDto();
            PeopleService peopleService = Inject<PeopleService>();

            ExecResult<ActorDto> commandResult = await peopleService.AddActorAsync(actor);
            notifControl.Set<ActorDto>(commandResult);
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateActorDto actor = InitUpdateActorDto();
            PeopleService peopleService = Inject<PeopleService>();

            ExecResult<ActorDto> commandResult = await peopleService.UpdateActorAsync(actor);
            notifControl.Set<ActorDto>(commandResult);
        }
    }
}