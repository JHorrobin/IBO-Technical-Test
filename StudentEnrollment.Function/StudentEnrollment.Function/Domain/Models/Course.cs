using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<Student> Students { get; set; }
    }
}
