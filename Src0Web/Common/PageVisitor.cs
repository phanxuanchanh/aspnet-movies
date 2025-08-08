
using System.Threading;

namespace Web.Shared
{
    public static class PageVisitor
    {
        private static long _views = 0;

        public static long Views => Interlocked.Read(ref _views);

        public static void Add()
        {
            Interlocked.Increment(ref _views);
        }

        public static void Remove()
        {
            Interlocked.Decrement(ref _views);
        }
    }
}