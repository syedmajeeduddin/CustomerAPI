﻿using Customers.DalEf.DbContexts;
using Customers.DalEf.SeedDatabase;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace Customers.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //1. Get Host
           var host = CreateWebHostBuilder(args).Build();

            //2. Find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                //3. Get the instance of CustomerDBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CustomerDBContext>();
               

                //4. Call the SeedCustomerData to create sample data
                SeedCustomerData.Initialize(services);
            }

            //Continue to run the application
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
