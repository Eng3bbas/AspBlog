using BlogAPI.Helpers;
using BlogAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public interface IPostImageService
    {
        public Task<PaginatedList<PostImage>> All(int postId,int page = 1);
        public Task<IList<PostImage>> Create(int postId, ICollection<IFormFile> files);

        public Task<bool> Delete(int imageId);
    }
}
