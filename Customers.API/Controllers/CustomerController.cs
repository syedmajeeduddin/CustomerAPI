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
                return Ok(await _repo.GetCustomers());

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
        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<Customer> GetCustomersById(int id)
        {
            var customer = _repo.FindById(id);
            if (customer.Result  == null)
            {
                _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                return NotFound($"Customer with id: {id} not found");
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
        [ProducesResponseType(200)]
        [HttpPost("Create")]
        public async Task<IActionResult> Post(CustomerDto dto)
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

                bool customerExists = _repo.DoesCustomerAlreadyExists(dto);

                if (!customerExists)
                {
                    var result = await _repo.AddCustomer(dto);

                    return CreatedAtRoute("GetCustomerById", new { id = result.Id }, dto);
                }
                else
                    return StatusCode(200, $"customer already exists with the given First Name : {dto.FirstName} and Last Name : {dto.LastName}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add Customer action: {ex.Message}");
                return StatusCode(500, "Internal server error :" +  ex.Message);
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

                var customer = _repo.FindById(id);
                if (customer.Result  == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound($"Customer with Id {id} not found.");
                }
                await _repo.UpdateCustomer(id, dto);

                return StatusCode(200, "Customer updated successfully.");

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
                if (customer.Result  == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound($"Customer with Id {id} not found.");
                }
                await _repo.DeleteCustomer(id);

                return StatusCode(200, "Customer Deleted successfully.");

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
        public async Task<IActionResult> SearchCustomers(string filterBy)
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