using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.DTOs;
using Customers.DalEf.Models;
using Customers.DalEf.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Customers.API.Controllers
{
    /// <summary>
    /// Customer Web Api
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    
    public class CustomerController : ControllerBase
    {

        //inject the DBContext and logger into the controller...
        private CustomerRepository _repo;

        private ILogger _logger;

        /// <summary>
        /// C-tor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public CustomerController(CustomerRepository repository, ILogger logger)
        {
            
            _repo = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the Customers
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            { 
                _logger.LogInformation($"Returned all Customers from database.");
                return  Ok(await _repo.GetCustomers());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside {nameof(GetCustomers)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets customer by id 
        /// </summary>
        /// <param name="id">id </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpGet("{id:int}", Name = "GetCustomerById")]
        public ActionResult<Customer> GetCustomersById(int id)
        {
            var customer = _repo.FindById(id);
            if (customer == null)
            {
                _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Returned customer with id: {id}");
                
                return Ok(customer.Result);
            }

           
        }

        /// <summary>
        /// Add Customer 
        /// </summary>
        /// <param name="dto">Customer DTO</param>
        [ProducesResponseType(typeof(Customer), 201)]        
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpPost("Create")]       
        public async Task<IActionResult> Post( CustomerDto dto)
        {
            try
            {
                //TODO: Check if the customer already exists
                if (dto == null)
                {
                    _logger.LogError("Customer object sent from client is null.");
                    return BadRequest("Customer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Customer object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var result = await _repo.AddCustomer(dto);

                return CreatedAtRoute("GetCustomerById", new { id = result.Id }, dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Updates Customer 
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="dto">Customer DTO</param>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto dto)
        {

            try
            { 
                if (dto == null)
                {
                    _logger.LogError("Customer object sent from client is null.");
                    return BadRequest("Customer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Customer object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var customer =  _repo.FindById(id);
                if (customer == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                 await _repo.UpdateCustomer(id, dto);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }


        /// <summary>
        /// Deletes Customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
               var customer = _repo.FindById(id);
                if (customer == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _repo.DeleteCustomer(id);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Search Customer based on First Name and Last Name 
        /// </summary>
        /// <param name="filterBy">search by</param>
        /// <returns>list of Customers</returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(typeof(Exception), 500)]
        [HttpGet("Search/{filterBy}")]
        public async  Task<IActionResult> SearchCustomers(string filterBy)
        {
            //return await _repo.FindCustomerByName(filterBy).ToAsyncEnumerable();
            try
            {
                _logger.LogInformation($"Returned all Customers from database.");
                return Ok(await _repo.FindCustomerByName(filterBy));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside {nameof(GetCustomers)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
