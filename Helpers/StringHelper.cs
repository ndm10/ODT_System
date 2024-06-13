namespace ODT_System.Helpers
{
    public static class StringHelper
    {
        public static bool CompareIgnoreCase(string str1, string str2)
        {
            return str1.Contains(str2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
