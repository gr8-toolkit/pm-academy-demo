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
    public class CreateStudentValidatorTests
    {
        [Fact]
        public async Task Validate_Should_Fail_When_Student_With_Same_Id_Already_Exists()
        {
            var student = new Student(Guid.NewGuid(), "existing", "Student");

            var repo = NSubstitute.Substitute.For<IStudentsRepository>();
            repo.FindById(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(student);

            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);
            var sut = new CreateStudentValidator(unitOfWork);

            var command = new CreateStudent(student.Id, "another", "Student");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.Context == nameof(CreateStudent.StudentId) && e.Message.Contains(student.Id.ToString()));
        }

        [Fact]
        public async Task ValidateAsync_Should_Succeed_When_Command_Valid()
        {
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            var sut = new CreateStudentValidator(unitOfWork);

            var command = new CreateStudent(Guid.NewGuid(), "new", "Student");
            var result = await sut.ValidateAsync(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}