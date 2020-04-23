using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BCrypt;
namespace BlogAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(200)]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,MaxLength(62)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
