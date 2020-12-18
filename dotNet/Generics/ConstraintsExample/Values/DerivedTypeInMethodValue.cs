using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class DerivedTypeInMethodValue<T>
    {
        public void Add<U>(U item) where U : T
        {

        }
    }
}
