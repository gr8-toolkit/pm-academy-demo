using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExceptionsTests
{
    public class TryPatternTests
    {
        [Fact]
        public void Parse_String()
        {
            // Act
            string example = "234g";
            int result = default;
            bool success = false;

            // Action

            // Assert
            Assert.Throws<FormatException>(() =>
            {
                result = int.Parse(example);
                success = result != default;
            });
            Assert.False(success);
        }

        [Fact]
        public void TryParse_String()
        {
            // Act
            string example = default;
            int result = default;
            bool success = false;

            // Action
            success = int.TryParse(example, out result);

            // Assert
            Assert.Equal(default, result);
            Assert.False(success);
        }


        [Fact]
        public void TryHandle_Worker()
        {
            // Act
            SomeWorker worker;
            string result;
            bool success;

            // Action
            worker = new SomeWorker();
            success = worker.TryExecute(out result);

            // Assert
            Assert.NotNull(result);
            Assert.False(success);
        }

    }
}
