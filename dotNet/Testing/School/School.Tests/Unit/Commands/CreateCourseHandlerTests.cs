using System;
using System.Threading;
using System.Threading.Tasks;
using School.Commands;
using School.Persistence;
using NSubstitute;
using Xunit;

namespace School.Tests.Unit.Commands
{
    public class CreateCourseHandlerTests
    {
        [Fact]
        public async Task Handle_should_create_entity()
        {
            var repo = NSubstitute.Substitute.For<ICoursesRepository>();
            
            var unitOfWork = NSubstitute.Substitute.For<ISchoolUnitOfWork>();
            unitOfWork.CoursesRepository.ReturnsForAnyArgs(repo);
            
            var validator = NSubstitute.Substitute.For<IValidator<CreateCourse>>();
            validator.ValidateAsync(null, CancellationToken.None)
                .ReturnsForAnyArgs(ValidationResult.Successful);

            var sut = new CreateCourseHandler(validator, unitOfWork);

            var command = new CreateCourse(Guid.NewGuid(), "new course");
            await sut.Handle(command, CancellationToken.None);

            await repo.Received(1).CreateAsync(Arg.Is((Course c) => c.Id == command.CourseId && c.Title == command.CourseTitle), Arg.Any<CancellationToken>());
            await unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}