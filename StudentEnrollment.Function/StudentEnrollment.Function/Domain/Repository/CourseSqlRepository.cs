using StudentEnrollment.Function.Domain.Models;
using System;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Repository
{
    internal class CourseSqlRepository : ICoursesRepository
    {
        public Task<bool> Create()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<Course> Read(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
