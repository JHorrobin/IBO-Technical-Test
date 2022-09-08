using System;

namespace StudentEnrollment.Function.Domain.Models
{
    public class StudentEnrollment
    {
        public string Forname { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public int CourseCode { get; set; }
        public ContactDetails StudentContactDetails { get; set; }
    }
}
