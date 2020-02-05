using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using MvcMusicStore.Infrastructure;
using MvcMusicStore.Logging;
using MvcMusicStore.Models;
using PerformanceCounterHelper;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        private readonly IUserLogger logger;

        public HomeController()
        {
            if (ConfigurationManager.AppSettings["Logging"].AsBool())
                logger = new Logger();
        }

        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            logger?.Debug("Enter to Home page");

            var counterHelper = PerformanceHelper.CreateCounterHelper<PerfomanceCounters>("Mvc project");
            counterHelper.Increment(PerfomanceCounters.VisitHomePage);
            

            return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}