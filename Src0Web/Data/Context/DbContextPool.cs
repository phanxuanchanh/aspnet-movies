using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Data.Context
{
    public class DBContextPool
    {
        private readonly BlockingCollection<DBContext> _pool;
        private readonly int _maxPool;
        private int _currentCount = 0;
        private readonly SemaphoreSlim _semaphore;

        public DBContextPool(int maxPool = 128)
        {
            _pool = new BlockingCollection<DBContext>(new ConcurrentStack<DBContext>(), maxPool);
            _maxPool = maxPool;
            _semaphore = new SemaphoreSlim(maxPool, maxPool);
        }

        public DBContext Acquire()
        {
            _semaphore.Wait();

            while (true)
            {
                if (_pool.TryTake(out DBContext context))
                {
                    SqlConnection connection = context.GetConnection();

                    if (connection.State == ConnectionState.Open)
                        return context;

                    context.Dispose();
                    Interlocked.Decrement(ref _currentCount);
                    continue;
                }

                if (_currentCount < _maxPool)
                {
                    Interlocked.Increment(ref _currentCount);
                    DBContext newContext = new DBContext();
                    return newContext;
                }

                DBContext blockedContext = _pool.Take(); // blocking
                SqlConnection blockedConn = blockedContext.GetConnection();
                if (blockedConn.State == ConnectionState.Open)
                    return blockedContext;

                blockedContext.Dispose();
                Interlocked.Decrement(ref _currentCount);
            }
        }

        public void Release(DBContext context)
        {
            if (context.GetConnection().State == ConnectionState.Open)
                _pool.Add(context);
            else
            {
                context.Dispose();
                Interlocked.Decrement(ref _currentCount);
            }

            _semaphore.Release();
        }
    }
}
