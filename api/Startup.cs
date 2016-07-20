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

namespace Api
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

            app.UseMvc();
        }
    }

    [Route("api/[controller]")]
    public class MachineController : Controller
    {
        private readonly string _machineName;

        public MachineController(IConfigurationRoot configuration)
        {
            _machineName = configuration.GetValue<string>("HOSTNAME") 
                           ?? configuration.GetValue<string>("COMPUTERNAME");
        }

        public string Get()
        {
            return _machineName;
        }
    }
}
