using Blazored.Modal;
using Blazored.SessionStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SurfsUpBlazor.Client.StateService;

namespace SurfsUpBlazor.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			builder.Services.AddHttpClient("SurfsUpBlazor.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddBlazoredSessionStorage();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SurfsUpBlazor.ServerAPI"));
			builder.Services.AddSingleton<StateContainerService>();

			builder.Services.AddApiAuthorization();
			builder.Services.AddBlazoredToast();
			builder.Services.AddBlazoredModal();
          
            WebAssemblyHost app = builder.Build();

            await app.RunAsync();
		}
	}
}