using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloader.Interfaces
{
    public interface IContentSaver
    {
        void SaveFile(Uri uri, Stream fileStream);
        void SaveHtmlDocument(Uri uri, string name, Stream documentStream);
    }
}
