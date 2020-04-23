using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Data.Repositories;
using BlogAPI.Helpers;
using BlogAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BlogAPI.Extenstions;
using System.IO;
namespace BlogAPI.Services
{
    public class PostImageService : IPostImageService
    {
        private readonly IPostImagesRepository repository;
        private readonly IHttpContextAccessor httpContext;
        private readonly IWebHostEnvironment env;

        public PostImageService(IPostImagesRepository repository,IHttpContextAccessor httpContext,IWebHostEnvironment env)
        {
            this.repository = repository;
            this.httpContext = httpContext;
            this.env = env;
        }

        public async Task<PaginatedList<PostImage>> All(int postId, int page = 1)
        {
            return await repository.All(postId, page);
        }


        public async Task<IList<PostImage>> Create(int postId , ICollection<IFormFile> files)
        {
            List<PostImage> postImages = new List<PostImage>();
            string dir = $"Images/Posts/user.{httpContext.HttpContext.User.FindFirst("userId").Value}/post.{postId}";
            if (!Directory.Exists(env.WebRootPath + "/" + dir))
                Directory.CreateDirectory(env.WebRootPath + "/" + dir);
            files.ToList().ForEach(async img =>
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName).ToLower();
                
                using(FileStream stream = new FileStream(Path.Combine(env.WebRootPath + "/" + dir,fileName),FileMode.Create))
                {
                   await img.CopyToAsync(stream);
                }
                postImages.Add(new PostImage { PostId = postId , Path = $"{dir}/{fileName}" });
            });
            return (await repository.Create(postImages)).ToList();
        }

        public async Task<bool> Delete(int imageId)
        {
            int userId = int.Parse(httpContext.HttpContext.User.FindFirst("userId").Value);
            var image = await repository.Delete(imageId, userId);
            if (image == null)
                return false;
            File.Delete(env.WebRootPath + "/" + image.Path);
            return true;
        }
    }
}
