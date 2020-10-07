using Blazored.LocalStorage;
using Entities.DTO;
using GrönaFastigheter.AuthProviders;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrönaFastigheter.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        /// <summary>
        /// Automaticaly injects project instances of all parameters.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="authStateProvider"></param>
        /// <param name="localStorage"></param>
        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }
        /// <summary>
        /// Runs when user register and comfirm. Returns if the operations worked in the backend. User HAS to be informed if operation failed!
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            string content = userForRegistration.ToString();
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            Console.WriteLine(await bodyContent.ReadAsStringAsync());
            try
            {
                HttpResponseMessage httpResponse = await _client.PostAsync("/api/account/register", bodyContent);
                return httpResponse;
            }
            catch
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadGateway };
            }  
        }
        /// <summary>
        /// Sends password and username and get a jwt token in the result package.
        /// </summary>
        /// <param name="userForAuthentication"></param>
        /// <returns></returns>
        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {
            string content = userForAuthentication.ToString();
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage httpResponse = await _client.PostAsync("/token", bodyContent);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            AuthResponseDto result = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Console.WriteLine("Access granted is: " + httpResponse.IsSuccessStatusCode);
            result.Status =(int) httpResponse.StatusCode;
            if (!httpResponse.IsSuccessStatusCode)
            {
                result.Message = httpResponse.ReasonPhrase;
                return result;
            }

            await _localStorage.SetItemAsync("authToken", result.Access_Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.Username);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Access_Token);

            return result;
        }
        /// <summary>
        /// Removes user jwt token from cache and changes the authproviders status.
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
