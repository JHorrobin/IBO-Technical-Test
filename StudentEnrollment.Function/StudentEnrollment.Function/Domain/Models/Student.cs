using System;

namespace StudentEnrollment.Function.Domain.Models
{
    public class Student
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ContactDetails ContactDetails { get; set; }
    }
}
