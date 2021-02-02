using System;
using System.Collections.Generic;
using System.Linq;
using Solid.Aim.Models;

namespace Solid.Aim.Services.Impl
{
    internal class UserStorage : IUserStorage
    {
        private readonly List<User> _users = new List<User>();
        public User GetUser(string userId)
        {
            return _users.FirstOrDefault(user => user.Id == userId);
        }

        public void UpsetUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var existedIndex = _users.FindIndex(u => u.Id == user.Id);
            
            // add if not exists, otherwise - replace
            if (existedIndex < 0) _users.Add(user);
            else _users[existedIndex] = user;
        }
    }
}
