using Foundation.Api.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Services.Auth
{
    public class TokenProvider : ITokenProvider
    {

        private string AccessToken;
        private string RefreshToken;
        public string GetCurrentAccessToken()
        {
            return AccessToken;
        }
        public string GetCurrentRefreshToken()
        {
            return RefreshToken;
        }

        public void UpdateCurrentToken(string newToken, string newRefreshToken)
        {
            this.AccessToken = newToken;
            this.RefreshToken = newRefreshToken;
        }
    }
}
