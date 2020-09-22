using Entities.DTO;
using GrönaFastigheter.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GrönaFastigheter.Pages
{
    public partial class Login
    {
        private UserForAuthenticationDto _userForAuthentication = new UserForAuthenticationDto();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await AuthenticationService.Login(_userForAuthentication);

            if (result.Status == 200)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Error = result.Message;
                ShowAuthError = true;

            }
        }
    }
}
