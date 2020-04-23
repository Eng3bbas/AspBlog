using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/post-images/{action}")]
    [ApiController]
    [Authorize]
    public class PostImagesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPostImageService service;
        private readonly string[] ImageExtenstions = { ".jpg",".jpeg",".png" };
        public PostImagesController(IMapper mapper,IPostImageService service)
        {
            this.mapper = mapper;
            this.service = service;
        }
        [Route("{postId:int}")]
        public async Task<IActionResult> All(int postId ,[FromQuery] int page = 1)
        {
            try
            {
                var images = await service.All(postId, page);
                if (images.Items.Count() <= 0)
                    return NotFound(new { Error = "No images found with this post" });
                object response = new
                {
                    Pagination = new
                    {
                        CurrentPage = images.CurrentPage,
                        PageSize = images.PageSize,
                        DataCount = images.TotalCount,
                        TotalPages = images.TotalPages
                    },
                    Rows = mapper.Map<IList<PostImageMapper>>(images.Items)
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                return ServerError(new { Error = e.Message });
            }
        }

        [Route("{postId:int}")]
        [HttpPost]
        public async Task<IActionResult> Create(int postId, [FromServices]IPostService postService , [FromForm(Name = "images")] ICollection<IFormFile> images  )
        {
            try
            {
                var post = await postService.Show(postId);
                if (post == null)
                    return NotFound(new { Error = "Post not found with this id" });
                if (post.UserId.ToString() != HttpContext.User.FindFirst("userId").Value)
                    return Unauthorized(new { Error = "You can't edit this post" });
                if (images.Count() == 0)
                    ModelState.AddModelError("images", "images required");
                images.ToList().ForEach(img =>
                {
                    if (!ImageExtenstions.Contains(Path.GetExtension(img.FileName).ToLower()))
                        ModelState.AddModelError($"file : {img.FileName}", "Image Is not allowded");
                });
                if (!ModelState.IsValid)
                    return BadRequest(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary(ModelState));
                return StatusCode(201, new
                {
                    Images = mapper.Map<IList<PostImageMapper>>(await service.Create(postId, images))
                });
            }
            catch (Exception e)
            {
                return ServerError(new { ServerError = e.Message });
            }
           
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await service.Delete(id);
                if (!deleted)
                    return NotFound(new { Error = "image not found with this id" });
                return NoContent();
            }
            catch (Exception e)
            {
                return ServerError(new { Error = e.Message });
            }
        }
        private IActionResult ServerError(object value)
        {
            return StatusCode(500, value);
        }
    }
}