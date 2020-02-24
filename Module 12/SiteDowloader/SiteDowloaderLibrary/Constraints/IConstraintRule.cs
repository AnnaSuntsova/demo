using System;

namespace SiteDownloaderLibrary.Constraints
{
    public interface IConstraintRule
    {
        bool IsValid(Uri uri);
        bool IsValid(string url);
    }
}
