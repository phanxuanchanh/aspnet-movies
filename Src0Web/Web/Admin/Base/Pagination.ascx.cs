using Common.Web;
using System;

namespace Web.Admin.Base
{
    public partial class Pagination : System.Web.UI.UserControl
    {
        protected long pageSize;
        protected long currentPage;
        protected long totalPages;
        protected long totalItems;
        protected long prevPage;
        protected long nextPage;
        protected long startPage;
        protected long endPage;
        protected bool showPrev;
        protected bool showNext;

        protected string routeName;

        public int MaxVisiblePages { get; set; } = 5;

        public Pagination()
        {

        }

        public void SetUrl(string routeName)
        {
            this.routeName = routeName;
        }

        public void SetAndRender<T>(PagedList<T> pagedList)
        {
            pageSize = (int)pagedList.PageSize;
            currentPage = pagedList.CurrentPage;
            totalPages = pagedList.TotalPages;
            totalItems = pagedList.TotalItems;
            prevPage = Math.Max(1, currentPage - 1);
            nextPage = Math.Min(totalPages, currentPage + 1);

            int half = MaxVisiblePages / 2;

            startPage = Math.Max(1, currentPage - half);
            endPage = startPage + MaxVisiblePages - 1;

            if(endPage > totalPages)
            {
                endPage = totalPages;
                startPage = Math.Max(1, endPage - MaxVisiblePages + 1);
            }

            showPrev = currentPage > 1;
            showNext = currentPage < totalPages;
        }
    }
}
