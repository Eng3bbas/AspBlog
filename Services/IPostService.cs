using BlogAPI.Helpers;
using BlogAPI.Models;
using BlogAPI.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Services
{
    public interface IPostService
    {
        public Task<PaginatedList<Post>> All(int page = 1);
        public Task<Post> Create(PostRequest request, IList<IFormFile> Images = null);
        public Task<Post> Update(PostRequest request,int id);
        public Task<Post> Show(int id);

        public Task<bool> Delete(int id);
    }
}
