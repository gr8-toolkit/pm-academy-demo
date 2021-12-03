using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.DataStructures
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> : IEnumerable<T>
    {
        public Node<T> Head { get; private set; }
        public Node<T> Tail { get; private set; }
        public int Count { get; private set; }

        /// <summary>
        /// Adds an object to the end of the Queue.
        /// </summary>
        /// <param name="data"></param>
        public void Enqueue(T data)
        {
            Node<T> node = new Node<T>(data);
            Node<T> tempNode = Tail;
            Tail = node;
            if (Count == 0)
                Head = Tail;
            else
                tempNode.Next = Tail;
            Count++;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            T output = Head.Data;
            Head = Head.Next;
            Count--;
            return output;
        }

        /// <summary>
        /// Returns the object at the beginning of the Queue without removing it.
        /// </summary>
        public T Peek
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty.");
                }

                return Head.Data;
            }
        }

        /// <summary>
        /// Removes all objects from the Queue.
        /// </summary>
        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        /// <summary>
        /// Determines whether an element is in the Queue.
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
        /// Returns an enumerator that iterates through the Queue.
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

