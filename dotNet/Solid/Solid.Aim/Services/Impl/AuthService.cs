using System;
using System.Collections.Generic;
using Solid.Aim.Models;

namespace Solid.Aim.Services.Impl
{
    internal class AuthService : IAuthService
    {
        // SRP : need to move tokens storage to external class
        private readonly Dictionary<string, string> _tokens = new Dictionary<string, string>();
        
        // SRP : account storage has been moved to another class
        private readonly IAccountStorage _storage;

        public AuthService(IAccountStorage storage)
        {
            _storage = storage;
        }

        public bool Register(string login, string password)
        {
            return _storage.AddAccount(new Account
            {
                Id = Guid.NewGuid().ToString(),
                Login = login,
                Password = password
            });
        }

        public string Login(string login, string password)
        {
            var account = _storage.FindAccount(login, password);
            if (account == null) return null;

            var token = Guid.NewGuid().ToString();
            _tokens.Add(token, account.Id);

            return token;
        }

        public string Authorize(string token)
        {
            // exchange token to account id
            if (token == null || !_tokens.ContainsKey(token)) return null;
            return _tokens[token];
        }
    }
}
