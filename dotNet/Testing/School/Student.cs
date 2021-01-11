using School.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace School
{
    public class Student : BaseAggregateRoot<Guid>
    {
        public Student(Guid id, string firstname, string lastname)
        {
            Id = id;
            ChangeFirstName(firstname);
            ChangeLastName(lastname);
        }

        public string FirstName { get; private set; }

        public void ChangeFirstName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            FirstName = value;
        }

        public string LastName { get; private set; }
        
        public void ChangeLastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            LastName = value;
        }

        private readonly List<StudentCourseStatus> _courses = new List<StudentCourseStatus>();
        public IReadOnlyCollection<StudentCourseStatus> Courses => _courses.ToImmutableArray();

        public void Add(Course course)
        {
            if(null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();

            var isEmpty = !oldCourses.Any();
            var hasWithdrawn = !isEmpty && oldCourses.OrderByDescending(c => c.CreatedAt).First().Status == StudentCourseStatus.Statuses.Withdrawn;

            if (isEmpty || hasWithdrawn)
            {
                _courses.Add(StudentCourseStatus.Enrolled(this, course));
                AddEvent(new StudentEnrolled(this, course));
            }
        }

        public void Cancel(Course course)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();

            var isEmpty = !oldCourses.Any();

            var isCompleted = !isEmpty && oldCourses.OrderByDescending(c => c.CreatedAt).First().Status == StudentCourseStatus.Statuses.Completed;
            if(isCompleted)
                throw new ArgumentException($"student {this.Id} has completed course {course.Id} already");

            var isEnrolled = !isEmpty && oldCourses.OrderByDescending(c => c.CreatedAt).First().Status == StudentCourseStatus.Statuses.Enrolled;
            if (!isEnrolled)
                throw new ArgumentException($"student {this.Id} not enrolled in course {course.Id}");

            _courses.Add(StudentCourseStatus.Withdrawn(this, course));
            AddEvent(new StudentWithdrawn(this, course));
        }

        public void Complete(Course course)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();
            if (!oldCourses.Any())
                throw new ArgumentException($"Student {Id} not enrolled in course {course.Id}");
            if(oldCourses.Any(c => c.Status == StudentCourseStatus.Statuses.Withdrawn))
                throw new ArgumentException($"Student {Id} has withdrawn from course {course.Id}");

            _courses.Add(StudentCourseStatus.Completed(this, course));
            AddEvent(new CourseCompleted(this, course));
        }
    }
}