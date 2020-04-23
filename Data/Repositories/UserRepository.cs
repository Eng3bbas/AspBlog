using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Extenstions;
using BlogAPI.Extenstions;
using BlogAPI.Helpers;

namespace BlogAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext context;

        public UserRepository(BlogContext context)
        {
            this.context = context;
        }

        public async Task<PaginatedList<User>> All()
        {
            return context.Users.Paginate();
        }

        public User Create(User data)
        {
            var result = context.Users.Add(data).Entity;
             context.SaveChanges();
            return result;
        }

        public void Delete(int Id)
        {
            context.Users.Remove(FindById(Id));
            context.SaveChanges();
        }

        public User? FindById(int id)
        {
            return context.Users.Find(id);
        }

        public User GetByEmail(string email)
        {
            try
            {
                 return context.Users.Where(u => u.Email == email).First();
            }
            catch (Exception)
            {

                return null;
            }
           
        }

        public async Task<PaginatedList<User>> GetByRole(bool isAdmin = false)
        {
            return context.Users.Where(u => u.IsAdmin == isAdmin).Paginate(); 
        }

        public async Task<User> Update(User data)
        {
             var user = context.Users.Update(data).Entity;
             await context.SaveChangesAsync();
             return user;
        }

       
    }
}
