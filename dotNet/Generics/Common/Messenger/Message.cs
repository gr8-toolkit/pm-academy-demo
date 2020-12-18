using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messenger
{
    public class Message<TUser, TMessage, TMeta> : IMessage 
        where TUser: IUser
        where TMessage: IMessageEntry
        where TMeta: IMessageMetadata
    {
        /// <summary>
        /// Mesage sender.
        /// </summary>
        public TUser Sender { get; set; }

        /// <summary>
        /// Message receiver.
        /// </summary>
        public TUser Receiver { get; set; }

        /// <summary>
        /// Message entry.
        /// </summary>
        public TMessage Data { get; set; }

        /// <summary>
        /// Message metadata.
        /// </summary>
        public TMeta Metadata { get; set; }


        public void Send()
        {
            Console.WriteLine($"New message was sent from {Sender.GetName()} to {Receiver.GetName()}");
        }
    }
}
