using System;
using System.Threading;
using System.Threading.Tasks;
using School.Persistence;

namespace School.Commands
{
    public class Enroll : MediatR.INotification
    {
        public Enroll(Guid courseId, Guid studentId)
        {
            if (Guid.Empty == courseId)
                throw new ArgumentOutOfRangeException(nameof(courseId));
            if (Guid.Empty == studentId)
                throw new ArgumentOutOfRangeException(nameof(studentId));
            CourseId = courseId;
            StudentId = studentId;
        }

        public Guid CourseId { get; }
        public Guid StudentId { get; }
    }

    public class EnrollValidator : BaseValidator<Enroll>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;

        public EnrollValidator(ISchoolUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunAsync(Enroll command, ValidationResult result, CancellationToken cancellationToken)
        {
            var courseById = await _unitOfWork.CoursesRepository.FindByIdAsync(command.CourseId, cancellationToken);
            if (courseById is null)
                result.AddError(nameof(Enroll.CourseId), $"invalid course id {command.CourseId}");

            var studentById = await _unitOfWork.StudentsRepository.FindById(command.StudentId, cancellationToken);
            if (studentById is null)
                result.AddError(nameof(Enroll.StudentId), $"invalid student id {command.StudentId}");
        }
    }

    public class EnrollHandler : BaseCommandHandler<Enroll>
    {
        private readonly ISchoolUnitOfWork _unitOfWork;
        public EnrollHandler(IValidator<Enroll> validator, ISchoolUnitOfWork unitOfWork) : base(validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task RunCommand(Enroll command, CancellationToken cancellationToken)
        {
            var course = await _unitOfWork.CoursesRepository.FindByIdAsync(command.CourseId, cancellationToken); 
            var student = await _unitOfWork.StudentsRepository.FindById(command.StudentId, cancellationToken);

            student.Add(course);

            await _unitOfWork.Commit(cancellationToken);
        }
    }
}