using System.Collections.Generic;

namespace StudentEnrollment.Function.Domain.Models
{
    public class StudentEnrollmentResult
    {
        public bool Success { get { return this.ErrorMessage == null; } }

        public string ErrorMessage { get; set; }

        public List<Course> Result { get; set; }
    }
}
