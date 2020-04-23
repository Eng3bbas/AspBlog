using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Helpers;
namespace BlogAPI.Extenstions
{
    public static class PaginationExtention
    {
        public static PaginatedList<T> Paginate<T>(this IQueryable<T> query , int recordsPerPage = 15 , int page = 1)
        {
            int count = query.Count();
            int skipRows = (page - 1) * recordsPerPage;
            return new PaginatedList<T> { 
                TotalPages = (int)Math.Ceiling(count / (double)recordsPerPage),
                TotalCount = count,
                CurrentPage = page,
                PageSize = recordsPerPage,
                Items = query.Skip(skipRows).Take(recordsPerPage).ToList()
            };
        }
    }
}
