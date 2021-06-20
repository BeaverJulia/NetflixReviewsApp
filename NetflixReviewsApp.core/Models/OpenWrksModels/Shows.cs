using System.Collections.Generic;

namespace NetflixReviewsApp.core.Models.OpenWrksModels
{
    public class Shows
    {
        public List<Show> Data { get; set; }
        public Links Links { get; set; }
        public Pagination Pagination { get; set; }
    }
}