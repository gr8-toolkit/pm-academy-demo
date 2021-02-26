using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Authorization
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public const string User = "User";

        public const string Test = "Test";

        public const string NewUser = "NewUser";

        public const string AllUsers = "Admin,User,Test";

    }
}
