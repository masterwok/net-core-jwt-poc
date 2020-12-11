using Api.Constants;

namespace Api.Config
{
    public class AuthResult
    {
        public AuthStatus AuthStatus { get; set; }
        public string? Token { get; set; }
    }
}