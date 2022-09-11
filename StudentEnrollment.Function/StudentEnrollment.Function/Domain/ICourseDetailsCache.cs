using StudentEnrollment.Function.Domain.Models;

namespace StudentEnrollment.Function.Domain
{
    public interface ICourseDetailsCache
    {
        ReadCourseDetailsDatabaseResult Get();
    }
}
