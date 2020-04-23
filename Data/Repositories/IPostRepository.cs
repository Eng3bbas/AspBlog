using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Repositories
{
    public interface IPostRepository
    {
        enum PostType
        {
            WithImages=1,
            TextsOnly=2
        }
        public Task<PaginatedList<Post>> All(int page = 1);
        public Task<Post> Create(Post post);
        public Task<Post> UpdateAsync(Post post);
        public Task<Post> FindById(int id);
        public Task<PostType?> Delete(int id, IHttpContextAccessor httpContext);
    }
}
