using System;
using Microsoft.AspNetCore.WebUtilities;
using NetflixReviewsApp.core.Contracts;

namespace NetflixReviewsApp.core.Services
{
    public interface IUriService
    {
        public Uri GetShowsUri(PaginationQuery pagination);
    }
}