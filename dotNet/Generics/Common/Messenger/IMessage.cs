using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    public interface IMessage
    {
        /// <summary>
        /// Sends message.
        /// </summary>
        void Send();
    }
}
