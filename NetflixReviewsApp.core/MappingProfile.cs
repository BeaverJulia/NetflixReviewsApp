using System;
using AutoMapper;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Models.OpenWrksModels;
using NetflixReviewsApp.data.Entities;

namespace NetflixReviewsApp.core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReviewInput, ReviewEntity>();
            CreateMap<ReviewEntity, ReviewOutput>();
            CreateMap<Show, ShowWithReview>();

        }
    }
}
