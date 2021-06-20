namespace NetflixReviewsApp.core.OpenWrksApiSettings
{
    internal static class ApiRoutes
    {
        public const string Root = "https://netflix-app.openwrks.com/";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public const string Shows = Base + "/Shows";
    }
}