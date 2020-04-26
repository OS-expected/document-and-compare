namespace docAndCom.Helpers
{
    public static class ShortenInvokes
    {
        public static string GetResourceString(string key)
        {
            return ResourceLoader.Instance.GetString(key);
        }
    }
}
