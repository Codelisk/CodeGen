namespace Foundation.Api.Services.Auth
{
    public interface ITokenProvider
    {
        string GetCurrentAccessToken();
        string GetCurrentRefreshToken();
        void UpdateCurrentToken(string newToken, string newRefreshToken);
    }
}
