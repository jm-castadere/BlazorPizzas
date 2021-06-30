using BlazorPizzas.Data;
using BlazorPizzas.Opts;
using BlazorPizzas.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace BlazorPizzas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            //config service to appsetting value load
            // "Api": {"Url": "http://localhost:6000"
            services.AddOptions();
            services.Configure<ApiOptions>(Configuration.GetSection("Api"));

            //services.AddSingleton<IPizzaManager, InMemoryPizzaManager>();
            //Add API HtppPizza ->To interface
            services.AddHttpClient<IPizzaManager, HttpPizzaManager>((sp, client) =>
            {
                //Get service options
                var options = sp.GetRequiredService<IOptions<ApiOptions>>();
                //Get url of appsettings
                client.BaseAddress = new Uri(options.Value.Url);

            })
                //Add retry
                .AddPolicyHandler(GetPolicy());
        }

        /// <summary>
        /// Retry fonction of error
        /// </summary>
        /// <returns></returns>
        private IAsyncPolicy<HttpResponseMessage> GetPolicy() => HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(300 + (i * 100)));


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
