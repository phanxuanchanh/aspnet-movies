using System.Web;

namespace Web.Shared.WebForms
{
    public static class HttpResponseExtension
    {
        public static void ForceRedirectToRoute(this HttpResponse httpResponse, GeneralPage page, string routeName, object routeValues = null)
        {
            httpResponse.RedirectToRoute(routeName, routeValues);
            page.SkipRender = true;
        }
    }
}