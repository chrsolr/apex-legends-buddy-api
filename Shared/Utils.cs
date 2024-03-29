public static class Utils
{
    public static string CleanRevisionImageUrl(string url)
    {
        return !string.IsNullOrEmpty(url)
            ? url[..url.IndexOf("/revision", StringComparison.Ordinal)]
            : url;
    }
}
