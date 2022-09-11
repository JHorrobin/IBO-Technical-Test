using System;

namespace StudentEnrollment.Function.Domain.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public ContactDetails ContactDetails { get; set; }
    }
}
