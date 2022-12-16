using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MovieMaster.Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Net.Mime.MediaTypeNames;

namespace MovieMaster
{
    /// <summary>
    /// Startup file contains the common configurations to the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Interface for configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"> service </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Movie Master",
                        Version = "v1",
                        Description = "The best movie database In The World",
                    });

                AddSwaggerXmlComments(c);
            });
            services.ConfigureServices();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Movie Master V1");
                });
            }

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = Text.Plain;
                    await context.Response.WriteAsync("Internal Server Error !!!");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    var response = new { error = exceptionHandlerPathFeature.Message };
                    logger.LogCritical("Exception thrown startup : " + response);
                });
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void AddSwaggerXmlComments(SwaggerGenOptions options)
        {
            foreach (var xmlDocFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.XML"))
            {
                options.IncludeXmlComments(xmlDocFile);
            }
        }


    }
}