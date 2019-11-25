using System;
using System.Collections.Generic;
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
        public string FirstName { get; set; }


        /// <summary>
        /// Last Name 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Date Of Birth
        /// </summary>
        public string DateOfBirth { get; set; }
    }
}
