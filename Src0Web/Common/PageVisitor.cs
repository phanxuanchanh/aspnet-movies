
namespace Web.Shared
{
    public static class PageVisitor
    {
        public static long Views;

        public static void Add()
        {
            Views ++;
        }

        public static void Remove()
        {
            Views --;
        }
    }
}