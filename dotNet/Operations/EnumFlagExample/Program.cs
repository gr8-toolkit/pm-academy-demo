using EnumFlagExample.Custom;
using System;
using System.Text;

namespace EnumFlagExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("EnumFlagExample");

            var monday1 = Day.Monday;
            var monday2 = DayFlags.Monday;
            var monday3 = DayFlagInt32.Monday;
            var monday4 = DayFlagBitLiteral.Monday;

            var saturdayInt32M1 = (int)Day.Saturday;                                // 6
            var saturdayInt32M2 = (int)DayFlags.Saturday;                           // 32
            var saturdayInt32M3 = (int)DayFlagInt32.Saturday;                       // 32
            var saturdayInt32M4 = (int)DayFlagBitLiteral.Saturday;                  // 32

            var mondyOrFriday1 = DayFlags.Monday | DayFlags.Friday;

            var checkFridayInWeekdays = (DayFlags.Weekdays & DayFlags.Friday) != 0; // true

            var sb = new StringBuilder();
            sb.AppendLine("All possible Day variants");
            for (int d = 0; d < 10; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (Day)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlags variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlags)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlagInt32 variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlagInt32)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlagBitLiteral variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlagBitLiteral)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayInt32Pow2 variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayInt32Pow2)d, Environment.NewLine);
            }
            var info = sb.ToString();
        }
    }
}
