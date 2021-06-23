using System.Collections.Generic;
using System.Threading.Tasks;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Models.OpenWrksModels;
using RestSharp;

namespace NetflixReviewsApp.core.Services
{
    public interface IOpenWrksApiService
    {
        Task<IRestResponse> GetShow(string id);
        Task<IRestResponse> GetShows(PaginationFilter pagination);
    }
}