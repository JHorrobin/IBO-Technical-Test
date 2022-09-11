using StudentEnrollment.Function.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Repository
{
    public class CourseDetailsSqlRepository : ICourseDetailsRepository
    {
        private const string ReadCourseDetailsProcedure = "ReadCourseDetailsProcedure";

        private readonly ISqlDataContext sqlDataContext;

        public CourseDetailsSqlRepository(ISqlDataContext sqlDataContext)
        {
            this.sqlDataContext = sqlDataContext;
        }

        public ReadCourseDetailsDatabaseResult Read()
        {
            try
            {
                var records = this.sqlDataContext.ExecuteReader(ReadCourseDetailsProcedure);

                var result = records.Select(record => new CourseDetails
                {
                    CourseId = int.Parse(record["CourseId"].ToString()),
                    CourseName = record["CourseName"].ToString(),
                }).ToList();

                return new ReadCourseDetailsDatabaseResult
                {
                    Result = result
                };

            }
            catch (Exception exception)
            {
                return new ReadCourseDetailsDatabaseResult
                {
                    ErrorMessage = exception.Message
                };
            }
        }
    }
}
