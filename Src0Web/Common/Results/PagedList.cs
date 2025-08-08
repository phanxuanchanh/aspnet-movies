using System;
using System.Collections.Generic;

namespace Web.Shared.Result
{
    public class PagedList<T>
    {
        public long PageSize { get; set; }
        public long CurrentPage { get; set; }
        public long TotalItems { get; set; }
        public List<T> Items { get; set; }

        public long TotalPages { get { return (int)Math.Ceiling((double)TotalItems / PageSize); } }
    }
}
