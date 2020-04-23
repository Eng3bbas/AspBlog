using AutoMapper.Configuration.Conventions;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Mappers
{
    public class PostMapper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public UserMapper User { get; set; }
        public ICollection<PostImageMapper> PostImages { get; set; } = new HashSet<PostImageMapper>();
    }
}
