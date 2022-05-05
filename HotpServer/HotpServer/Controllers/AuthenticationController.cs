using HotpServer.Models.Dto;
using HotpServer.Models.Requests;
using HotpServer.Services;
using HotpServer.Storage;
using HotpServer.Storage.Models;
using HotpServer.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("RegisterUser")]
        public async Task<UserDto> RegisterUser([FromBody] RegistrationRequest registrationRequest)
        {
            try
            {
                User requestUser = MapperBuilder.CreateMapper<RegistrationRequest, User>().Map<User>(registrationRequest);
                User resultUser = await _authenticationService.RegisterUserAsync(requestUser);

                if (resultUser != null)
                {
                    return MapperBuilder.CreateMapper<User, UserDto>().Map<UserDto>(resultUser);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return null;
        }

        [HttpPost("AuthenticateByPassword")]
        public async Task<UserDto> AuthenticateByPassword([FromBody] AuthenticationRequest authenticationRequest)
        {
            try
            {
                User requestUser = MapperBuilder.CreateMapper<AuthenticationRequest, User>().Map<User>(authenticationRequest);
                User resultUser = await _authenticationService.AuthenticateAsync(requestUser);

                if (resultUser != null)
                {
                    return MapperBuilder.CreateMapper<User, UserDto>().Map<UserDto>(resultUser);
                }
            }
            catch(Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }

            return null;
        }
    }
}
