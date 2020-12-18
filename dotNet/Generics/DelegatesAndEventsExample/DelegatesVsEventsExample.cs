using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesAndEventsExample
{
    public static class DelegatesVsEventsExample
    {
        public static void Foo()
        {
            var inst1 = new DelegatesVsEvents();
            inst1.CommonUpdate += CommonUpdateM1;
            inst1.CommonUpdate += CommonUpdateM2;

            inst1.SpecialUpdate += SpecialUpdateM1;
            inst1.SpecialUpdate += SpecialUpdateM2;


            inst1.PerformCommonUpdate(new DelegatesVsEventsArgs());
            inst1.PerformCustomUpdate(new DelegatesVsEventsArgs());

            // error
            //inst1.CommonUpdate = (sender, args) => Console.WriteLine("CommonUpdateM3");
            inst1.SpecialUpdate = (sender, args) => Console.WriteLine("SpecialUpdateM3");

            inst1.PerformCustomUpdate(new DelegatesVsEventsArgs());

            // TODO: unsubscribe example
        }

        private static void CommonUpdateM1(object sender, IDelegatesVsEventsArgs e)
        {
            Console.WriteLine("CommonUpdateM1");
        }

        private static void SpecialUpdateM1(object sender, IDelegatesVsEventsArgs e)
        {
            Console.WriteLine("SpecialUpdateM1");
        }

        private static void CommonUpdateM2(object sender, IDelegatesVsEventsArgs e)
        {
            Console.WriteLine("CommonUpdateM2");
        }

        private static void SpecialUpdateM2(object sender, IDelegatesVsEventsArgs e)
        {
            Console.WriteLine("SpecialUpdateM2");
        }
    }


    public class DelegatesVsEvents
    {
        public delegate void DelegateExampleHandler<T>(object sender, T args) where T : IDelegatesVsEventsArgs;

        public event EventHandler<IDelegatesVsEventsArgs> CommonUpdate;
        public DelegateExampleHandler<IDelegatesVsEventsArgs> SpecialUpdate;

        public void PerformCommonUpdate(IDelegatesVsEventsArgs args)
        {
            CommonUpdate?.Invoke(this, args);
        }
        public void PerformCustomUpdate(IDelegatesVsEventsArgs args)
        {
            SpecialUpdate?.Invoke(this, args);
        }

    }

    public class DelegatesVsEventsArgs : IDelegatesVsEventsArgs
    {

    }

    public interface IDelegatesVsEventsArgs
    {

    }
}
