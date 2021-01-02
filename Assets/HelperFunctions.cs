namespace Assets
{
    public static class HelperFunctions
    {
        public static bool IsNullOrWhiteSpace(string s)
        {
            if (s == null)
                return true;

            foreach (char c in s)
            {
                if (c != ' ') return false;
            }
            return true;

        }
    }
}
