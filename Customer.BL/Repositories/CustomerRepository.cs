using Customers.DalEf.DbContexts;
using Customers.DTOs;
using Customers.DalEf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.DalEf.Repositories
{
    /// <summary>
    ///  Repository to fetch data 
    /// </summary>
    public class CustomerRepository
    {
        private CustomerDBContext _dbContext;
        

       /// <summary>
       /// C-Tor
       /// </summary>
       /// <param name="dbContext"></param>
       
        public CustomerRepository(CustomerDBContext dbContext)
        {
            _dbContext = dbContext;
           
        }

        /// <summary>
        /// Get all Customers
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetCustomers()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        /// <summary>
        /// Get Customer by Id
        /// </summary>
        /// <returns></returns>
        public async Task<Customer> FindById(int id)
        {
            return await  _dbContext.Customers.Where(e => e.Id == id).FirstOrDefaultAsync(); ;
        }

        /// <summary>
        /// Add new Customer
        /// </summary>
        /// <param name="dto">dto</param>
        /// <returns>Customer Id </returns>
        public async Task<Customer> AddCustomer(CustomerDto dto)
        {
             var newCustomer = new Customer() { FirstName = dto.FirstName, LastName = dto.LastName, DOB = Convert.ToDateTime(dto.DateOfBirth) };
            _dbContext.Customers.Add(newCustomer);
                await SaveAsync();           
            
             return  newCustomer;

        }

        /// <summary>
        /// Updates Customer
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="dto">Customer Dto</param>
        public async Task UpdateCustomer( int id, CustomerDto dto)
        {

            var customer = _dbContext.Customers.Find(id);
            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.DOB = Convert.ToDateTime(dto.DateOfBirth);

            _dbContext.Update(customer);
            await SaveAsync();

        }

        /// <summary>
        /// Customer to be deleted
        /// </summary>
        /// <param name="id">Customer Id</param>
        public async Task DeleteCustomer(int id)
        {

            var customer = _dbContext.Customers.Find(id);
            _dbContext.Customers.Remove(customer);
            await SaveAsync();
           
        }

        /// <summary>
        /// Search Customer by First Or Last Name 
        /// </summary>
        /// <param name="filterBy">search parameter</param>
        /// <returns>list of customers</returns>
        public async Task<List<Customer>> FindCustomerByName (string filterBy)
        {
            var customer = _dbContext.Customers.Where(e => (e.FirstName.ToLower().Contains(filterBy.ToLower()) || e.LastName.ToLower().Contains(filterBy.ToLower())));
            return await customer.ToListAsync();
        }


        private async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }



    }
}
