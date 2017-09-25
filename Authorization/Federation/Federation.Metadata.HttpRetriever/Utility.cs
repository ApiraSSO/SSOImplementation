using System;

namespace Federation.Metadata.HttpRetriever
{
    internal static class Utility
    {
        public static bool IsHttps(string address)
        {
            if (string.IsNullOrEmpty(address))
                return false;
            try
            {
                Uri uri = new Uri(address);
                return Utility.IsHttps(new Uri(address));
            }
            catch (UriFormatException)
            {
                return false;
            }
        }

        public static bool IsHttps(Uri uri)
        {
            if (uri == (Uri)null)
                return false;
            return uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
        }
    }
}