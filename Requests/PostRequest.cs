using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Requests
{
    public class PostRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }        
    }
}
