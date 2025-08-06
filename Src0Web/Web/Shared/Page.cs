using Ninject;
using System;
using System.Web.UI;
using Web.App_Start;

namespace Web.Shared
{
    public class GeneralPage : System.Web.UI.Page
    {
        public bool SkipRender { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            NinjectWebCommon.Kernel.Inject(this);
        }

        public T Inject<T>() where T : IDisposable
        {
            return NinjectWebCommon.Kernel.Get<T>();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if(!SkipRender)
                base.Render(writer);
        }
    }
}