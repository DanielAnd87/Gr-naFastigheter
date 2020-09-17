using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using GrönaFastigheter.AuthProviders;
using GrönaFastigheter.HttpRepository;

namespace GrönaFastigheter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped(sp =>
            {
                Uri uri = new Uri("https://mockapi-gronafastigheter.herokuapp.com");
                return new HttpClient { BaseAddress = uri };
            });
           

         //   builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IRealEstateHttpsRepository, RealEstateHttpsRepository>();
            await builder.Build().RunAsync();
        }
    }
}