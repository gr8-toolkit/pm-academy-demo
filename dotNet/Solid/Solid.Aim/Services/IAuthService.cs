namespace Solid.Aim.Services
{
    public interface IAuthService
    {
        string Authorize(string token);

        string Login(string login, string password);

        bool Register(string login, string password);
    }
}
