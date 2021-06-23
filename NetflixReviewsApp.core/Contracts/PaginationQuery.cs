namespace NetflixReviewsApp.core.Contracts
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = 1;
            Limit = 20;
        }

        public PaginationQuery(int pageNumber, int limit)
        {
            PageNumber = pageNumber;
            Limit = limit ;
        }
        public int PageNumber { get; set; }
        public int Limit { get; set; }
    }
}
