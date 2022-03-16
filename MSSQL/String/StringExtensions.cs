using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string SubStr(this string input, int start, int end)
        {
            return input.Substring(0, end - start);
        }

        public static string SubStr(this string input, char first, char last)
        {
            int start_idx = input.IndexOf(first);
            int last_idx = input.LastIndexOf(last);
            if (start_idx == -1 || last_idx == -1)
                throw new Exception("");
            return input.SubStr(start_idx, end: last_idx + 1);
        }

        public static string SubStr(this string input, string first, string last)
        {
            int start_idx = input.IndexOf(first);
            int last_idx = input.LastIndexOf(last);
            if (start_idx == -1 || last_idx == -1)
                throw new Exception("");
            return input.SubStr(start_idx, end: last_idx + last.Length);
        }

        public static bool IsUnicode(this string input)
        {
            return Encoding.ASCII.GetByteCount(input) != Encoding.UTF8.GetByteCount(input);
        }
    }
}
