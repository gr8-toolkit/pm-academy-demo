using Solid.Aim.Models;

namespace Solid.Aim.Services
{
    public interface IUserStorage
    {
        User GetUser(string userId);
        void UpsetUser(User user);
    }
}
