using AutoMapper;
using BlogAPI.Data.Mappers;
using BlogAPI.Data.Repositories;
using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BlogAPI.Data.Repositories.IPostRepository;

namespace BlogAPI.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPostRepository repository;
        private readonly IWebHostEnvironment env;
        private readonly IPostImagesRepository postImagesRepository;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public PostService(IHttpContextAccessor httpContextAccessor,IPostRepository repository,IWebHostEnvironment env,IPostImagesRepository postImagesRepository , IUserService userService , IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
            this.env = env;
            this.postImagesRepository = postImagesRepository;
            this.userService = userService;
            this.mapper = mapper;
        }
        public async Task<PaginatedList<Post>> All(int page = 1) => await repository.All(page);
        public async Task<Post> Create(PostRequest request, IList<IFormFile> Images = null)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst("userId").Value;
            var post = repository.Create(new Post
            {
                Title = request.Title,
                Description = request.Description,
                UserId =  int.Parse(userId)
            }).Result;
            if(Images != null)
            {
                if(!Directory.Exists(env.WebRootPath +"/Images/Posts"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "/Images/Posts"); 
                }
                string dir = $"Images/Posts/user.{userId}/post.{post.Id}";
                Directory.CreateDirectory(env.WebRootPath + $"/{dir}");
                List <PostImage> images = new List<PostImage>();
                foreach (IFormFile Image in Images)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    using(FileStream stream = new FileStream(Path.Combine(env.WebRootPath + $"/{dir}", fileName), FileMode.Create))
                    {
                       await Image.CopyToAsync(stream);
                    }
                    images.Add(new PostImage
                    {
                        PostId = post.Id,
                        Path = $"{dir}/{fileName}"
                    });
                }
               await postImagesRepository.Create(images);
            }
            return post;
        }

        public async Task<bool> Delete(int id)
        {
            PostType? postDeleted = await repository.Delete(id,httpContextAccessor);
            var userId = httpContextAccessor.HttpContext.User.FindFirst("userId").Value;
            if (postDeleted == null)
                return false;
            if(postDeleted == PostType.WithImages)
                Directory.Delete(env.WebRootPath + $"/Images/Posts/user.{userId}/post.{id}");
            return true;
        }

        public async Task<Post> Show(int id)
        {
            try
            {
                return await repository.FindById(id);

            }
            catch (Exception)
            {

                return null;
            }
          
        }
        public async Task<Post> Update(PostRequest request,int id)
        {
            Post post = await repository.FindById(id);
            int userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst("userId").Value);
            if (post == null || post.UserId != userId)
                return null;
            post.Title = request.Title;
            post.Description = request.Description;
            return await repository.UpdateAsync(post);
        }
        private string PostSlug(string postTitle)
        {
            string[] postChars = postTitle.Split(' ');
            return string.Join('_', postChars);
        }
    }
}
