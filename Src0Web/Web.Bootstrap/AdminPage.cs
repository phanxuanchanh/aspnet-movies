using Ninject;
using System;

namespace Web
{
    public class AdminPage : GeneralPage
    {
        protected T GetId<T>(string key = "id", bool fromQueryString = false)
        {
            object obj = null;
            if (fromQueryString)
                obj = Request.QueryString[key];
            else
                obj = Page.RouteData.Values[key];

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

        public event EventHandler OnLoadNoPostback;
        public event EventHandler OnLoadWithPostback;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if(this is IPostbackAwarePage)
            {
                TryAutoBindEventHandler(nameof(IPostbackAwarePage.Page_LoadNoPostback), ref OnLoadNoPostback);
                TryAutoBindEventHandler(nameof(IPostbackAwarePage.Page_LoadWithPostback), ref OnLoadWithPostback);
            }
        }

        private void TryAutoBindEventHandler(string methodName, ref EventHandler eventField)
        {
            var method = GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Public);

            if (method != null)
            {
                var handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, method);
                eventField += handler;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(this is IPostbackAwarePage)
            {
                if (IsPostBack)
                    OnLoadWithPostback?.Invoke(this, e);
                else
                    OnLoadNoPostback?.Invoke(this, e);
            }   
        }
    }
}