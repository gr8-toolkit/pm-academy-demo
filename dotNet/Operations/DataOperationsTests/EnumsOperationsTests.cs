using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataOperationsTests
{
    public class EnumsOperationsTests
    {
        [Fact]
        public void Test()
        {
            // Act
            Day exampleEnum, exampleEnum2;
            DayFlags flagsExample, mondyOrFridayFlag;
            DayFlagInt32 flagInt32Example;
            DayFlagBitLiteral flagBitLiteralExample;
            bool checkFridayInWeekdays;
            int saturdayInt32M1, saturdayInt32M2, saturdayInt32M3, saturdayInt32M4, mondyOrFridayInt32;
            string mondyOrFridayFlagVm1, mondyOrFridayFlagVm2;

            // Action
            exampleEnum = Day.Monday;
            flagsExample = DayFlags.Monday;
            flagInt32Example = DayFlagInt32.Monday;
            flagBitLiteralExample = DayFlagBitLiteral.Monday;

            saturdayInt32M1 = (int)Day.Saturday;                                // 6
            saturdayInt32M2 = (int)DayFlags.Saturday;                           // 32
            saturdayInt32M3 = (int)DayFlagInt32.Saturday;                       // 32
            saturdayInt32M4 = (int)DayFlagBitLiteral.Saturday;                  // 32
            checkFridayInWeekdays = (DayFlags.Weekdays & DayFlags.Friday) != 0; // true

            mondyOrFridayFlag = DayFlags.Monday | DayFlags.Friday;
            mondyOrFridayInt32 = (int)mondyOrFridayFlag;
            exampleEnum2 = (Day)mondyOrFridayInt32;

            mondyOrFridayFlagVm1 = mondyOrFridayFlag.ToString();
            mondyOrFridayFlagVm2 = exampleEnum2.ToString();

            // Assert
            Assert.NotEqual(Day.Saturday, exampleEnum);
            Assert.Equal(6, saturdayInt32M1);
            Assert.NotEqual(DayFlags.Saturday, flagsExample);
            Assert.Equal(32, saturdayInt32M2);
            Assert.NotEqual(DayFlagInt32.Saturday, flagInt32Example);
            Assert.Equal(32, saturdayInt32M3);
            Assert.NotEqual(DayFlagBitLiteral.Saturday, flagBitLiteralExample);
            Assert.Equal(32, saturdayInt32M4);
            Assert.Equal(DayFlags.Monday | DayFlags.Friday, mondyOrFridayFlag);
            Assert.True(checkFridayInWeekdays);
            Assert.Equal(17, mondyOrFridayInt32);

            Assert.Equal("Monday, Friday", mondyOrFridayFlagVm1);
            Assert.Equal("17", mondyOrFridayFlagVm2);

        }
    }
}
