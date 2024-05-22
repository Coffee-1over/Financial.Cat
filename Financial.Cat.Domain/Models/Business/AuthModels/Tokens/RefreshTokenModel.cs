namespace Financial.Cat.Domain.Models.Business.AuthModels.Tokens
{
    public class RefreshTokenModel
    {
        public string RefreshToken { get; set; }
        public TimeSpan RefreshTokenExpireInterval { get; set; }
    }
}
