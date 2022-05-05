using HotpServer.Exceptions;
using HotpServer.Models.Requests;
using HotpServer.Storage;
using HotpServer.Storage.Models;

namespace HotpServer.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        IDataLayer _dataLayer;
        public AuthenticationService(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public async Task<User> AuthenticateAsync(User authenticationRequestUser)
        {
            var user = await _dataLayer.GetUserByLoginAsync(authenticationRequestUser.Login);
            if (user != null)
            {
                if (user.Password.Equals(authenticationRequestUser.Password))
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<User> RegisterUserAsync(User registrationRequestUser)
        {
            if(await _dataLayer.GetUserByLoginAsync(registrationRequestUser.Login) != null)
            {
                throw new UserAlreadyExistsException();
            }
            await _dataLayer.AddOrUpdateUserAsync(registrationRequestUser);
            return await _dataLayer.GetUserByLoginAsync(registrationRequestUser.Login);
        }
    }
}
