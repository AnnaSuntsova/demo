using SiteDownloaderLibrary;
using SiteDownloaderLibrary.Constraints;
using System;
using System.Collections.Generic;
using System.IO;

namespace SiteDowloaderApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Введите глубину анализа ссылок:");
            int deepLevel = int.Parse(Console.ReadLine());

            Console.WriteLine(@"Ограничения на переходы в другие домены (1 - без ограничений, 2 - внутри текущего домена, 3 - не выше пути в исходном URL):");
            var constr = Console.ReadLine();
            DomainLimitations domainLimit = (DomainLimitations)Enum.Parse(typeof(DomainLimitations), constr);

            Console.WriteLine(@"Ограничения на расширения скачиваемых ресурсов (например: gif,jpeg,jpg,pdf):");
            var extensions = Console.ReadLine();
            var extList = GetConstraints(extensions);

            Console.WriteLine("Стартовый URL:");
            var rootUrl = Console.ReadLine();

            Console.WriteLine("Корневой файловый каталог:");
            var rootDirectory = Console.ReadLine();
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            bool tracingMode;
            Console.WriteLine("Отображать трассировку? (0 - нет, 1 -да)");
            if (Console.ReadLine() == "0")
            {
                tracingMode = false;
            }
            else
            {
                tracingMode = true;
            }
            var logger = new Logger();
            var contentValidator = new Validator(new List<IConstraintRule>
            {
                new ExtensionConstraint(extList)
            });

            var siteSaver = new SiteSaver(rootDirectory, logger, tracingMode);
            var siteDownloader = new SiteDownloader(logger, siteSaver, contentValidator, tracingMode);
            var urlValidator = new Validator(new List<IConstraintRule>
            {
                new SchemeConstraint(),
                new DomainConstraint(new Uri(rootUrl), domainLimit)
            });
            var siteManager = new SiteManager(siteDownloader, logger, urlValidator, tracingMode);
            var listUrl = new List<Uri> { new Uri(rootUrl) };

            var countLevel = 0;

            try
            {
                siteManager.Start(listUrl, deepLevel, countLevel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Finish!");
            Console.ReadKey();
        }

        private static List<string> GetConstraints(string constr)
        {
            var result = new List<string>();
            char[] separators = new char[] { ' ', '.', ',', ';', ':' };
            var constraints = constr.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var extens in constraints)
            {
                result.Add(extens);
            }
            return result;
        }
    }
}
