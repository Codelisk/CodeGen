using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Services.Auth
{
    public interface ITokenProvider
    {
        string GetCurrentAccessToken();
        string GetCurrentRefreshToken();
        void UpdateCurrentToken(string newToken, string newRefreshToken);
    }
}
