using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IExList<T>: IExCollection<T>, IExEnumerable<T>, IEnumerable
    {
        T this[int index]
        {
            get;
            set;
        }

        int IndexOf(T item);

        void Insert(int index, T item);

        void RemoveAt(int index);
    }
}
