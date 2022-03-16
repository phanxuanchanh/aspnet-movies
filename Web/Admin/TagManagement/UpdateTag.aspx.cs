using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.TagManagement
{
    public partial class UpdateTag : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        protected TagInfo tagInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            tagBLL = new TagBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                long id = GetTagId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_TagDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteTag", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadTagInfo(id);
                        }
                    }
                    else
                    {
                        await LoadTagInfo(id);
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            tagBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private long GetTagId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task LoadTagInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
            }
            else
            {
                tagBLL.IncludeDescription = true;
                TagInfo tagInfo = await tagBLL.GetTagAsync(id);
                if (tagInfo == null)
                {
                    Response.RedirectToRoute("Admin_TagList", null);
                }
                else
                {
                    hdTagId.Value = tagInfo.ID.ToString();
                    txtTagName.Text = tagInfo.name;
                    txtTagDescription.Text = tagInfo.description;
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

        private TagUpdate GetTagUpdate()
        {
            return new TagUpdate
            {
                ID = long.Parse(Request.Form[hdTagId.UniqueID]),
                name = Request.Form[txtTagName.UniqueID],
                description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            TagUpdate tagUpdate = GetTagUpdate();
            UpdateState state = await tagBLL.UpdateTagAsync(tagUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật thẻ tag thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật thẻ tag thất bại";
            }
            enableShowResult = true;
        }
    }
}