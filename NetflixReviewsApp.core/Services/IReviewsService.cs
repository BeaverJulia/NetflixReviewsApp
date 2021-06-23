using System.Collections.Generic;
using System.Threading.Tasks;
using NetflixReviewsApp.core.Models;

namespace NetflixReviewsApp.core.Services
{
    public interface IReviewsService
    {
        Task<AddReviewResponse> AddReview(ReviewInput review);
        Task<List<ShowWithReview>> GetShowsWithReviews(PaginationFilter pagination);
    }
}