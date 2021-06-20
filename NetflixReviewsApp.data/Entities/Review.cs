using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetflixReviewsApp.data.Entities
{
    public class ReviewEntity
    {
        [Key]
        public string Id { get; set; }
        public string ShowId { get; set; }
        public int Stars { get; set; }
        public string Description { get; set; }
      
    }
}
