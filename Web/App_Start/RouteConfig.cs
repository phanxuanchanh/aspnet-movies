using System.Web.Routing;

namespace Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Account_Login", "account/login", "~/Account/Login.aspx");
            routes.MapPageRoute("Account_Logout", "account/logout", "~/Account/Logout.aspx");
            routes.MapPageRoute("Account_Register", "account/register", "~/Account/Register.aspx");
            routes.MapPageRoute("Account_ResetPassword", "account/reset-password", "~/Account/ResetPassword.aspx");
            routes.MapPageRoute("Account_NewPassword", "account/new-password/{userId}/{newPasswordToken}", "~/Account/NewPassword.aspx");
            routes.MapPageRoute("Account_Confirm", "account/confirm/{userId}/{confirmToken}/{type}", "~/Account/Confirm.aspx");

            routes.MapPageRoute("Admin_Overview", "admin/overview", "~/Admin/Index.aspx");
            routes.MapPageRoute("Admin_FilmList", "admin/film/list", "~/Admin/FilmManagement/FilmList.aspx");
            routes.MapPageRoute("Admin_FilmDetail", "admin/film/detail/{id}", "~/Admin/FilmManagement/FilmDetail.aspx");
            routes.MapPageRoute("Admin_CreateFilm", "admin/film/create", "~/Admin/FilmManagement/CreateFilm.aspx");
            routes.MapPageRoute("Admin_EditCategory_Film", "admin/film/edit-category/{id}", "~/Admin/FilmManagement/EditCategory.aspx");
            routes.MapPageRoute("Admin_EditTag_Film", "admin/film/edit-tag/{id}", "~/Admin/FilmManagement/EditTag.aspx");
            routes.MapPageRoute("Admin_EditDirector_Film", "admin/film/edit-director/{id}", "~/Admin/FilmManagement/EditDirector.aspx");
            routes.MapPageRoute("Admin_EditCast_Film", "admin/film/edit-cast/{id}", "~/Admin/FilmManagement/EditCast.aspx");
            routes.MapPageRoute("Admin_EditImage_Film", "admin/film/edit-image/{id}", "~/Admin/FilmManagement/EditImage.aspx");
            routes.MapPageRoute("Admin_EditSource_Film", "admin/film/edit-source/{id}", "~/Admin/FilmManagement/EditSource.aspx");
            routes.MapPageRoute("Admin_UpdateFilm", "admin/film/update/{id}", "~/Admin/FilmManagement/UpdateFilm.aspx");
            routes.MapPageRoute("Admin_DeleteFilm", "admin/film/delete/{id}", "~/Admin/FilmManagement/DeleteFilm.aspx");
            routes.MapPageRoute("Admin_CategoryList", "admin/category/list", "~/Admin/CategoryManagement/CategoryList.aspx");
            routes.MapPageRoute("Admin_CategoryDetail", "admin/category/detail/{id}", "~/Admin/CategoryManagement/CategoryDetail.aspx");
            routes.MapPageRoute("Admin_CreateCategory", "admin/category/create", "~/Admin/CategoryManagement/CreateCategory.aspx");
            routes.MapPageRoute("Admin_UpdateCategory", "admin/category/update/{id}", "~/Admin/CategoryManagement/UpdateCategory.aspx");
            routes.MapPageRoute("Admin_DeleteCategory", "admin/category/delete/{id}", "~/Admin/CategoryManagement/DeleteCategory.aspx");
            routes.MapPageRoute("Admin_RoleList", "admin/role/list", "~/Admin/RoleManagement/RoleList.aspx");
            routes.MapPageRoute("Admin_RoleDetail", "admin/role/detail/{id}", "~/Admin/RoleManagement/RoleDetail.aspx");
            routes.MapPageRoute("Admin_CreateRole", "admin/role/create", "~/Admin/RoleManagement/CreateRole.aspx");
            routes.MapPageRoute("Admin_UpdateRole", "admin/role/update/{id}", "~/Admin/RoleManagement/UpdateRole.aspx");
            routes.MapPageRoute("Admin_DeleteRole", "admin/role/delete/{id}", "~/Admin/RoleManagement/DeleteRole.aspx");

            routes.MapPageRoute("Admin_CountryList", "admin/country/list", "~/Admin/CountryManagement/CountryList.aspx");
            routes.MapPageRoute("Admin_CountryDetail", "admin/country/detail/{id}", "~/Admin/CountryManagement/CountryDetail.aspx");
            routes.MapPageRoute("Admin_EditCountry", "admin/country/edit", "~/Admin/CountryManagement/EditCountry.aspx");
            
            routes.MapPageRoute("Admin_LanguageList", "admin/language/list", "~/Admin/LanguageManagement/LanguageList.aspx");
            routes.MapPageRoute("Admin_LanguageDetail", "admin/language/detail/{id}", "~/Admin/LanguageManagement/LanguageDetail.aspx");
            routes.MapPageRoute("Admin_EditLanguage", "admin/language/edit", "~/Admin/LanguageManagement/EditLanguage.aspx");
            
            routes.MapPageRoute("Admin_DirectorList", "admin/director/list", "~/Admin/DirectorManagement/DirectorList.aspx");
            routes.MapPageRoute("Admin_DirectorDetail", "admin/director/detail/{id}", "~/Admin/DirectorManagement/DirectorDetail.aspx");
            routes.MapPageRoute("Admin_CreateDirector", "admin/director/create", "~/Admin/DirectorManagement/CreateDirector.aspx");
            routes.MapPageRoute("Admin_UpdateDirector", "admin/director/update/{id}", "~/Admin/DirectorManagement/UpdateDirector.aspx");
            routes.MapPageRoute("Admin_DeleteDirector", "admin/director/delete/{id}", "~/Admin/DirectorManagement/DeleteDirector.aspx");
            routes.MapPageRoute("Admin_CastList", "admin/cast/list", "~/Admin/CastManagement/CastList.aspx");
            routes.MapPageRoute("Admin_CastDetail", "admin/cast/detail/{id}", "~/Admin/CastManagement/CastDetail.aspx");
            routes.MapPageRoute("Admin_CreateCast", "admin/cast/create", "~/Admin/CastManagement/CreateCast.aspx");
            routes.MapPageRoute("Admin_UpdateCast", "admin/cast/update/{id}", "~/Admin/CastManagement/UpdateCast.aspx");
            routes.MapPageRoute("Admin_DeleteCast", "admin/cast/delete/{id}", "~/Admin/CastManagement/DeleteCast.aspx");
            routes.MapPageRoute("Admin_TagList", "admin/tag/list", "~/Admin/TagManagement/TagList.aspx");
            routes.MapPageRoute("Admin_TagDetail", "admin/tag/detail/{id}", "~/Admin/TagManagement/TagDetail.aspx");
            routes.MapPageRoute("Admin_CreateTag", "admin/tag/create", "~/Admin/TagManagement/CreateTag.aspx");
            routes.MapPageRoute("Admin_UpdateTag", "admin/tag/update/{id}", "~/Admin/TagManagement/UpdateTag.aspx");
            routes.MapPageRoute("Admin_DeleteTag", "admin/tag/delete/{id}", "~/Admin/TagManagement/DeleteTag.aspx");        
            routes.MapPageRoute("Admin_UserList", "admin/user/list", "~/Admin/UserManagement/UserList.aspx");
            routes.MapPageRoute("Admin_CreateUser", "admin/user/create", "~/Admin/UserManagement/CreateUser.aspx");

            routes.MapPageRoute("User_Home", "", "~/User/Index.aspx");
            routes.MapPageRoute("User_CategoryList", "category-list", "~/User/CategoryList.aspx");
            routes.MapPageRoute("User_FilmDetail", "film-detail/{slug}/{id}", "~/User/FilmDetail.aspx");
            routes.MapPageRoute("User_Watch", "watch/{slug}/{id}", "~/User/Watch.aspx");
            routes.MapPageRoute("User_Search", "search/", "~/User/Search.aspx");
            routes.MapPageRoute("User_WatchedList", "watched-list", "~/User/WatchedList.aspx");
            routes.MapPageRoute("User_FilmsByCategory", "films-by-category/{slug}/{id}", "~/User/FilmsByCategory.aspx");
            routes.MapHttpHandlerRoute("User_UpvoteFilm", "film/upvote", "~/User/UpvoteFilm.ashx");
            routes.MapHttpHandlerRoute("User_DownvoteFilm", "film/downvote", "~/User/DownvoteFilm.ashx");
            routes.MapHttpHandlerRoute("User_IncreaseView", "film/increase-view", "~/User/IncreaseView.ashx");

            routes.MapPageRoute("User_Home_Lite", "lite-version/", "~/User/LiteVersion/IndexLite.aspx");
            routes.MapPageRoute("User_FilmDetail_Lite", "lite-version/film-detail/{slug}/{id}", "~/User/LiteVersion/FilmDetailLite.aspx");
            routes.MapPageRoute("User_Watch_Lite", "lite-version/watch/{slug}/{id}", "~/User/LiteVersion/WatchLite.aspx");
            routes.MapPageRoute("User_FilmsByCategory_Lite", "lite-version/films-by-category/{slug}/{id}", "~/User/LiteVersion/FilmsByCategoryLite.aspx");

            routes.MapPageRoute("Notification_Error", "notification/error", "~/Notification/Error.aspx");

            routes.MapPageRoute("Install", "install/run-setup", "~/Install/Index.aspx");
        }
    }
}