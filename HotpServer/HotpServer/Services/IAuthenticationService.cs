using HotpServer.Storage.Models;

namespace HotpServer.Services
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(User authenticationRequest);
        Task<User> RegisterUserAsync(User registrationRequest);
    }
}
