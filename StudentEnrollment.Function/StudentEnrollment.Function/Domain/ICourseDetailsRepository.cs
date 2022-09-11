using StudentEnrollment.Function.Domain.Models;

namespace StudentEnrollment.Function.Domain
{
    public interface ICourseDetailsRepository
    {
        public ReadCourseDetailsDatabaseResult Read();
    }
}
