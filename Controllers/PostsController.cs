using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Requests;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using BlogAPI.Models;
using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace BlogAPI.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostService service;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly string[] ImagesExtenstions = { ".jpg", ".jpeg", ".png", ".gif" };
        public PostsController(IPostService service,IMapper mapper,IUserService userService)
        {
            this.service = service;
            this.mapper = mapper;
            this.userService = userService;
        }
        public async Task<IActionResult> All([FromQuery]int page = 1)
        {
            if (page < 0) page = 1;
            PaginatedList<Post> posts = await service.All(page);
            if (page > posts.TotalPages)
                return BadRequest(new { Error = $" Page is bigger than total pages" });
            return Ok(new { 
                PaginationData = new {
                    PageSize = posts.PageSize,
                    CurrentPage = posts.CurrentPage,
                    DataCount = posts.TotalCount,
                    TotalPages = posts.TotalPages
                },
                Posts = mapper.Map<List<PostMapper>>(posts.Items)
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PostRequest request , ICollection<IFormFile> images)
        {
            try
            {
                if (images != null && images.Count > 0)
                {
                    
                    images.ToList().ForEach(img =>
                    {
                        if (!ImagesExtenstions.Contains(Path.GetExtension(img.FileName).ToLower()))
                            ModelState.AddModelError($"File {img.FileName}", $"Exenstion Not allowded, Allowded Extenstions are : {string.Join(',', ImagesExtenstions)}");
                    });
                    
                }
                if (!ModelState.IsValid)
                    return BadRequest(new { Errors = ModelState.ToDictionary(kv => kv.Key, kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
                Post post = await service.Create(request,images.Count >0 ? images.ToList() : null);
                post.User = await userService.GetUserById(int.Parse(HttpContext.User.FindFirst("userId").Value));
                post.User.Password = null;
                return StatusCode(201, new
                {
                    Post = mapper.Map<PostMapper>(post)
                });
            }
            catch (Exception e)
            {

                return StatusCode(500,new { Error =  $"Server Error : {e.Message.Trim('\n')}" });
            }
            
        }
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {
            try
            {
                Post post = await service.Show(id);
                if (post == null)
                {
                    return NotFound(new { Error = "Post Not found" });
                }
                return Ok(mapper.Map<PostMapper>(post));
            }
            catch (Exception e)
            {

                return StatusCode(500, new { Error = e.Message });
            }
            
        }
        [Route("{id:int}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id,PostRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ModelStateDictionary(ModelState));
                Post post = await service.Update(request, id);
                if (post == null)
                    return NotFound(new { Error = "Not found" });
                return Ok(mapper.Map<PostMapper>(post));
            }
            catch (Exception e)
            {
                return ServerError(new { Error = e.Message });
            }
            
        }
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool postDeleted = await service.Delete(id);
            if (!postDeleted)
                return NotFound();
            return NoContent();
        }
        private IActionResult ServerError(object value)
        {
            return StatusCode(500, value);
        }
    }
}