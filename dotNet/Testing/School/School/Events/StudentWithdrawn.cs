using System;

namespace School.Events
{
    public class StudentWithdrawn : IDomainEvent
    {
        public StudentWithdrawn(Student student, Course course)
        {
            Student = student ?? throw new ArgumentNullException(nameof(student));
            Course = course ?? throw new ArgumentNullException(nameof(course));
        }
        public Student Student { get; }
        public Course Course { get; }
    }
}
