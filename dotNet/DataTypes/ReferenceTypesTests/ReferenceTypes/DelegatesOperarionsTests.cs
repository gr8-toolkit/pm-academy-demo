using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataTypesTests.ReferenceTypes
{
    /// <summary>
    /// Delegate in action.
    /// </summary>
    public class DelegatesOperarionsTests
    {
        delegate void DelegateExample();
        delegate void DelegateExampleWithParam(string name);

        [Fact]
        public void Activate_Code()
        {
            // Act
            DelegateExample example1 = Hello;
            DelegateExampleWithParam example2 = null;
            DelegateExample example3 = null;
            //DelegateExample example4 = HelloUser;  not allowed operation

            // Action
            var example1Exception = Record.Exception(() => example1.Invoke());
            var example2Exception = Record.Exception(() => example2?.Invoke("Ping pong"));

            // Assert
            Assert.Null(example1Exception);
            Assert.Null(example2Exception);
            Assert.Throws<NullReferenceException>(() => example3.Invoke());

        }

        static void Hello()
        {
            Console.WriteLine("Hello");
        }

        static void HelloUser(string userName)
        {
            Console.WriteLine($"Hello, {userName}");
        }

    }
}
