using Ninject;
using Web.App_Start;

namespace Web.App_Code
{
    public class GeneralPage : System.Web.UI.Page
    {
        public T Inject<T>() where T : class
        {
            return NinjectWebCommon.Kernel.Get<T>();
        }
    }
}