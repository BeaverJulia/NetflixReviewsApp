using System;
using Microsoft.EntityFrameworkCore;
using NetflixReviewsApp.data.Entities;

namespace NetflixReviewsApp.data
{
    public class DataContext: DbContext
    {
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<ReviewEntity> Reviews { get; set; }
      
    }
}
