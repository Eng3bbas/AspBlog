using AutoMapper;
using BlogAPI.Data.Repositories;
using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IHttpContextAccessor http;
        private readonly AuthenticationResponse response;
        private readonly ITokenHelper tokenHelper;
        private readonly IWebHostEnvironment env;
        public UserService(IUserRepository repository,IMapper mapper,ITokenHelper tokenHelper,IWebHostEnvironment env,IHttpContextAccessor http)
        {
            this.repository = repository;
            this.response = new AuthenticationResponse(mapper);
            this.tokenHelper = tokenHelper;
            this.env = env;
            this.http = http;
        }

        public async Task<User> GetUserById(int id)
        {
            return  repository.FindById(id);
        }

        public object Login(UserLoginRequest request)
        {
            var user = repository.GetByEmail(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return null;
            string token = tokenHelper.GenerateToken(user);
            return response.Response(user, token);
        }

        public object Register(UserRegisterRequest request)
        {
            if (repository.GetByEmail(request.Email) != null)
                return null;
            var user =  repository.Create(new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name
            });
            Directory.CreateDirectory(env.WebRootPath + $"/Images/Posts/user.{user.Id}");
            string token = tokenHelper.GenerateToken(user);
            return response.Response(user, token);
        }

        public async Task<User> Update(UpdateRequest request)
        {
            var user =  repository.GetByEmail(request.Email);
            user.Id = int.Parse(http.HttpContext.User.FindFirst("userId").Value);
            user.Email = request.Email;
            user.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Password) || !string.IsNullOrWhiteSpace(request.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            return await repository.Update(user);
        }
    }
}
