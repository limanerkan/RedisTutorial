using System;
using System.Collections.Generic;

namespace RedisTutorial.Models
{
    public partial class Courses
    {
        public Courses()
        {
            StudentsCourses = new HashSet<StudentsCourses>();
        }

        public int Id { get; set; }
        public string CourseName { get; set; }
        public bool State { get; set; }

        public virtual ICollection<StudentsCourses> StudentsCourses { get; set; }
    }
}
