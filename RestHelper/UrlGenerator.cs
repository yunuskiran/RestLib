using System;

namespace RestHelper
{
    public static class UrlGenerator
    {
        public static string CreateUrl(string url)
        {
            if (!IsValidUrl(url)) throw new UriFormatException("Invalid Uri");
            return url;
        }

        public static string CreateUrl<TRequest>(string url, TRequest request, bool ignoreNullProperties = false)
        {
            if (!IsValidUrl(url)) throw new UriFormatException("Invalid Uri");

            var queryString = string.Empty;
            if (request != null) queryString = QueryStringGenerator.Generate(request, ignoreNullProperties);

            return !string.IsNullOrWhiteSpace(queryString) ? $"{url}?{queryString}" : url;
        }

        public static bool IsValidUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute);
        }
    }
}