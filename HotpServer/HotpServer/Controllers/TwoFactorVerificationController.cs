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
    public class TwoFactorAuthenticationController : ControllerBase
    {
        ITwoFactorAuthService _twoFactorAuthService;

        public TwoFactorAuthenticationController(ITwoFactorAuthService twoFactorAuthService) : base()
        {
            _twoFactorAuthService = twoFactorAuthService ?? throw new ArgumentNullException(nameof(twoFactorAuthService));
        }

        [HttpGet("IsTwoFactorConfirmed/{login}")]
        public async Task<ActionResult<bool>> IsTwoFactorConfirmed([FromRoute] string login)
        {
            try
            {
                return Ok(await _twoFactorAuthService.IsTwoFactorConfirmed(login));
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

        [HttpGet("IsTwoFactorAuthEnabled/{login}")]
        public async Task<ActionResult<bool>> IsTwoFactorAuthEnabled([FromRoute] string login)
        {
            try
            {
                return Ok(await _twoFactorAuthService.IsTwoFactorAuthEnabled(login));
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

        [HttpPost("ConfirmTwoFactorAuth")]
        public async Task<ActionResult<bool>> ConfirmTwoFactorAuth([FromBody] TwoFactorConfirmationRequest confirmationRequest)
        {
            try
            {
                return Ok(await _twoFactorAuthService.ConfirmTwoFactorAuth(confirmationRequest.Login, confirmationRequest.Code));
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

        [HttpPost("ChangeTwoFactorStatus")]
        public async Task<ActionResult<UserResponce>> ChangeTwoFactorStatus([FromBody] ChangeTwoFactorStatusRequest changeStatusRequest)
        {
            try
            {
                User updatedUser = await _twoFactorAuthService.ChangeTwoFactorStatus(changeStatusRequest.Login,
                                                                                     changeStatusRequest.Password,
                                                                                     changeStatusRequest.IsEnabled,
                                                                                     changeStatusRequest.KeyBase64 ?? string.Empty);

                return Ok(MapperBuilder.CreateMapper<User, UserResponce>().Map<UserResponce>(updatedUser));
            }
            catch (NoSuchUserException ex)
            {
                ExceptionHandler.HandleException(ex);
                return NotFound();
            }
            catch(InvalidCredentialsException ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status401Unauthorized, ex);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
