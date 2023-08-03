namespace Foundation.Api.Models
{
    public class AuthResult
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public int SellerState { get; set; }
    }
}
