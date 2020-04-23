using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI
{
    public class AuthenticationResponse
    {
        private IMapper mapper;
        public AuthenticationResponse(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public object Response(User user, string token)
        {
            return new
            {
                UserData = mapper.Map<UserMapper>(user),
                AccessToken = token
            };
        }
    }
}
