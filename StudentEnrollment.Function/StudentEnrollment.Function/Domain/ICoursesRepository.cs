using StudentEnrollment.Function.Domain.Models;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain
{
    public interface ICoursesRepository
    {
        Task<Course> Read(int courseId);
        Task<bool> Create();
        Task<bool> Update();
        Task<bool> Delete();
    }
}
