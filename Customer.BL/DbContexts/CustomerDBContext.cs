using Customers.DalEf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.DalEf.DbContexts
{
    public class CustomerDBContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerDBContext()
        { }

        public CustomerDBContext(DbContextOptions<CustomerDBContext> options)
         : base(options)
        {
        }
    }
}
