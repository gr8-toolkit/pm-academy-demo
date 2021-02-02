using System;
using System.Collections.Generic;
using System.Linq;
using Solid.Aim.Models;

namespace Solid.Aim.Services.Impl
{
    internal class AccountStorage : IAccountStorage
    {
        private readonly List<Account> _accounts = new List<Account>();

        public bool Register(string login, string password)
        {
            var id = Guid.NewGuid().ToString();
            if (_accounts.Any(acc => acc.Login == login || acc.Id == id)) return false;
            
            _accounts.Add(new Account
            {
                Id = id, 
                Login = login, 
                Password = password
            });
            
            return true;
        }
        
        public Account FindAccount(string login, string password)
        {
            return _accounts.FirstOrDefault(acc =>
                acc.Login == login &&
                acc.Password == password);
        }

        public Account FindAccount(string login)
        {
            return _accounts.FirstOrDefault(acc => acc.Login == login);
        }

        public bool AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (_accounts.Any(acc => acc.Id == account.Id)) return false;
            _accounts.Add(account);
            return true;
        }
    }
}
