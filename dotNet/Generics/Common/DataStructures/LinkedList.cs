using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Common
{
    public class LinkedList<T> : IEnumerable<T>
    {
        public Node<T> Head { get; private set; }
        public Node<T> Tail { get; private set; }
        public int Count { get; private set; }

        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        /// <summary>
        /// Adds a new node containing the specified value at the end of the LinkedList<T>.
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            var node = new Node<T>(data);

            if (Head == null)
                Head = node;
            else
                Tail.Next = node;

            Tail = node;
            Count++;
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from the LinkedList<T>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Remove(T data)
        {
            Node<T> current = Head;
            Node<T> prev = null;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (prev != null)
                    {
                        prev.Next = current.Next;

                        if (current.Next == null)
                            Tail = prev;
                    }
                    else
                    {
                        Head = Head.Next;

                        if (Head == null)
                            Tail = null;
                    }
                    Count--;
                    return true;
                }

                prev = current;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// Determines whether a value is in the LinkedList<T>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Contains(T data)
        {
            Node<T> current = Head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the LinkedList<T>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = Head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
    }
}
