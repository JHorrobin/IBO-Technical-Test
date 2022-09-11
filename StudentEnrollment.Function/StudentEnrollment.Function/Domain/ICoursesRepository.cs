using StudentEnrollment.Function.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain
{
    public interface ICoursesRepository
    {
        Task<DatabaseResult> UpdateAsync(IEnumerable<Course> courses);
    }
}
