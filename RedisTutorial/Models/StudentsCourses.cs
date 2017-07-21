using System;
using System.Collections.Generic;

namespace RedisTutorial.Models
{
    public partial class StudentsCourses
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Students Student { get; set; }
    }
}
