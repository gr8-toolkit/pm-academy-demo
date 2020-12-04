using System;
using System.Diagnostics;

namespace Loops.Do.Example2
{
    public class Program
    {
        private const int Min = 1;
        private const int Max = 100;
        private const string StopWord = "exit";
        
        static void Main()
        {
            Console.WriteLine("Guess number!");
            Console.WriteLine($"I'm thinking about [{Min}..{Max}] ");
            
            var random = new Random();
            var number = random.Next(Min, Max + 1); // Max+1 - because of [Min, Max) behaviour of .Next(...)

            // cheating
            Debug.WriteLine($"I guess {number}");

            GuessNumber(number);
        }

        private static void GuessNumber(int number)
        {
            int answer;
            
            do
            {
                Console.Write("Try to guess number :");
                var input = Console.ReadLine();

                // - say goodbye
                if (input == StopWord)
                {
                    Console.WriteLine("See you later...");
                    return;
                }

                // - parse int from string;
                // - in case of invalid input skip this iteration
                if (!int.TryParse(input, out answer)) continue;

                // - give a hint
                if (answer < number) Console.WriteLine("Too few!");
                if (answer > number) Console.WriteLine("Too much!");

            } while (answer != number);
            
            Console.WriteLine($"You are winner! It was {number}");
        }
    }
}
