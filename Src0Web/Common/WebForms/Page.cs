using Ninject;
using System;
using System.Web.UI;

namespace Web.Shared.WebForms
{
    public class GeneralPage : System.Web.UI.Page
    {
        public static IKernel Kernel;

        public bool SkipRender { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Kernel.Inject(this);
        }

        public T Inject<T>() where T : IDisposable
        {
            return Kernel.Get<T>();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!SkipRender)
                base.Render(writer);
        }
    }
}