using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{
    public class GenFinalizerInAction<T>
    {
        // Finalizer should not include any generic parameters
        ~GenFinalizerInAction()
        {

        }
    }
}
