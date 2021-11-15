using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chesslab.Dao
{
    public class UserRepository : IUserRepository
    {
        private ApplicationContext _appContext;
        public UserRepository(ApplicationContext appContext)
        {
            _appContext = appContext;
        }
        public async Task AddRole(IdentityUserRole<string> identityUserRole)
        {
            await _appContext.UserRoles.AddAsync(identityUserRole);
        }

        public async Task<IdentityRole> GetRole(string role)
        {
            return await _appContext.Roles.FirstOrDefaultAsync(x => x.Name == role);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _appContext.Users.ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            var _user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (_user != null)
            {
                return _user;
            }

            return null;
        }

        public async Task<User> GetByName(string nickName)
        {
            var _user = await _appContext.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
            if (_user != null)
            {
                return _user;
            }

            return null;
        }

        public async Task<User> GetByEmail(string email)
        {
            var _user = await _appContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (_user != null)
            {
                return _user;
            }

            return null;
        }

        public async Task Add(User entity) => await _appContext.Users.AddAsync(entity);


        public async Task<User> EditNickName(User user, string NickName)
        {
            var _user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (_user != null)
            {
                _user.NickName = NickName;
                _appContext.Users.Update(user);
                return _user;
            }

            return  null;
        }

        public async Task<User> EditLocation(User user, string location)
        {
            var _user = await _appContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            _user.Location = location;
            _appContext.Users.Update(user);
            return _user;
        }

        public async Task Delete(string id)
        {
            await _appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Save() => await _appContext.SaveChangesAsync();
    }
}
