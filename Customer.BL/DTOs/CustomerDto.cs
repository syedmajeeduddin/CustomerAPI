using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.DTOs
{
    /// <summary>
    /// DTO to read that from the API Call
    /// </summary>
    public class CustomerDto
    {
        /// <summary>
        /// First Name 
        /// </summary>
        [Required]
        public string FirstName { get; set; }


        /// <summary>
        /// Last Name 
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Date Of Birth
        /// </summary
        [Required]
        public string DateOfBirth { get; set; }
    }
}
