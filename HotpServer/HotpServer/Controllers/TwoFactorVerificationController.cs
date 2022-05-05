using HotpServer.Models.Dto;
using HotpServer.Models.Requests;
using HotpServer.Services;
using HotpServer.Storage.Models;
using HotpServer.Utilities;
using Microsoft.AspNetCore.Http;
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
            _twoFactorAuthService = twoFactorAuthService;
        }

        [HttpGet("IsTwoFactorConfirmed")]
        public async Task<bool> IsTwoFactorConfirmed([FromQuery] string login)
        {
            try
            {
                return await _twoFactorAuthService.IsTwoFactorConfirmed(login);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return false;
        }

        [HttpGet("IsTwoFactorAuthEnabled")]
        public async Task<bool> IsTwoFactorAuthEnabled([FromQuery] string login)
        {
            try
            {
                return await _twoFactorAuthService.IsTwoFactorAuthEnabled(login);
            }
            catch(Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return false;
        }

        [HttpPost("ConfirmTwoFactorAuth")]
        public async Task<bool> ConfirmTwoFactorAuth([FromBody] TwoFactorConfirmationRequest confirmationRequest)
        {
            try
            {
                return await _twoFactorAuthService.ConfirmTwoFactorAuth(confirmationRequest.Login, confirmationRequest.Code);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return false;
        }

        [HttpPost("ChangeTwoFactorStatus")]
        public async Task<UserDto> ChangeTwoFactorStatus([FromBody] ChangeTwoFactorStatusRequest changeStatusRequest)
        {
            try
            {
                User updatedUser = await _twoFactorAuthService.ChangeTwoFactorStatus(changeStatusRequest.Login,
                                                                         changeStatusRequest.IsEnabled,
                                                                         changeStatusRequest.KeyBase64 ?? string.Empty);

                return MapperBuilder.CreateMapper<User, UserDto>().Map<UserDto>(updatedUser);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return null;
        }
    }
}
