using System;
using MediatR;

namespace School.Queries
{

    public class CourseById : IRequest<CourseDetails>
    {
        public CourseById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }


    public class CourseDetails
    {
        public CourseDetails(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; }
        public string Title { get; }
    }
}
