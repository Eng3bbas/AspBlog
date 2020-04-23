using BlogAPI.Helpers;
using BlogAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<PaginatedList<User>> All();
        public User Create(User data);
        public Task<User> Update(User data);
        public User? FindById(int id);
        public void Delete(int Id);
        public User? GetByEmail(string email);
        public Task<PaginatedList<User>> GetByRole(bool isAdmin = false);
    }
}
