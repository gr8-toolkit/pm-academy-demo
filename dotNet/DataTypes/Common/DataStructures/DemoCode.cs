using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataStructures
{
    internal class DemoCode
    {
        static void StackExample()
        {
            var stack = new Stack<int>();
            stack.Push(10);
            stack.Push(1);
            while (stack.Count > 0)
            {
                Console.WriteLine(stack.Pop());
            }
        }

        static void QueueExample()
        {
            var queue = new Queue<int>();
            queue.Enqueue(10);
            queue.Enqueue(1);
            while (queue.Count > 0)
            {
                Console.WriteLine(queue.Dequeue());
            }
        }
    }
}
