using Customers.DalEf.DbContexts;
using Customers.DTOs;
using Customers.DalEf.Models;
using Customers.DalEf.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerUnitTesting
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public void GetCustomers_ReturnsThreeCustomers()
        {

            // Arrange

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

           

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    },
                    new Customer(){
                        FirstName = "John", LastName = "Alexander", DOB = DateTime.Now.AddYears(-35)
                    },
                    new Customer(){
                        FirstName = "Trent", LastName = "Smith", DOB = DateTime.Now.AddYears(-56)
                    } });

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);

                // Act
                var customers = customerRepository.GetCustomers();

                // Assert
                Assert.Equal(3,  (customers.Result).Count);
            }
        }

        [Fact]
        public void GetCustomerById_ReturnsCustomerByIdTwo()
        {

            // Arrange

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;



            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    },
                    new Customer(){
                        FirstName = "John", LastName = "Alexander", DOB = DateTime.Now.AddYears(-35)
                    },
                    new Customer(){
                        FirstName = "Trent", LastName = "Smith", DOB = DateTime.Now.AddYears(-56)
                    } });

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);

                // Act
                var customer = customerRepository.FindById(2);

                // Assert
                Assert.Equal("John", (customer.Result).FirstName);
            }
        }

        [Fact]
        public void AddCustomer_ReturnsCustomerByName()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    } } );

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);
                var dto = new CustomerDto() { FirstName = "Mathew", LastName = "Hayden", DateOfBirth = "1982/12/10" };

                // Act
                var customer = customerRepository.AddCustomer(dto);

                // Assert
                Assert.Equal("Mathew", (customer.Result).FirstName);
            }
        }

        [Fact]
        public async Task UpadeCustomer_UpdatesCustomerWithIdOne()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    } });

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);
                var dto = new CustomerDto() { FirstName = "Ricky", LastName = "Pointing", DateOfBirth = "1982/12/10" };

                // Act
                 await customerRepository.UpdateCustomer(1,dto);

                var customer = customerRepository.FindById(1);


                // Assert
                Assert.Equal("Ricky", (customer.Result).FirstName);
            }
        }

        [Fact]
        public async Task DeleteCustomer_DeleteCustomerByIdOne()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    },
                    new Customer(){
                        FirstName = "John", LastName = "Alexander", DOB = DateTime.Now.AddYears(-35)
                    } });

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);

                // Act
                await customerRepository.DeleteCustomer(2);

                var customers = customerRepository.GetCustomers();


                // Assert
                Assert.Single<Customer>( customers.Result);
            }
        }

        [Fact]
        public void SearchCustomer_SearchByJohn()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CustomerDBContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Customers.AddRange(new[]
                   {
                    new Customer(){
                        FirstName = "Syed", LastName = "Majeeduddin", DOB = DateTime.Now.AddYears(-25)
                    },
                    new Customer(){
                        FirstName = "John", LastName = "Syed", DOB = DateTime.Now.AddYears(-35)
                    } });

                context.SaveChanges();

                var customerRepository = new CustomerRepository(context);

                // Act
               var customers = customerRepository.FindCustomerByName("Syed");


                // Assert
                Assert.Equal(2, customers.Result.Count);
            }
        }
    }
}
