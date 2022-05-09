using Authorizer.Models;
using Authorizer.Models.Requests;
using Authorizer.Models.Responces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Authorizer.Services.Implementations
{
    public class ServiceClient : IServiceClient
    {
        private const string BASE_ADDRESS = "https://42b7-37-214-57-105.ngrok.io/api/";

        private readonly HttpClient _httpClient;

        public ServiceClient()
        {
            _httpClient = new HttpClient();
        }

        #region Public Methods

        public async Task<BaseResponce<User>> PostRegisterUser(string login, string password)
        {
            try
            {
                var registrationRequest = new RegistrationRequest()
                {
                    Login = login,
                    Password = password
                };

                var registeredUserResponce = await _httpClient.PostAsync(BASE_ADDRESS + "Authentication/RegisterUser", FormPostContentAndSetHeaders(registrationRequest));
                return await FormUserResponce(registeredUserResponce);
            }
            catch (Exception ex)
            {
                return CreateErrorResponce();
            }
        }

        public async Task<BaseResponce<User>> PostAuthenticateUserByPassword(string login, string password)
        {
            try
            {
                var authenticationRequest = new AuthenticationRequest()
                {
                    Login = login,
                    Password = password
                };

                var authResponce = await _httpClient.PostAsync(BASE_ADDRESS + "Authentication/AuthenticateByPassword", FormPostContentAndSetHeaders(authenticationRequest));
                return await FormUserResponce(authResponce);
            }
            catch(Exception ex)
            {
                return CreateErrorResponce();
            }

        }

        public async Task<BaseResponce<User>> PostChangeTwoFactorStatus(string login, string keyBase64, bool isEnabled)
        {
            try
            {
                var changeStatusRequest = new ChangeTwoFactorStatusRequest()
                {
                    Login = login,
                    KeyBase64 = keyBase64,
                    IsEnabled = isEnabled
                };

                var changeResponce = await _httpClient.PostAsync(BASE_ADDRESS + "TwoFactorAuthentication/ChangeTwoFactorStatus", FormPostContentAndSetHeaders(changeStatusRequest));
                return await FormUserResponce(changeResponce);
            }
            catch (Exception e)
            {
                return CreateErrorResponce();
            }
        }

        public async Task<bool> PostConfirmTwoFactorAuth(string login, string code)
        {
            try
            {
                var confirmationRequest = new TwoFactorConfirmationRequest()
                {
                    Login = login,
                    Code = code
                };

                var confirmationResponce = await _httpClient.PostAsync(BASE_ADDRESS + "TwoFactorAuthentication/ConfirmTwoFactorAuth", FormPostContentAndSetHeaders(confirmationRequest));

                if (confirmationResponce.IsSuccessStatusCode && !confirmationResponce.StatusCode.Equals(HttpStatusCode.NoContent))
                    return true;
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        #endregion


        #region Private methods

        private StringContent FormPostContentAndSetHeaders(object request)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpContent;
        }

        private async Task<BaseResponce<User>> FormUserResponce(HttpResponseMessage responceMessage)
        {
            var userResponce = new BaseResponce<User>()
            {
                IsSuccess = responceMessage.IsSuccessStatusCode,
                StatusCode = responceMessage.StatusCode,
                ErrorMessage = responceMessage.ReasonPhrase
            };

            if (responceMessage.IsSuccessStatusCode)
            {
                var registeredUserSerialized = await responceMessage.Content.ReadAsStringAsync();
                userResponce.Content = JsonConvert.DeserializeObject<User>(registeredUserSerialized);
            }

            return userResponce;
        }

        private BaseResponce<User> CreateErrorResponce()
        {
            return new BaseResponce<User>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = "Something went wrong",
                Content = null
            };
        }

        #endregion
    }
}
