using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Customers.API.Controllers;
using Customers.DalEf.DbContexts;
using Customers.DalEf.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Customers.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
            services.AddScoped<ILogger, Logger<CustomerController>>();
            services.AddScoped<CustomerRepository>();

            services.AddDbContext<CustomerDBContext>(options => options.UseInMemoryDatabase(databaseName: "CustomerDB"));

            services.AddApiVersioning(delegate (ApiVersioningOptions v)
            {
                v.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Customer Management API", Version = "v1" });

                // Get xml comments path
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Set xml path
                options.IncludeXmlComments(xmlPath);

                options.OperationFilter<RemoveVersionParameter>(Array.Empty<object>());

                options.DocumentFilter<ReplaceVersionWithExactValueInPath>(Array.Empty<object>());
                //options.DocInclusionPredicate(delegate (string version, ApiDescription desc)
                //{
                //    IEnumerable<ApiVersion> source = desc.ControllerAttributes().OfType<ApiVersionAttribute>().SelectMany((ApiVersionAttribute attr) => attr.Versions);
                //    ApiVersion[] array = desc.ActionAttributes().OfType<MapToApiVersionAttribute>().SelectMany((MapToApiVersionAttribute attr) => attr.Versions)
                //        .ToArray();
                //    if (source.Any((ApiVersion v) => "v" + v.ToString() == version))
                //    {
                //        if (array.Length != 0)
                //        {
                //            return array.Any((ApiVersion v) => "v" + v.ToString() == version);
                //        }
                //        return true;
                //    }
                //    return false;
                //});
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable swagger-ui; specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Management API V1");
            });

            app.UseMvc();
        }
    }
}
