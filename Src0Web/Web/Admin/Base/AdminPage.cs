
using Common;
using System;
using System.Threading.Tasks;
using NotifControl = Web.Admin.Base.Notification;

namespace Web.Admin
{
    public class AdminPage : Web.Shared.GeneralPage
    {
        protected T GetId<T>(string key = "id")
        {
            object obj = Page.RouteData.Values[key];
            if (obj == null)
                return default;

            var str = obj.ToString();

            if (typeof(T) == typeof(string))
                return (T)(object)str;

            if (typeof(T) == typeof(int) && int.TryParse(str, out int i))
                return (T)(object)i;

            if (typeof(T) == typeof(long) && long.TryParse(str, out long l))
                return (T)(object)l;

            return default;
        }

        //protected async Task Delete<TService, Tid>(TService service, Tid id, NotifControl notifControl, Func<TService, Tid, Task<ExecResult>> func)
        //{
        //    ExecResult commandResult = await func(service, id);
        //    if (commandResult.Status == ExecStatus.Success)
        //    {
        //        Response.RedirectToRoute("Admin_ActorList", null);
        //        return;
        //    }
        //    else
        //    {
        //        notifControl.Set(commandResult);
        //    }
        //}
    }
}