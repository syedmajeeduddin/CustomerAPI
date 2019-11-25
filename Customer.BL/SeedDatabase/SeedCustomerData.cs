using Customers.DalEf.DbContexts;
using Customers.DalEf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.DalEf.SeedDatabase
{
    public class SeedCustomerData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new CustomerDBContext(
            serviceProvider.GetRequiredService<DbContextOptions<CustomerDBContext>>()))
            {


                if (!dbContext.Customers.Any())
                    dbContext.Customers.AddRange(new[]
                    {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    },
                    new Customer(){
                        FirstName = "John", LastName = "Alexander", DOB = DateTime.Now.AddYears(-35)
                    },
                    new Customer(){
                        FirstName = "Trent", LastName = "Smith", DOB = DateTime.Now.AddYears(-56)
                    }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
