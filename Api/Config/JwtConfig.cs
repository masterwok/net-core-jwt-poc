using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api.Config
{
    public sealed class JwtConfig
    {
        internal SecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        public string Secret { get; set; }
        
        public string Issuer { get; set; }
        
        public string Audience { get; set; }
    }
}