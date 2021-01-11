using System;

namespace School.Events
{
    public class StudentEnrolled : IDomainEvent
    {
        public StudentEnrolled(Student student, Course course)
        {
            Student = student ?? throw new ArgumentNullException(nameof(student));
            Course = course ?? throw new ArgumentNullException(nameof(course));
        }
        public Student Student { get; }
        public Course Course { get; }
    }
}
