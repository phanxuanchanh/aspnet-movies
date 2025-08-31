
namespace Data.Context
{
    public class DBContextPoolHandle
    {
        private readonly DBContextPool _contextPool;
        private DBContext _context;

        public DBContextPoolHandle(DBContextPool contextPool)
        {
            _contextPool = contextPool;
            _context = _contextPool.Acquire();
        }

        public DBContext Get() => _context;
        public void Return() => _contextPool.Release(_context);
    }
}