using AutoMapper;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            
            CreateMap<User, UserMapper>();
            CreateMap<PostImage, PostImageMapper>();

            CreateMap<Post, PostMapper>()
                .ForMember(pm => pm.User, m => m.MapFrom(p => p.User))
                .ForMember(pm => pm.PostImages, m => m.MapFrom(p => p.PostImages));
        }
    }
}
