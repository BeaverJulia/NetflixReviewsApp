using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Services;

namespace NetflixReviewsApp.api.Controllers
{
    [ApiController]
    public class ShowsController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ShowsController(IReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }

        [HttpPost("/post")]
        public async Task<ActionResult> AddReview([FromBody] ReviewInput input)
        {
            var output = await _reviewsService.AddReview(input);
            if (!output.Success)
                return BadRequest(output);
            if (!output.Success && output.Errors.Any())
                return NotFound(output);
            return new ObjectResult(output);
        }

        [HttpPost("/get")]
        public async Task<ActionResult> GetShows()
        {
            var output = await _reviewsService.GetShowsWithReviews();
            if (output==null)
                return NotFound();
            return new ObjectResult(output);
        }
    }
}