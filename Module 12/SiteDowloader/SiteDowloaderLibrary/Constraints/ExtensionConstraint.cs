using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteDownloaderLibrary.Constraints
{

    public class ExtensionConstraint: IConstraintRule
    {
        private readonly List<string> _extensions;

        public ExtensionConstraint(List<string> extensions)
        {
            _extensions = extensions;
        }

        public bool IsValid(Uri uri)
        {
            var currentExtension = GetCurrentExtension(uri);
            return currentExtension != null && _extensions.Any(ext => ext.Equals(currentExtension));
        }

        public bool IsValid(string url)
        {
            return IsValid(new Uri(url));
        }        

        private string GetCurrentExtension(Uri uri)
        {
            var lastSegment = uri.Segments.Last();
            var index = lastSegment.LastIndexOf('.');
            string extension;
            if ((index != -1) && (lastSegment.Length != index + 1))
            {
                extension = lastSegment.Substring(index + 1);
            }
            else
            {
                extension = null;
            }

            return extension;
        }
    }
}
