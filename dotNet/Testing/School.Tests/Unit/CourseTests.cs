using System;
using FluentAssertions;
using Xunit;

namespace School.Tests.Unit
{
    public class CourseTests
    {
        [Fact]
        public void SetTitle_Should_Fail_If_Value_Invalid()
        {
            var sut = new Course(Guid.NewGuid(), "title");
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(null));
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(""));
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(" "));
        }

        [Fact]
        public void SetTitle_Should_Set_Value_When_Valid()
        {
            var expected = "new title";
            var sut = new Course(Guid.NewGuid(), "title");
            sut.SetTitle(expected);
            sut.Title.Should().Be(expected);
        }
    }
}