using System;

namespace StudentEnrollment.Function.Domain.Models
{
    public class StudentEnrollmentRequest
    {
        public int StudentId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public int CourseCode { get; set; }
        public ContactDetails StudentContactDetails { get; set; }
    }
}
