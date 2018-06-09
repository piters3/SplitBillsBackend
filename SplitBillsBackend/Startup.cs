using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SplitBillsBackend.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace SplitBillsBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SplitBillsDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("SplitBillsConnection")
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SplitBills;Trusted_Connection=True;MultipleActiveResultSets=true")
                    //,
                    //b => b.MigrationsAssembly("SplitBillsBackend")
                    );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Split Bills API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Piotr Strzelecki",
                        Email = "Piotr.Strzelecki93@gmail.com",
                        Url = "https://www.facebook.com/pioter.strzelecki"
                    },
                    License = new License
                    {
                        Name = "Use under MIT License",
                        Url = "https://opensource.org/licenses/MIT"
                    }
                });
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Split Bills API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }
    }
}
