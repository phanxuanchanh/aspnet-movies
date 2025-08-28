using Data.DTO;
using Data.Services;
using System;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Web.Admin.TagManagement
{
    public partial class EditTag : AdminPage, IPostbackAwarePage
    {
        protected bool isCreateAction;

        protected void Page_Load(object sender, EventArgs e)
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

            hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
            InitValidation();
        }

        public async void Page_LoadNoPostback(object sender, EventArgs e)
        {
            if (isCreateAction)
                return;

            int id = GetId<int>(fromQueryString: true);
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
                return;
            }

            await LoadTag(id);
        }

        public async void Page_LoadWithPostback(object sender, EventArgs e)
        {
            if (isCreateAction)
                await Create();
            else
                await Update();
        }

        private async Task LoadTag(int id)
        {
            TaxonomyService taxonomyService = Inject<TaxonomyService>();
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

        private void InitValidation()
        {
            cvTagName.SetValidator(
                nameof(txtTagName),
                "Tên thẻ tag không được để trống",
                true,
                null,
                CustomValidation.ValidateTagName
            );
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
            Page.Validate();

            if (!Page.IsValid)
                return;

            CreateTagDto tag = InitCreateTagDto();
            TaxonomyService taxonomyService = Inject<TaxonomyService>();

            ExecResult<TagDto> commandResult = await taxonomyService.AddTagAsync(tag);
            notifControl.Set<TagDto>(commandResult);
        }

        public async Task Update()
        {
            Page.Validate();

            if (!Page.IsValid)
                return;

            UpdateTagDto tag = InitUpdateTagDto();
            TaxonomyService taxonomyService = Inject<TaxonomyService>();
            ExecResult<TagDto> commandResult = await taxonomyService.UpdateTagAsync(tag);
            notifControl.Set<TagDto>(commandResult);
        }
    }
}