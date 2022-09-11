using StudentEnrollment.Function.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain
{
    public interface IStudentEnrollmentService
    {
        public Task<StudentEnrollmentResult> EnrollStudentsAsync(List<StudentEnrollmentRequest> enrollments);
    }
}
