using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Extenstions;
using Microsoft.EntityFrameworkCore;
using System.IO;
namespace BlogAPI.Data.Repositories
{
    public class PostImagesRepository : IPostImagesRepository
    {
        private readonly BlogContext context;

        public PostImagesRepository(BlogContext context)
        {
            this.context = context;
        }

        public async Task<PaginatedList<PostImage>> All(int postId,int page = 1)
        {
            return context.PostImages.Where(img => img.PostId == postId).Paginate(page:page);
        }

        public async Task<PostImage> Create(PostImage image)
        {
      
            var img = await context.PostImages.AddAsync(image);
            await context.SaveChangesAsync();
            return img.Entity;
        }

        public async Task<IEnumerable<PostImage>> Create(IList<PostImage> images)
        {
           
            await context.PostImages.AddRangeAsync(images);
            await context.SaveChangesAsync();
            return await context.PostImages.Where(img => img.PostId == images[0].PostId).OrderByDescending(img => img.Id).Take(images.Count).ToListAsync(); 
        }
        public async Task<PostImage> Delete(int imageId,int userId)
        {
            try
            {
                var image = await context.PostImages.Include(img => img.Post).ThenInclude(p => p.User).Where(img => img.Id == imageId && img.Post.UserId == userId ).FirstAsync();
                context.PostImages.Remove(image);
                await context.SaveChangesAsync();
                return image;
            }
            catch (Exception)
            {

                return null;
            }
            
        }
    }
}
