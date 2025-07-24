using Common;

namespace Web.Admin.Base
{
    public partial class Notification : System.Web.UI.UserControl
    {
        protected bool enableShowResult;
        protected ExecResult commandResult;

        public Notification()
        {
            enableShowResult = false;
            commandResult = null;
        }

        public void Set(ExecResult result)
        {
            commandResult = result;
            enableShowResult = true;
        }

        public void Set<T>(ExecResult<T> result)
        {
            commandResult = result;
            enableShowResult = true;
        }
    }
}
