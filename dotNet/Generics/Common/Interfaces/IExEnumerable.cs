using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IExEnumerable<out T> : IEnumerable
    {
        new IEnumerator<T> GetEnumerator();
    }
}
