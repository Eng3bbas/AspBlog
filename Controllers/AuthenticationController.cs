using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BlogAPI.Helpers;
using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Services;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService service;

        public AuthenticationController(IUserService service)
        {
            this.service = service;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            try
            {
      
                if (!ModelState.IsValid)
                    return BadRequest(new { Errors = ModelState.ToDictionary(key => key.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
                var result = service.Register(request);
                if (result == null)
                    return BadRequest(new { Errors = new { Email = new string[] { "Email is used" }  } });
                return StatusCode(201, result);
            }
            catch (Exception e)
            {
                
                return StatusCode(500,new { Error = $"Server Error :{e.Message}" });
            }
      
        }
        [HttpPost,Route("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { Errors = ModelState });
                var result = service.Login(request);
                if (result == null)
                    return StatusCode(401, new { Error = "check your credntails" });
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = $"Server Error : {e.Message}" });
            }
            
        }
        
    }
} 