using System;
using FluentAssertions;
using Xunit;

namespace School.Tests.Unit
{
    public class BaseEntityTests
    {
        [Fact]
        public void Entities_Of_Different_Type_Should_Not_Be_Equal()
        {
            var id = Guid.NewGuid();
            var student = new Student(id, "firstname", "lastname");
            var course = new Course(id, "title");

            (student == course).Should().BeFalse();
            course.Equals(student).Should().BeFalse();
            student.Equals(course).Should().BeFalse();

            (student.GetHashCode() == course.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void Entities_Of_Same_Type_Should_Be_Equal_When_Ids_Match()
        {
            var id = Guid.NewGuid();
            var entityA = new Student(id, "firstname", "lastname");
            var entityB = new Student(id, "myname", "my surname");

            (entityA == entityB).Should().BeTrue();
            entityA.Equals(entityB).Should().BeTrue();
            entityB.Equals(entityA).Should().BeTrue();

            (entityA.GetHashCode() == entityB.GetHashCode()).Should().BeTrue();
        }

        [Fact]
        public void Entities_Of_Same_Type_Should_Not_Be_Equal_When_Ids_Different()
        {
            var entityA = new Student(Guid.NewGuid(), "firstname", "lastname");
            var entityB = new Student(Guid.NewGuid(), "myname", "my surname");

            (entityA == entityB).Should().BeFalse();
            entityA.Equals(entityB).Should().BeFalse();
            entityB.Equals(entityA).Should().BeFalse();

            (entityA.GetHashCode() == entityB.GetHashCode()).Should().BeFalse();
        }
    }
}