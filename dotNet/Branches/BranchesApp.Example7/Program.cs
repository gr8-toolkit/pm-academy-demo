using System;
using System.Threading.Tasks;

namespace BranchesApp.Example7
{
    class Program
    {
        public enum State
        {
            Invisible,
            Disabled,
            Visible
        }

        public class NetworkException : Exception
        {
            public int StatusCode { get; set; }

            public NetworkException(string message, int code) : base(message)
            {
                StatusCode = code;
            }
        }

        public static State ButtonState { get; set; }

        static void Main()
        {
            TypicalSwitch();
            
            Console.WriteLine(DataMapping("W"));
            
            Commands("STOP", 100d);
            
            Click();

            PrintError(new NetworkException("Oops! Error", 404));
            
            PrintError2(new NetworkException("Oops! Timeout Error", 408));
        }

        private static void TypicalSwitch()
        {
            int month = 3;
            switch (month)
            {
                case 1:
                case 2:
                case 12:
                    Console.WriteLine("Winter");
                    break;
                case 3:
                case 4:
                case 5:
                    Console.WriteLine("Spring");
                    break;
                case 6:
                case 7:
                case 8:
                    Console.WriteLine("Summer");
                    break;
                case 9:
                case 10:
                case 11:
                    Console.WriteLine("Autumn");
                    break;
                default:
                    Console.WriteLine("Invalid month");
                    break;
            }
        }
        
        private static string DataMapping(string direction)
        {
            switch (direction)
            {
                case "N": return "North";
                case "E": return "East";
                case "S": return "South";
                case "W": return "West";
                default: return direction;
            }
        }

        private static void Commands(string command, double speed)
        {
            switch (command)
            {
                case "FASTER": speed += 10; break;
                case "SLOWER": speed -= 10; break;
                case "STOP": speed = 0; break;
                default: throw new ArgumentOutOfRangeException();
            }
            Console.WriteLine(speed);
        }
        
        private static void Click()
        {
            switch (ButtonState)
            {
                case State.Invisible:
                case State.Disabled:
                    break;
                case State.Visible: Console.WriteLine("Click"); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void PrintError(NetworkException exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            switch (exception.StatusCode)
            {
                case 401: Console.WriteLine("Unauthorized"); break;
                case 404: Console.WriteLine("Not found"); break;
                default: Console.WriteLine("Unexpected error"); break;
            }
        }

        private static void PrintError2(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            switch (exception)
            {
                case TaskCanceledException tx:
                case NetworkException ex when ex.StatusCode == 408: 
                    Console.WriteLine("Timeout"); 
                    break;
                default: Console.WriteLine("Unexpected error"); break;
            }
        }
    }
}
