using Common.ReferenceTypes;
using System;
using Xunit;

namespace DataTypesTests.ReferenceTypes
{
    /// <summary>
    /// Reference types in action.
    /// </summary>
    public class ClassOperationsTests
    {
        private const string InitialStr = "initial";
        private const int InitialNumb = 42;
        private const string ResultStr = "result";
        private const int ResultNumb = 1;

        [Fact]
        public void Change_Reference_Types()
        {
            // Act
            var example1 = new ClassExample { Foo = InitialStr, Bar = InitialNumb };
            var example2 = example1;
            var example3 = new ClassExample { Foo = InitialStr, Bar = InitialNumb };
            var example4 = new ClassExample();

            // Action
            example1.Foo = ResultStr;
            example1.Bar = ResultNumb;
            ModifyRefType(example3);

            // Assert
            Assert.NotNull(example1);
            Assert.NotNull(example2);
            Assert.NotNull(example3);
            Assert.NotNull(example4);

            // values are equal (include strings)
            Assert.Equal(ResultStr, example1.Foo);
            Assert.Equal(ResultNumb, example1.Bar);

            Assert.Equal(ResultStr, example2.Foo);
            Assert.Equal(ResultNumb, example2.Bar);

            Assert.Equal(ResultStr, example3.Foo);
            Assert.Equal(ResultNumb, example3.Bar);

            Assert.Equal(ResultStr, example3.Foo);
            Assert.Equal(ResultNumb, example3.Bar);

            Assert.Equal(default, example4.Foo);
            Assert.Equal(default, example4.Bar);

            Assert.Equal(example1, example2);
            // references are not equal
            Assert.NotEqual(example1, example3);
        }

        [Fact]
        public void Pass_Reference_Types()
        {
            // Act
            var example1 = new ClassExample { Foo = InitialStr, Bar = InitialNumb };
            var example2 = example1;
            ClassExample example3 = null;
            ClassExample example4 = example3;

            // Action
            ModifyRefType(example1);
            OverrideRefType(example1);
            OverrideRefRefType(ref example3);
            ModifyRefType(example3);

            // Assert
            Assert.NotNull(example1);
            Assert.NotNull(example2);
            Assert.NotNull(example3);
            Assert.Null(example4); // example4 still referencing to NULL

            // values are equal (include strings)
            Assert.NotEqual(default, example1.Foo);
            Assert.NotEqual(default, example1.Bar);

            Assert.Equal(ResultStr, example2.Foo);
            Assert.Equal(ResultNumb, example2.Bar);

            Assert.Equal(ResultStr, example3.Foo);
            Assert.Equal(ResultNumb, example3.Bar);
        }

        private static void ModifyRefType(ClassExample example)
        {
            // overrides values
            example.Foo = ResultStr;
            example.Bar = ResultNumb;
        }

        private static void OverrideRefType(ClassExample example)
        {
            // creates new object locally inside method frame
            // dot't overrides original reference
            example = new ClassExample();
        }

        private static void OverrideRefRefType(ref ClassExample example)
        {
            // overrides original reference
            example = new ClassExample();
        }
    }
}
