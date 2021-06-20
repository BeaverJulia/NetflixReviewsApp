using System;

namespace NetflixReviewsApp.core.Models.OpenWrksModels
{
    public class Show
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public int ReleaseYear { get; set; }
        public string Rating { get; set; }
        public string Duration { get; set; }
        public string ListedIn { get; set; }
        public string Description { get; set; }
    }
}