using AutoMapper.Configuration.Conventions;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Mappers
{
    public class PostImageMapper
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int PostId { get; set; }
       
    }
}
