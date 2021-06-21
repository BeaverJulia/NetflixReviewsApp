using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Models.OpenWrksModels;
using NetflixReviewsApp.data;
using NetflixReviewsApp.data.Entities;
using Newtonsoft.Json;

namespace NetflixReviewsApp.core.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IOpenWorksApiService _openWorksApiService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewsService(IOpenWorksApiService openWorksApiService, DataContext context, IMapper mapper)
        {
            _openWorksApiService = openWorksApiService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AddReviewResponse> AddReview(ReviewInput review)
        {
            var response = await _openWorksApiService.GetShow(review.ShowId);
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new AddReviewResponse(false, new List<ValidationResult>(),
                    new List<string> {response.ErrorMessage});

            var entity = _mapper.Map<ReviewEntity>(review);
            entity.Id = Guid.NewGuid().ToString();
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new AddReviewResponse(true, new List<ValidationResult>(),
                new List<string>());
        }

        public async Task<List<ShowWithReview>> GetShowsWithReviews()
        {
            var response = await _openWorksApiService.GetShows();
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            var shows = JsonConvert.DeserializeObject<Shows>(response.Content.ToString());
            var showsWithReviews = _mapper.Map<List<ShowWithReview>>(shows.Data);

            var reviewEntities = _context.Reviews.ToList();
            foreach (var show in showsWithReviews)
                if (reviewEntities.Any(x => x.ShowId == show.Id))
                {
                    var reviews = reviewEntities.FindAll(x => x.ShowId == show.Id);
                    show.Reviews = _mapper.Map<List<ReviewOutput>>(reviews);
                    var stars = new List<int>();
                    reviews.ForEach(x => stars.Add(x.Stars));
                    show.AverageStars = stars.Sum() / stars.Count;
                }

            return showsWithReviews;
        }
    }
}