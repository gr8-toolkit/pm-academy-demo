using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesAndEventsExample
{
    public static class EventsInActionExample
    {
        public static void Foo()
        {
            var example = new EventsInAction();
            try
            {
                example.CommonUpdate += CommonUpdateE1;
                //example.CommonUpdate += CommonUpdateE2;
                //example.CommonUpdate -= CommonUpdateE1;

                example.OnCommonUpdateReceived(new ClientArgs(123));
                example.CommonUpdate += CommonUpdateE1;
            }
            catch (Exception e)
            {
                var o = e.Message;
                // Object reference not set to an instance of an object.
            }
            finally
            {
                example.CommonUpdate -= CommonUpdateE1;
            }
        }

        private static void CommonUpdateE1(object sender, ClientArgs e) =>
            Console.WriteLine("First handler");

        private static void CommonUpdateE2(object sender, ClientArgs e) =>
            Console.WriteLine("Second handler");
    }

    public class ClientArgs : EventArgs
    {
        public readonly int Arg;
        public ClientArgs(int val) { Arg = val; }
    }

    public class EventsInAction
    {
        public event EventHandler<ClientArgs> CommonUpdate;
        public event CustomHandler<ClientArgs> CustomUpdate;

        public void OnCommonUpdateReceived(ClientArgs e) =>
            CommonUpdate.Invoke(this, e);

        public void OnCumsomUpdateReceived(ClientArgs e) =>
            CustomUpdate?.Invoke(this, e);


        public delegate void CustomHandler<T>(object sender, T args) where T : ClientArgs;
    }
}
