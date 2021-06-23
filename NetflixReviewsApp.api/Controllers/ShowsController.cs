using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetflixReviewsApp.core.Contracts;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Services;

namespace NetflixReviewsApp.api.Controllers
{
    //TODO Add pagination
    [ApiController]
    public class ShowsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ShowsController(IReviewsService reviewsService, IMapper mapper, IUriService uriService)
        {
            _mapper = mapper;
            _reviewsService = reviewsService;
            _uriService = uriService;
        }

        [HttpPost("v1/review")]
        public async Task<ActionResult> AddReview([FromBody] ReviewInput input)
        {
            var output = await _reviewsService.AddReview(input);
            if (!output.Success)
                return BadRequest(output);
            if (!output.Success && output.Errors.Any())
                return NotFound(output);
            return new OkObjectResult(new ApiResponse<AddReviewResponse>(output));
        }

        [HttpGet("v1/shows")]
        public async Task<ActionResult> GetShows([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var output = await _reviewsService.GetShowsWithReviews(pagination);
            if (output == null)
                return NotFound();
            if (pagination == null || pagination.Limit < 1 || pagination.PageNumber < 1)
                return Ok(new PagedResponse<ShowWithReview>(output));
            var nextPage = pagination.PageNumber >= 1
                ? _uriService.GetShowsUri(new PaginationQuery(pagination.PageNumber+1, pagination.Limit)).ToString()
                : null;
            var previousPage = pagination.PageNumber-1 >= 1
                ? _uriService.GetShowsUri(new PaginationQuery(pagination.PageNumber-1, pagination.Limit)).ToString()
                : null;
            var response = new PagedResponse<ShowWithReview>
            {
                Data = output,
                PageNumber = pagination.PageNumber >=1 ? pagination.PageNumber : (int?)null,
                Limit = pagination.Limit >= 1 ? pagination.Limit : (int?)null,
                NextPage = output.Any() ? nextPage:null,
                PreviousPage = previousPage

            };

            return new OkObjectResult(response);
        }
    }
}