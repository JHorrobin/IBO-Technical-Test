using StudentEnrollment.Function.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Services
{
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        public async Task<StudentEnrollmentResult> EnrollStudents(List<StudentEnrollmentRequest> Enrollments)
        {
            throw new NotImplementedException();
        }
    }
}
