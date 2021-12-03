using System;
using System.Collections.Generic;

namespace BranchesApp.Example11
{
    class Program
    {
        static void Main()
        {
            MonthsMapping(3);
            SeasonMapping(3);
        }

        private static void MonthsMapping(int monthNumber)
        {
            string[] months = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};

            var month = months[monthNumber - 1];
            Console.WriteLine("{0} is {1}", month, monthNumber);
        }


        private static void SeasonMapping(int monthNumber)
        {
            Dictionary<int, string> months = new Dictionary<int, string>
            {
                [1] = "Winter", [2] = "Winter", [12] = "Winter",
                [3] = "Spring", [4] = "Spring", [5] = "Spring",
                [6] = "Summer", [7] = "Summer", [8] = "Summer",
                [9] = "Autumn", [10] = "Autumn", [11] = "Autumn"
            };
            var season = months[monthNumber];
            Console.WriteLine("{0} month is {1}", monthNumber, season);
        }
    }
}
