using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{
    public class GenericEventExample
    {
        public GenericEventExample()
        {
            var inst1 = new GenericEventInAction<ClientArgsV1>();
            var inst2 = new GenericEventInAction<ClientArgsV2>();

            inst1.CommonEvent += Client1Update;
            // erorr: No overload for 'Client1Update' matches delegate 'EventHandler<ClientArgsV2>'
            //inst2.CommonEvent += Client1Update;
            inst2.CommonEvent += Client2Update;


            inst1.CommonEvent += CommonUpdate<ClientArgsV1>;
            inst2.CommonEvent += CommonUpdate<ClientArgsV2>;
        }

        private static void Client1Update(object sender, ClientArgsV1 e) =>
            Console.WriteLine("First handler");

        private static void Client2Update(object sender, ClientArgsV2 e) =>
            Console.WriteLine("Second handler");

        private static void CommonUpdate<T>(object sender, T e) =>
            Console.WriteLine($"Common handler {typeof(T)}");
    }

    public class GenericEventInAction<T>
    {
        public event EventHandler<T> CommonEvent;

        public void Execute(T args)
        {
            CommonEvent?.Invoke(this, args);
        }
    }

    public class ClientArgsV1 : EventArgs
    {

    }

    public class ClientArgsV2 : EventArgs
    {

    }
}
