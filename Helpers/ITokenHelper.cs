using BlogAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Helpers
{
   public interface ITokenHelper
    {
        public string GenerateToken(User user);
    }
}
