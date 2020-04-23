using BlogAPI.Helpers;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Repositories
{
    public interface IPostImagesRepository
    {
        public Task<PaginatedList<PostImage>> All(int postId,int page = 1);
        public Task<PostImage> Create(PostImage image);
        public Task<IEnumerable<PostImage>> Create(IList<PostImage> images);
        public Task<PostImage> Delete(int imageId,int userId);
    }
}
