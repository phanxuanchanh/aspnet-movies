using System;

namespace Data.Context
{
    public class DBContextPoolHandle : IDisposable
    {
        private readonly DBContextPool _contextPool;
        public DBContext Context { get; }


        public DBContextPoolHandle(DBContextPool contextPool)
        {
            _contextPool = contextPool;
            Context = _contextPool.Acquire();
        }

        public void Dispose()
        {
            _contextPool.Release(Context);
        }
    }
}