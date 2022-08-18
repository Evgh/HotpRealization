using HotpServer.Contracts.Requests;
using HotpServer.Contracts.Responces;
using HotpServer.Exceptions;
using HotpServer.Services;
using HotpServer.Storage.Models;
using HotpServer.Utilities;
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
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult<UserResponce>> RegisterUser([FromBody] RegistrationRequest registrationRequest)
        {
            try
            {
                User requestUser = MapperBuilder.CreateMapper<RegistrationRequest, User>().Map<User>(registrationRequest);
                User resultUser = await _authenticationService.RegisterUserAsync(requestUser);

                if (resultUser != null)
                    return Ok(MapperBuilder.CreateMapper<User, UserResponce>().Map<UserResponce>(resultUser));

                else return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (UserAlreadyExistsException ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status409Conflict, ex);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("AuthenticateByPassword")]
        public async Task<ActionResult<UserResponce>> AuthenticateByPassword([FromBody] AuthenticationRequest authenticationRequest)
        {
            try
            {
                User requestUser = MapperBuilder.CreateMapper<AuthenticationRequest, User>().Map<User>(authenticationRequest);
                User resultUser = await _authenticationService.AuthenticateAsync(requestUser);

                if (resultUser != null)               
                    return Ok(MapperBuilder.CreateMapper<User, UserResponce>().Map<UserResponce>(resultUser));
                
                else return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch(InvalidCredentialsException ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (NoSuchUserException ex)
            {
                ExceptionHandler.HandleException(ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
