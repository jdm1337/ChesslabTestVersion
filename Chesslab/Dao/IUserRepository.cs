using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Microsoft.AspNetCore.Identity;

namespace Chesslab.Dao
{
    public interface IUserRepository
    {
        Task AddRole(IdentityUserRole<string> identityUserRole);
        Task<IdentityRole> GetRole(string role);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(string id);

        Task<User> GetByName(string userName);

        Task<User> GetByEmail(string email);

        Task Add(User entity);

        Task<User> EditNickName(User user, string name);

        Task<User> EditLocation(User user, string location);
        Task<User> EditAvatar(User user, string avatarLink);

        Task Delete(string id);

        Task Save();
        
    }
}
