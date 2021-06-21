using System.ComponentModel.DataAnnotations;

namespace NetflixReviewsApp.core.Models
{
    public class ReviewInput
    {
        [Required]
        [Range(0, 5, ErrorMessage = "Please enter Number between 0-5")]
        public int Stars { get; set; }
        [Required] 
        [MaxLength(256)] 
        public string Description { get; set; }
        [Required] 
        public string ShowId { get; set; }
        [Required]
        public string Nickname { get; set; }
    }
}