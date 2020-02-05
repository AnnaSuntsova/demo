using SiteDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using SiteDownloader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDownloaderApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Введите глубину анализа ссылок:");
            int deepLevel = int.Parse(Console.ReadLine());

            //Console.WriteLine(@"Ограничения на переходы в другие домены (n - без ограничений, h - внутри текущего домена, e - не выше пути в исходном URL):");
            //var constr = Console.ReadLine();

            //Console.WriteLine(@"Ограничения на расширения скачиваемых ресурсов (например: gif,jpeg,jpg,pdf):");
            //var extensions = Console.ReadLine();

            Console.WriteLine("Стартовая точка:");
            var path = Console.ReadLine();

            //Console.WriteLine("Отображать обрабатываемую страницу? (Y/N)");
            //var log = Console.ReadLine();

            //DirectoryInfo rootDirectory = new DirectoryInfo(path);
            //IContentSaver contentSaver = new ContentSaver(rootDirectory);
            //ILogger logger = new SimpleLogger(log);
            //List<string> extens = GetConstraints(extensions);
            var downloader = new SiteDownloaderLibrary(deepLevel);

            try
            {
                downloader.LoadFromUrl(path);
            }
            catch (Exception ex)
            {
                //logger.Log($"Error during site downloading: {ex.Message}");
            }
        }

        private static List<string> GetConstraints(string constr)
        {
            var result = new List<string>();
            char[] separators = new char[] { ' ', '.', ',', ';', ':'};
            var constraints = constr.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var extens in constraints)
            {
                result.Add(extens);
            }
            return result;
        }
    }
}
