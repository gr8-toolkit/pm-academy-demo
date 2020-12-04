using System;

namespace DataStructuresExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DataStructuresExample");
            InlineArrayExample();
            MatrixExample();
            StackExample();
            QueueExample();
        }

        static void InlineArrayExample()
        {
            try
            {
                int[] numericArrayE1 = new int[3];                                      // 0,0,0
                int[] numericArrayE2 = new int[] { 5, 7, 2 };                           // 5,7,2
                int[] numericArrayE3 = new[] { 1, 2, 2 };                               // 1,2,2

                numericArrayE1[1] = 13;                                                 // 0 >> 13
                /* IndexOutOfRangeException: Index was outside the bounds of the array. */
                //numericArrayE3[10] = 3;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void MatrixExample()
        {
            try
            {
                int[,] numericArrayE1 = new int[2, 3];                                      // 0,0,0 | 0,0,0 
                int[,] numericArrayE2 = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };         // 1,2,3 | 4,5,6 
                int[,] numericArrayE3 = { { 1, 2, 3 }, { 7, 8, 9 } };                       // 1,2,3 | 7,8,9

                numericArrayE1[0, 1] = 12;                                                  // 0 >> 12
                numericArrayE2[1, 2] = 45;                                                  // 6 >> 45
                /* IndexOutOfRangeException: Index was outside the bounds of the array. */
                //numericArrayE2[2, 2] = 5;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

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
