
namespace System.Web.Routing
{
    public class HttpHandlerRoute<T> : IRouteHandler where T : IHttpHandler
    {
        private String _virtualPath = null;

        public HttpHandlerRoute(String virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public HttpHandlerRoute() { }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return Activator.CreateInstance<T>();
        }
    }

    public class HttpHandlerRoute : IRouteHandler
    {
        private String _virtualPath = null;

        public HttpHandlerRoute(String virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (!string.IsNullOrEmpty(_virtualPath))
            {
                return (IHttpHandler)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(IHttpHandler));
            }
            else
            {
                throw new InvalidOperationException("HttpHandlerRoute threw an error because the virtual path to the HttpHandler is null or empty.");
            }
        }
    }

    public static class RoutingExtension
    {
        public static void MapHttpHandlerRoute(this RouteCollection routes, string routeName, string routeUrl, string physicalFile, RouteValueDictionary defaults = null, RouteValueDictionary constraints = null)
        {
            var route = new Route(routeUrl, defaults, constraints, new HttpHandlerRoute(physicalFile));
            routes.Add(routeName, route);
        }

        public static void MapHttpHandlerRoute<T>(this RouteCollection routes, string routeName, string routeUrl, RouteValueDictionary defaults = null, RouteValueDictionary constraints = null) where T : IHttpHandler
        {
            var route = new Route(routeUrl, defaults, constraints, new HttpHandlerRoute<T>());
            routes.Add(routeName, route);
        }
    }
}
