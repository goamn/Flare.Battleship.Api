using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Flare.Battleship.Api.Features.GameplayFeature;
using Flare.Battleship.Api.Features.SetupFeature;
using Flare.Battleship.Api.Features.SetupFeature.Repository;
using Flare.Battleship.Api.Infrastructure.Exceptions;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;

namespace Flare.Battleship.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISetupValidationService, SetupValidationService>();
            services.AddScoped<ISetupService, SetupService>();
            services.AddScoped<IGameplayService, GameplayService>();
            services.AddSingleton<IBoardRepository, BoardRepository>();

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Flare.Battleship.Api",
                    Description = "Backend API for state management and reporting",
                    Contact = new OpenApiContact { Name = "Hussain Al-saady" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var isDevEnvironment = env.EnvironmentName.Equals("dev", StringComparison.InvariantCultureIgnoreCase) || env.IsDevelopment();
            if (isDevEnvironment)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flare.Battleship.Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
