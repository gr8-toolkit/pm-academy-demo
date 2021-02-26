using System.Linq;
using Solid.Aim.Contracts;
using Solid.Aim.Exceptions;
using Solid.Aim.Mappings;
using Solid.Aim.Services;

namespace Solid.Aim.Controllers
{
    public class UserController
    {
        private readonly IAuthService _auth;
        private readonly IUserStorage _storage;
        private readonly IUserValidator _validator;
        private readonly ILogger _logger;

        public UserController(
            IAuthService auth, 
            IUserStorage storage, 
            IUserValidator validator, 
            ILogger logger)
        {
            _auth = auth;
            _storage = storage;
            _validator = validator;
            _logger = logger;
        }

        public UserDto GetUser(string token)
        {
            var userId = _auth.Authorize(token);
            if (userId == null) throw new AuthException("Unauthorized");
            
            var user = _storage.GetUser(userId);
            return user.ToUserDto();
        }

        public void UpsertUser(string token, UserDto dto)
        {
            var userId = _auth.Authorize(token);
            if (userId == null) throw new AuthException("Unauthorized");
            
            try
            {
                // LSP demo
                var validResults = _validator.Validate(dto).ToList();
                validResults.ForEach(result => result.ThrowIfFail());
            }
            catch (ValidationException e)
            {
                _logger.Log($"Validation failed : {e.Message}");
                throw;
            }

            var user = dto.ToUser(userId);
            _storage.UpsetUser(user);
        }
    }
}
