using System.Collections.Generic;

namespace Common.Web
{
    public class PagedList<T>
    {
        public long PageNumber { get; set; }
        public long CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
