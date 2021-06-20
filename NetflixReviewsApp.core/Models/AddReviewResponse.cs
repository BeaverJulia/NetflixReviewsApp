using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetflixReviewsApp.core.Models
{
    public class AddReviewResponse 
    {
        public bool Success { get; set; }
        public IEnumerable<ValidationResult> ValidationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }
        

        public AddReviewResponse(bool success, IEnumerable<ValidationResult> validationErrors, IEnumerable<string> errors)
        {
            Success = success;
            ValidationErrors = validationErrors;
            Errors = errors;
           
        }
    }
}
