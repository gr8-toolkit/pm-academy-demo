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
    public class WithdrawValidatorTests
    {
        [Fact]
        public async Task Validate_Should_Fail_When_Student_Does_Not_Exists()
        {
            var repo = Substitute.For<IStudentsRepository>();
            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(Guid.NewGuid(), Guid.NewGuid());
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(Withdraw.StudentId) && e.Message.Contains(command.StudentId.ToString()));
        }

        [Fact]
        public async Task Validate_Should_Fail_When_Course_Does_Not_Exists()
        {
            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(Guid.NewGuid(), Guid.NewGuid());
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(Withdraw.CourseId) && e.Message.Contains(command.CourseId.ToString()));
        }

        [Fact]
        public async Task Validate_Should_Succeed_When_Command_Valid()
        {
            var student = new Student(Guid.NewGuid(), "existing", "Student");

            var studentsRepository = NSubstitute.Substitute.For<IStudentsRepository>();
            studentsRepository.FindById(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(student);

            var course = new Course(Guid.NewGuid(), "existing course");

            var coursesRepository = Substitute.For<ICoursesRepository>();
            coursesRepository.FindByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(course);

            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(studentsRepository);
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(coursesRepository);
            var sut = new WithdrawValidator(unitOfWork);

            var command = new Withdraw(course.Id, student.Id);
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
