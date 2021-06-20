using System;
using System.Collections.Generic;
using System.Text;

namespace NetflixReviewsApp.core.Credentials
{
    public class AccessTokenResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
}
