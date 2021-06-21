using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetflixReviewsApp.core.Models
{
    public class ReviewOutput
    {
        public string Id { get; set; }
        public int Stars { get; set; }
        public string Description { get; set; }
        public string Nickname { get; set; }
    }
}

