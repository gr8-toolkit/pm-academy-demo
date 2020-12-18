using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class AppenderExample : IAppender<IExample>
    {
        public void Append(IExample data)
        {
        }
    }

    public class AppenderExample2 : IAppender<IExample>
    {
        public AppenderExample2(int abc)
        {

        }

        public void Append(IExample data)
        {
        }
    }
}
