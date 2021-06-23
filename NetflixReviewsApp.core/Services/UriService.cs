using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using NetflixReviewsApp.core.Contracts;

namespace NetflixReviewsApp.core.Services
{
    public class UriService : IUriService
    {
        public Uri GetShowsUri(PaginationQuery pagination)
        {
            string baseurl = "https://localhost:5001/v1/shows";
            var uri = new Uri("https://localhost:5001/v1/shows");
            var modifed = QueryHelpers.AddQueryString(baseurl, "PageNumber", pagination.PageNumber.ToString());
            modifed = QueryHelpers.AddQueryString(modifed, "Limit", pagination.Limit.ToString());
            return new Uri(modifed);
        }
    }
}
