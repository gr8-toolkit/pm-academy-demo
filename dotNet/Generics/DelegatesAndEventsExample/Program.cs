using System;

namespace DelegatesAndEventsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Delegates & Events");
            DelegatesInAction.Foo();
            BuildinDelegates.Foo();
            EventsInActionExample.Foo();
            DelegatesVsEventsExample.Foo();
            EnclosingInAction.Foo();
            Console.ReadLine();
        }
    }
}
