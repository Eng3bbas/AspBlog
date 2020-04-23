using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Requests;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public AccountController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary(ModelState));
            return Ok(mapper.Map<UserMapper>(await userService.Update(request)));
        }
    }
}