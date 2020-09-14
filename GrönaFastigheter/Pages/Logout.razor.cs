using GrönaFastigheter.HttpRepository;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GrönaFastigheter.Pages
{
    public partial class Logout
    {

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavManager.NavigateTo("/");
            
        }
    }
}
