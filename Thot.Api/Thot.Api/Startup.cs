using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Thot.Api.Controllers;
using Thot.Api.Core.DTOs;
using Thot.Api.Core.Interfaces;
using Thot.Api.Core.Services;
using Thot.Api.Infrastructure.Repositories;

namespace Thot.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.Configure<WordDatabaseSettings>(Configuration.GetSection(nameof(WordDatabaseSettings)));

            services.AddSingleton<IWordDatabaseSettings>(sp => sp.GetRequiredService<IOptions<WordDatabaseSettings>>().Value);
            services.AddScoped(typeof(IWordRepository), typeof(WordRepository));
            services.AddScoped(typeof(IWordService), typeof(WordService));
            services.AddScoped(typeof(IProcessorService), typeof(ProcessorService));
            services.AddScoped(typeof(ILeaderboardService), typeof(LeaderboardService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<WordController>();
                endpoints.MapGrpcService<ProcessorController>();
                endpoints.MapGrpcService<LeaderboardController>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
