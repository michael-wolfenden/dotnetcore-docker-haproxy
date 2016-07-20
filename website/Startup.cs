using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Website
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var hostUri = Configuration.GetValue<Uri>("HOST_URI");

            var host = new WebHostBuilder()
                .UseUrls(hostUri.ToString())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConfigurationRoot>(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
    public class HomeController : Controller
    {
        private readonly string _apiUri;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfigurationRoot configuration, ILogger<HomeController> logger)
        {
            _logger = logger;

            var apiBaseUri = configuration.GetValue<Uri>("API_BASE_URI");
            _apiUri = new Uri(apiBaseUri, "api/machine").ToString();
        }

        public async Task<string> Index()
        {
            try
            {
                var machineName = await new HttpClient().GetStringAsync(_apiUri);
                return $"Reveived a response from {machineName}";
            }
            catch (Exception ex)
            {
                var message = $"/GET {_apiUri} failed with message:\n{ex.Message.ToString()}";
                _logger.LogError(0, ex, message);

                return message;
            }
        }
    }
}
