using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    public class User : IUser
    {
        private readonly string _id;

        public User(string id)
        {
            _id = id;
        }

        public string GetName()
        {
            // lazy get name by id
            return string.Empty;
        }
    }
}
