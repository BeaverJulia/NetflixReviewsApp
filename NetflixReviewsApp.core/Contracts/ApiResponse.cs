namespace NetflixReviewsApp.core.Contracts
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}