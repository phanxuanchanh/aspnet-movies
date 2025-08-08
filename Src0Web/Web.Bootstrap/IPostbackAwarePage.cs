using System;

namespace Web
{
    public interface IPostbackAwarePage
    {
        void Page_LoadNoPostback(object sender, EventArgs e);
        void Page_LoadWithPostback(object sender, EventArgs e);
    }
}