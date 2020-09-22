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

        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            string content = userForRegistration.ToString();
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage httpResponse = await _client.PostAsync("/api/account/register", bodyContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {
            string content = userForAuthentication.ToString();
            StringContent bodyContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage httpResponse = await _client.PostAsync("/token", bodyContent);
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            AuthResponseDto result = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
