using Solid.Aim.Services;

namespace Solid.Aim.Controllers
{
    public class AuthController
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        public string Login(string login, string password)
        {
            return _auth.Login(login, password);
        }

        public bool Register(string login, string password)
        {
            return _auth.Register(login, password);
        }
    }
}
