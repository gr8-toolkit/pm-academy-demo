using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

// TODO: add events
namespace Common
{
    /// <summary>
    /// Represents a simple last-in-first-out (LIFO) non-generic collection of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Stack<T> : IEnumerable<T>
    {
        public Node<T> Head { get; private set; }
        public int Count { get; private set; }

        /// <summary>
        /// Inserts an object at the top of the Stack.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            Node<T> node = new Node<T>(item)
            {
                Next = Head
            };
            Head = node;
            Count++;
        }

        /// <summary>
        /// Removes and returns the object at the top of the Stack.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            Node<T> temp = Head;
            Head = Head.Next;
            Count--;
            return temp.Data;
        }

        /// <summary>
        /// Returns the object at the top of the Stack without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            return Head.Data;
        }

        /// <summary>
        /// Returns an IEnumerator for the Stack.
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
