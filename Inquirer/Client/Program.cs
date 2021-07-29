using Inquirer.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inquirer.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("Inquirer.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Inquirer.ServerAPI"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISurveyService, SurveyService>();

            await builder.Build().RunAsync();
        }
    }
}
