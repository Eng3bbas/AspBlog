using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Models;
using BlogAPI.Requests;

namespace BlogAPI.Services
{
    public interface IUserService
    {
        public object Register(UserRegisterRequest request);
        public object Login(UserLoginRequest request);
        public Task<User> GetUserById(int id);
        public Task<User> Update(UpdateRequest request);
    }
}
