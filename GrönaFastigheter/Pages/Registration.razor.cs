using Entities.DTO;
using GrönaFastigheter.HttpRepository;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrönaFastigheter.Pages
{
    public partial class Registration
    {
        private UserForRegistrationDto _userForRegistration = new UserForRegistrationDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowRegistrationErros { get; set; }
        public string ErrorMessage { get; set; }

        public async Task Register()
        {
            ShowRegistrationErros = false;

            var result = await AuthenticationService.RegisterUser(_userForRegistration);
            Console.WriteLine(result.IsSuccessStatusCode);
            if(!result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                RegistrationResponseDto registrationResponseDto = JsonSerializer.Deserialize<RegistrationResponseDto>(response, new JsonSerializerOptions {PropertyNameCaseInsensitive = true });
                ErrorMessage = registrationResponseDto.Message;

                ShowRegistrationErros = true;
            }
            else
            {
                Console.WriteLine("De som gör de");
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
