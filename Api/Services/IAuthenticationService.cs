using System.Threading.Tasks;
using Api.Config;

namespace Api.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResult> Authenticate(string username, string password);
    }
}