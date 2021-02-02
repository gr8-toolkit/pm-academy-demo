using Solid.Aim.Models;

namespace Solid.Aim.Services
{
    public interface IAccountStorage
    {
        Account FindAccount(string login, string password);
        Account FindAccount(string login);
        bool AddAccount(Account account);
    }
}
