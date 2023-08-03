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
            AccessToken = newToken;
            RefreshToken = newRefreshToken;
        }
    }
}
