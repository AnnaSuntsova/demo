using System;

namespace SiteDownloaderLibrary.Constraints
{
    
    public class SchemeConstraint : IConstraintRule
    {
        public bool IsValid(Uri uri)
        {
            return uri.Scheme.Equals("http") || uri.Scheme.Equals("https");
        }

        public bool IsValid(string url)
        {
            return url.StartsWith("http") | url.StartsWith("https") &&
                 CountSubstringInUrl(url, "http") == 1 |
                  CountSubstringInUrl(url, "https") == 1;
        }

        private static int CountSubstringInUrl(string inputString, string substring)
        {
            return (inputString.Length - inputString.Replace(substring, "").Length) / substring.Length;
        }
    }
}
