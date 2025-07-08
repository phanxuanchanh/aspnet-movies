using Common;
using Data.BLL;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Admin.ActorManagement
{
    public partial class EditActor : System.Web.UI.Page
    {
        private CustomValidation customValidation;
        protected ExecResult<ActorDto> commandResult;
        protected bool isCreateAction;
        protected bool enableShowResult;

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
            enableShowResult = false;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_ActorList", null);
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
                        await LoadActor(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : long.Parse(Request.QueryString["Id"]));
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task LoadActor(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_ActorList", null);
                return;
            }

            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
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
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvActorName,
                "txtActorName",
                "Tên diễn viên không hợp lệ",
                true,
                null,
                customValidation.ValidateCastName
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
            using(PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                commandResult = await peopleService.AddActorAsync(actor);
            }

            enableShowResult = true;
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateActorDto actor = InitUpdateActorDto();
            using (PeopleService peopleService = NinjectWebCommon.Kernel.Get<PeopleService>())
            {
                commandResult = await peopleService.UpdateActorAsync(actor);
            }

            enableShowResult = true;
        }
    }
}