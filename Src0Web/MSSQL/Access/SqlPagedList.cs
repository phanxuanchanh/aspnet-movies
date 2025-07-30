using System.Collections.Generic;

namespace MSSQL.Access
{
    public class SqlPagedList<T>
    {
        private long skip;
        private long take;
        private long totalRecord;
        private List<T> items;
        private long pageNumber;
        private long currentPage;

        public long Take { get { return take; } }
        public long Skip { get { return skip; } }
        public long TotalRecord { get { return totalRecord; } }
        public long PageNumber { get { return pageNumber; } }
        public long CurrentPage { get { return currentPage; } }
        public List<T> Items { get { return items; } set { items = value; } }

        public SqlPagedList()
        {
            skip = 0;
            take = 0;
            totalRecord = 0;
            pageNumber = 0;
            currentPage = 0;
            items = null;
        }

        public void Solve(long totalRecord, long pageIndex, long pageSize)
        {
            this.totalRecord = totalRecord;
            take = pageSize;
            int index = 0;
            skip = 0;
            if(pageSize >= totalRecord)
            {
                currentPage = 0;
                pageNumber = 1;

            }
            else
            {
                for (long i = 0; i < totalRecord; i = i + pageSize)
                {
                    if (pageIndex == index)
                    {
                        skip = i;
                        currentPage = index;
                    }
                    index++;
                }
                pageNumber = index;
            }
            
        }
    }
}
