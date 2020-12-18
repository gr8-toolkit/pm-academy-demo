using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{
    public class GenericIndexerExample
    {
        public GenericIndexerExample()
        {
            var int32Store = new Store<int>(10);
            var stringStore = new Store<string>(5);

            var int32Val1 = int32Store[0];                  // 0
            var stringVal1 = stringStore[0];                // null
        }
    }


    public class Store<T>
    {
        private T[] _array;

        public Store(int capacity)
        {
            _array = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                CheckIndex(index);
                return _array[index];
            }
            set
            {
                CheckIndex(index);
                _array[index] = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index > _array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
