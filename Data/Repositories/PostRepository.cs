using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Requests;
using BlogAPI.Extenstions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using static BlogAPI.Data.Repositories.IPostRepository;

namespace BlogAPI.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext context;

        public PostRepository(BlogContext context)
        {
            this.context = context;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            Post p = context.Posts.Update(post).Entity;
            await context.SaveChangesAsync();
            return p;
        }
        public async Task<PaginatedList<Post>> All(int page = 1)
        {
            
            return context.Posts.OrderByDescending(p => p.Id).Include(p => p.User).Include(p => p.PostImages).Paginate(page:page);
        }
         
        public async Task<Post> Create(Post post)
        {
            var p = await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return p.Entity;
        }

        public async Task<Post> FindById(int id)
        {
            try
            {
                return await context.Posts.AsNoTracking().Include(p => p.PostImages).Include(p => p.User).Where(p => p.Id == id).FirstAsync();

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<PostType?> Delete(int id, IHttpContextAccessor httpContext)
        {
            Post post = await context.Posts.FindAsync(id);
            if (post == null || post.UserId != int.Parse(httpContext.HttpContext.User.FindFirst("userId").Value))
                return null;
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
            return post.PostImages.Count() > 0 ? PostType.WithImages : PostType.TextsOnly;
        }
    }
}
