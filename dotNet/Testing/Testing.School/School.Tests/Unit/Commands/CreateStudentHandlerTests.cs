using System;
using System.Threading;
using System.Threading.Tasks;
using School.Commands;
using School.Persistence;
using NSubstitute;
using Xunit;

namespace School.Tests.Unit.Commands
{
    public class CreateStudentHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Entity()
        {
            var repo = Substitute.For<IStudentsRepository>();

            var unitOfWork = Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.StudentsRepository.ReturnsForAnyArgs(repo);

            var validator = Substitute.For<IValidator<CreateStudent>>();
            validator.ValidateAsync(null, CancellationToken.None)
                .ReturnsForAnyArgs(ValidationResult.Successful);

            var sut = new CreateStudentHandler(validator, unitOfWork);

            var command = new CreateStudent(Guid.NewGuid(), "new","student");
            await sut.Handle(command, CancellationToken.None);

            await repo.Received(1).Create(Arg.Is((Student c) => c.Id == command.StudentId && c.FirstName == command.StudentFirstname && c.LastName == command.StudentLastname), Arg.Any<CancellationToken>());
            await unitOfWork.Received(1).Commit(Arg.Any<CancellationToken>());
        }
    }
}