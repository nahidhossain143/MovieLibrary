using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Models
{
    public class Movie
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Year { get; set; }
        public string Cast { get; set; }
        public List<int> Ratings { get; set; } = new List<int>();
        public double AverageRating => Ratings.Count == 0 ? 0 : Ratings.Average();
    }
}
