using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CategoryManagement
{
    public partial class UpdateCategory : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected CategoryInfo categoryInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                int id = GetCategoryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CategoryDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCategory", new { id = id });
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        if (IsValidData())
                        {
                            await Update();
                            await LoadCategoryInfo(id);
                        }
                    }
                    else
                    {
                        await LoadCategoryInfo(id);
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
            categoryBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task LoadCategoryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                categoryBLL.IncludeDescription = true;
                CategoryInfo categoryInfo = await categoryBLL.GetCategoryAsync(id);
                if (categoryInfo == null)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
                else
                {
                    hdCategoryId.Value = categoryInfo.ID.ToString();
                    txtCategoryName.Text = categoryInfo.Name;
                    txtCategoryDescription.Text = categoryInfo.Description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCategoryName,
                "txtCategoryName",
                "Tên thể loại không hợp lệ",
                true,
                null,
                customValidation.ValidateCategoryName
            );
        }

        private void ValidateData()
        {
            cvCategoryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCategoryName.IsValid;
        }

        private CategoryUpdate GetCategoryUpdate()
        {
            return new CategoryUpdate
            {
                ID = int.Parse(Request.Form[hdCategoryId.UniqueID]),
                Name = Request.Form[txtCategoryName.UniqueID],
                Description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CategoryUpdate categoryUpdate = GetCategoryUpdate();
            UpdateState state = await categoryBLL.UpdateCategoryAsync(categoryUpdate);
            if (state == UpdateState.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật thể loại thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật thể loại thất bại";
            }
            enableShowResult = true;
        }
    }
}