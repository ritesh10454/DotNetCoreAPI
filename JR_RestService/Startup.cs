using JR_RestService.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JR_RestService.Interfaces;
using JR_RestService.Repository;
using Newtonsoft.Json.Serialization;

namespace JR_RestService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<JRContext>(x => x.UseSqlServer(Configuration.GetConnectionString("cnd")));
            services.AddDbContext<JRContext>(x => x.UseSqlServer(Configuration.GetConnectionString("cndTacl")));
            // services.AddScoped
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IJRService, JR_Repository>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyJRPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                        .AllowAnyHeader()
                         .SetIsOriginAllowed(origin => true)// allow any origin
                        .AllowCredentials()// allow credentials
                        .WithExposedHeaders("X-Pagination");
                    });
            });
            services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //services.AddMvc(options => { options.Filters.Add(typeof(CustomExceptionFilterAttribute)); })
            //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("MyJRPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
