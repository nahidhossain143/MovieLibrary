using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovieLibrary.Helpers;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{
    public class HomeController : Controller
    {
        public static List<Movie> Movies = new List<Movie>
{
    new Movie
    {
        FileName = "1917.jpg",
        Title = "1917",
        Genre = "War, Drama",
        Year = "2019",
        Director = "Sam Mendes",
        Cast = "George MacKay, Dean-Charles Chapman, Mark Strong",
        Description = @"Two young British soldiers during WWI are given a mission to deliver a message that will stop a deadly attack.  
This visually stunning film is known for its one-shot cinematography technique, bringing intense real-time action."
    },
    new Movie
    {
        FileName = "interstellar.jpg",
        Title = "Interstellar",
        Genre = "Sci-Fi, Adventure",
        Year = "2014",
        Director = "Christopher Nolan",
        Cast = "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
        Description = @"In a dystopian future, Earth’s survival depends on a team of explorers traveling through a wormhole in search of a new home.  
Explores themes of time, love, and sacrifice with breathtaking visuals and complex scientific concepts."
    },
    new Movie
    {
        FileName = "superman.jpg",
        Title = "Superman",
        Genre = "Action, Sci-Fi",
        Year = "1978",
        Director = "Richard Donner",
        Cast = "Christopher Reeve, Margot Kidder, Gene Hackman",
        Description = @"The classic story of Clark Kent, an alien from Krypton raised on Earth, who uses his superpowers to protect humanity.  
A defining superhero film with iconic performances and memorable moments."
    },
    new Movie
    {
        FileName = "into the wild.jpg",
        Title = "Into the Wild",
        Genre = "Adventure, Biography",
        Year = "2007",
        Director = "Sean Penn",
        Cast = "Emile Hirsch, Vince Vaughn, Catherine Keener",
        Description = @"Based on the true story of Christopher McCandless, a young man who abandoned his possessions to live in the Alaskan wilderness.  
A profound exploration of freedom, nature, and self-discovery."
    },
    new Movie
    {
        FileName = "inception.jpg",
        Title = "Inception",
        Genre = "Sci-Fi, Action, Thriller",
        Year = "2010",
        Director = "Christopher Nolan",
        Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
        Description = @"A skilled thief who steals corporate secrets through dream-sharing technology is given a final chance to have his criminal history erased.  
A mind-bending thriller with layered dreams and stunning visuals."
    },
    new Movie
    {
        FileName = "dark knight.jpg",
        Title = "The Dark Knight",
        Genre = "Action, Crime, Drama",
        Year = "2008",
        Director = "Christopher Nolan",
        Cast = "Christian Bale, Heath Ledger, Aaron Eckhart",
        Description = @"Batman sets out to dismantle the remaining criminal organizations in Gotham.  
But he's soon faced with chaos unleashed by the Joker, a criminal mastermind with no rules."
    },
    new Movie
    {
        FileName = "parasite.jpg",
        Title = "Parasite",
        Genre = "Thriller, Drama",
        Year = "2019",
        Director = "Bong Joon-ho",
        Cast = "Song Kang-ho, Lee Sun-kyun, Cho Yeo-jeong",
        Description = @"A poor family schemes to become employed by a wealthy household by posing as unrelated, highly qualified individuals.  
A dark and thrilling satire on class disparity with unexpected twists."
    },
    new Movie
    {
        FileName = "joker.jpg",
        Title = "Joker",
        Genre = "Drama, Thriller",
        Year = "2019",
        Director = "Todd Phillips",
        Cast = "Joaquin Phoenix, Robert De Niro, Zazie Beetz",
        Description = @"A mentally troubled stand-up comedian descends into madness and crime in Gotham City.  
An origin story of the infamous villain that is both chilling and thought-provoking."
    }
};


        public ActionResult Index(string sortBy = "", string filter = "")
        {
            var movies = Movies;

            if (!string.IsNullOrEmpty(filter))
            {
                movies = movies.Where(m => m.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            switch (sortBy?.ToLower())
            {
                case "title":
                    movies = movies.OrderBy(m => m.Title).ToList();
                    break;
                case "rating":
                    movies = movies.OrderByDescending(m => m.AverageRating).ToList();
                    break;
            }

            var watchlist = Session.GetObject<List<string>>("Watchlist") ?? new List<string>();

            ViewBag.Watchlist = watchlist;
            ViewBag.SortBy = sortBy;
            ViewBag.Filter = filter;
            ViewBag.Message = TempData["Message"] as string;

            return View(movies);
        }

        public ActionResult Details(string title)
        {
            var movie = Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie == null)
                return HttpNotFound();

            var watchlist = Session.GetObject<List<string>>("Watchlist") ?? new List<string>();
            ViewBag.IsInWatchlist = watchlist.Contains(movie.Title);
            ViewBag.Message = TempData["Message"] as string;

            return View(movie);
        }

        [HttpPost]
        public ActionResult AddToWatchlist(string title)
        {
            var watchlist = Session.GetObject<List<string>>("Watchlist") ?? new List<string>();

            if (!watchlist.Contains(title))
            {
                watchlist.Add(title);
                Session.SetObject("Watchlist", watchlist);
                TempData["Message"] = $"\"{title}\" added to your Watchlist!";
            }
            else
            {
                TempData["Message"] = $"\"{title}\" is already in your Watchlist.";
            }

            return RedirectToAction("Details", new { title });
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

        [HttpPost]
        public ActionResult RateMovie(string title, int rating)
        {
            var movie = Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie == null)
                return new HttpStatusCodeResult(404, "Movie not found");

            if (rating < 1 || rating > 5)
                return new HttpStatusCodeResult(400, "Invalid rating");

            movie.Ratings.Add(rating);

            if (Request.IsAjaxRequest())
                return new HttpStatusCodeResult(200);

            return RedirectToAction("Details", new { title });
        }
    }
}
