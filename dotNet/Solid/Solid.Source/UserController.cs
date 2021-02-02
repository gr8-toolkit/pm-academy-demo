using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.Source
{
    public class UserController
    {
        private static readonly List<Account> Accounts = new List<Account>();
        private static readonly Dictionary<string, string> Tokens = new Dictionary<string, string>();
        private static readonly List<UserDto> Users = new List<UserDto>();
        
        public bool Register(string login, string password)
        {
            var id = Guid.NewGuid().ToString();

            if (Accounts.Any(acc => acc.Login == login || acc.Id == id)) return false;
            Accounts.Add(new Account
            {
                Id = id, 
                Login = login, 
                Password = password
            });
            return true;
        }

        public string Login(string login, string password)
        {
            var account = Accounts.FirstOrDefault(acc => acc.Login == login && acc.Password == password);
            if (account == null) return null;
            
            var token = Guid.NewGuid().ToString();
            Tokens.Add(token, account.Id);
            
            return token;
        }


        public UserDto GetUser(string token)
        {
            if (!Tokens.TryGetValue(token, out var id)) return null;
            return Users.FirstOrDefault(user => user.Id == id);
        }

        public void UpsertUser(string token, UserDto dto)
        {
            if (token == null || !Tokens.TryGetValue(token, out var id)) 
                throw new Exception("Unauthorized");

            ValidateDto(dto);

            dto.Id = id;
            var idx = Users.FindIndex(user => user.Id == id);
            
            if (idx < 0) Users.Add(dto);
            else Users[idx] = dto;
        }

        private void ValidateDto(UserDto dto)
        {
            if (dto == null) throw new Exception("DTO is missing");
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("Name is missing");
            if (!dto.Age.HasValue) throw new Exception("Age is missing");
            if (dto.Age.Value < 18) throw new Exception("Illegal age");
        }
    }
}
