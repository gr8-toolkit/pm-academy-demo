using System;
using System.Threading;
using System.Threading.Tasks;
using School.Commands;
using School.Persistence;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace School.Tests.Unit.Commands
{
    public class CreateCourseValidatorTests
    {
        [Fact]
        public async Task Validate_Should_Fail_When_Course_With_Same_Id_Already_Exists()
        {
            var course = new Course(Guid.NewGuid(), "existing course");
            
            var repo = Substitute.For<ICoursesRepository>();
            repo.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(course);

            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(course.Id, "another course");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateCourse.CourseId) && e.Message.Contains(course.Id.ToString()));
        }

        [Fact]
        public async Task Validate_Should_Fail_When_Course_With_Same_Title_Already_Exists()
        {
            var course = new Course(Guid.NewGuid(), "existing course");

            var repo = Substitute.For<ICoursesRepository>();
            repo.FindAsync(null, Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(new[]{course});

            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), course.Title);
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateCourse.CourseTitle) && e.Message.Contains(course.Title));
        }

        [Fact]
        public async Task Validate_Should_Succeed_When_Command_Valid()
        {
            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            var sut = new CreateCourseValidator(unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), "new Course");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
