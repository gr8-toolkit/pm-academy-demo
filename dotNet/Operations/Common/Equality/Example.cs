using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Equality
{
    public interface IExample
    {
        void Execute();
    }

    public class ExampleClass : IExample
    {
        public void Execute()
        {
            //
        }
    }

    public struct ExamlleStruct : IExample
    {
        public void Execute()
        {
            //
        }
    }
}
