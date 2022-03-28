using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationNeoPharm.Authenticate;

namespace WebApplicationNeoPharm
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

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplicationNeoPharm", Version = "v1" });
            });
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                  .AddConsole();
                builder.AddDebug();

            });

            services.AddTransient<ValidateHeaderHandler>();

            //For Basic Authentication
            string authInfo = "D002F8E1CEFA4567AB6032DF9EAA4D0D" + ":" + "PAT";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

            services.AddHttpClient("priority", c =>
            {
                //https://ngpapi.neopharmgroup.com/odata/Priority/tabula.ini/eld0999/NEO_APICUST?$filter=( PHONE2 eq '053-8778799' or PHONENUM eq '053-8778799' )&$top=1
                //c.BaseAddress = new Uri("https://ngpapi.neopharmgroup.com/odata/Priority/tabula.ini/eld0999/");
                c.BaseAddress = new Uri(Configuration["Priority_Url:BaseUrl"]);

                // Github API versioning
                c.DefaultRequestHeaders.Add("Accept", "application/json; charset=utf-8");
                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("Authorization", "Basic " + authInfo);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationNeoPharm v1"));
            }







            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
