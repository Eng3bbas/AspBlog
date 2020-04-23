using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Requests
{
    public class UserRegisterRequest
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
