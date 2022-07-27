namespace DisneyApi.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
