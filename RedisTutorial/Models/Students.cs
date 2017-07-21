using System;
using System.Collections.Generic;

namespace RedisTutorial.Models
{
    public partial class Students
    {
        public Students()
        {
            StudentsCourses = new HashSet<StudentsCourses>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreationDate { get; set; }
        public bool State { get; set; }

        public virtual ICollection<StudentsCourses> StudentsCourses { get; set; }
    }
}
