using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Loops.SimpleStack.Example1
{

    public class SimpleStack<T> : IEnumerable<T>
    {
        private readonly T[] _array;
        private int _pointer= -1;

        public int Count => _pointer + 1;
        
        public bool IsFilled => Count >= _array.Length;
        
        public bool IsEmpty => Count <= 0;

        public SimpleStack(int size)
        {
            // ctor
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
            _array = new T[size];
        }

        public void Push(T item)
        {
            if (IsFilled) throw new InvalidOperationException("Stack overflow");

            _pointer++;
            _array[_pointer] = item;
        }

        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Stack is empty");
            
            var item = _array[_pointer];
            _pointer--;
            
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            // 1. With own enumerator
            return new SimpleStackEnumerator(this);

            // 2. With yield return
            //for (var i = 0; i <= _pointer; i++)
            //{
            //    yield return _array[i];
            //}
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class SimpleStackEnumerator : IEnumerator<T>
        {
            private readonly SimpleStack<T> _stack;

            private int _position = -1;

            object IEnumerator.Current => (object)Current;
            
            public T Current =>_stack._array[_position];
                    
            public bool MoveNext()
            {
                _position++;
                return _position <= _stack._pointer;
            }

            public void Reset()
            {
                _position = -1;
            }

            public void Dispose()
            {
                // nothing to dispose
            }

            public SimpleStackEnumerator(SimpleStack<T> stack)
            {
                _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello simple stack!");
            var stack = new SimpleStack<string>(10);
            
            stack.Push("Winter");
            stack.Push("Spring");
            stack.Push("Summer");
            stack.Push("Autumn");

            Console.WriteLine("1. Stack after Push (x4) :");
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            Debug.WriteLine(stack.Pop());
            Debug.WriteLine(stack.Pop());

            Console.WriteLine("2. Stack after Pop (x2) :");
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }
        }
    }
}
