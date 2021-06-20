using System;
using System.Collections.Generic;
using System.Text;
using NetflixReviewsApp.core.Models.OpenWrksModels;

namespace NetflixReviewsApp.core.Models
{
    public class ShowWithReview : Show
    {
        public float AverageStars { get; set; }
        public List<ReviewOutput> Reviews { get; set; }
    }
}
