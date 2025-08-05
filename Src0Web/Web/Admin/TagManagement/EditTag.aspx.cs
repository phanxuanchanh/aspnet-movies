using Common;
using Data.DTO;
using Data.Services;
using Ninject;
using System;
using System.Threading.Tasks;
using Web.App_Start;
using Web.Models;
using Web.Validation;

namespace Web.Admin.TagManagement
{
    public partial class EditTag : AdminPage
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
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
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
                    await LoadTag(string.IsNullOrEmpty(Request.QueryString["Id"]) ? -1 : int.Parse(Request.QueryString["Id"]));
            }
        }

        private async Task LoadTag(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
                return;
            }

            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<TagDto> result = await taxonomyService.GetTagAsync(id);
                if (result.Status == ExecStatus.Success)
                {
                    hdTagId.Value = result.Data.ID.ToString();
                    txtTagName.Text = result.Data.Name;
                    txtTagDescription.Text = result.Data.Description;
                }
                else
                {
                    Response.RedirectToRoute("Admin_TagList", null);
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvTagName,
                "txtTagName",
                "Tên thẻ tag không hợp lệ",
                true,
                null,
                customValidation.ValidateTagName
            );
        }

        private void ValidateData()
        {
            cvTagName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvTagName.IsValid;
        }

        private CreateTagDto InitCreateTagDto()
        {
            return new CreateTagDto
            {
                Name = Request.Form[txtTagName.UniqueID],
                Description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        private UpdateTagDto InitUpdateTagDto()
        {
            return new UpdateTagDto
            {
                ID = int.Parse(Request.Form[hdTagId.UniqueID]),
                Name = Request.Form[txtTagName.UniqueID],
                Description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (!IsValidData())
                return;

            CreateTagDto tag = InitCreateTagDto();
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<TagDto> commandResult = await taxonomyService.AddTagAsync(tag);
                notifControl.Set<TagDto>(commandResult);
            }
        }

        public async Task Update()
        {
            if (!IsValidData())
                return;

            UpdateTagDto tag = InitUpdateTagDto();
            using (TaxonomyService taxonomyService = NinjectWebCommon.Kernel.Get<TaxonomyService>())
            {
                ExecResult<TagDto> commandResult = await taxonomyService.UpdateTagAsync(tag);
                notifControl.Set<TagDto>(commandResult);
            }
        }
    }
}