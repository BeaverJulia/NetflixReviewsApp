using System;
using System.Collections.Generic;
using System.Text;

namespace NetflixReviewsApp.core.Credentials
{
    public class OpenWrksCredentials
    {
        public string Url { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
