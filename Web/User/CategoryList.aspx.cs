using Data.BLL;
using System;
using System.Data;
using Web.Models;

namespace Web.User
{
    public partial class CategoryList : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (FilmBLL filmBLL = new FilmBLL())
                {
                    object obj = await filmBLL.CountFilmByCategoryAsync();
                    if (obj is DataSet)
                    {
                        DataSet dataSet = (DataSet)obj;
                        DataTable dataTable = dataSet.Tables[0]; ;

                        DataTable dtTable = new DataTable();
                        dtTable.Columns.Add("name", typeof(string));
                        dtTable.Columns.Add("count", typeof(int));
                        dtTable.Columns.Add("url", typeof(string));

                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            DataRow newRow = dtTable.NewRow();
                            string categoryName = dataRow["name"].ToString();
                            string categoryId = dataRow["ID"].ToString();
                            newRow["name"] = categoryName;
                            newRow["count"] = dataRow["count"];
                            newRow["url"] = GetRouteUrl("User_FilmsByCategory", new { slug = categoryName.TextToUrl(), id = categoryId });
                            dtTable.Rows.Add(newRow);
                        }

                        grvCategoryList.DataSource = dtTable;
                        grvCategoryList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}