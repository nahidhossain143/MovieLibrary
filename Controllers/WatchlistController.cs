using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovieLibrary.Helpers;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{
    public class WatchlistController : Controller
    {
        public ActionResult Index()
        {
            var watchlistTitles = Session.GetObject<List<string>>("Watchlist") ?? new List<string>();
            var movies = HomeController.Movies.Where(m => watchlistTitles.Contains(m.Title)).ToList();

            ViewBag.Message = TempData["Message"] as string;

            return View(movies);
        }

        [HttpPost]
        public ActionResult RemoveFromWatchlist(string title)
        {
            var watchlist = Session.GetObject<List<string>>("Watchlist") ?? new List<string>();

            if (watchlist.Contains(title))
            {
                watchlist.Remove(title);
                Session.SetObject("Watchlist", watchlist);
                TempData["Message"] = $"\"{title}\" removed from your Watchlist.";
            }
            else
            {
                TempData["Message"] = $"\"{title}\" was not found in your Watchlist.";
            }

            return RedirectToAction("Index");
        }
    }
}
