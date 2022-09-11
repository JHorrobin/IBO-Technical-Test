using Microsoft.Extensions.Logging;
using StudentEnrollment.Function.Domain.Models;
using System;
using System.Linq;

namespace StudentEnrollment.Function.Domain.Repository
{
    public class CourseDetailsSqlRepository : ICourseDetailsRepository
    {
        private const string ReadCourseDetailsProcedure = "ReadCourseDetailsProcedure";

        private readonly ISqlDataContext sqlDataContext;
        private readonly ILogger<CourseDetailsSqlRepository> logger;

        public CourseDetailsSqlRepository(ISqlDataContext sqlDataContext, ILogger<CourseDetailsSqlRepository> logger)
        {
            this.sqlDataContext = sqlDataContext;
            this.logger = logger;
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
                this.logger.LogError(exception, exception.Message);
                return new ReadCourseDetailsDatabaseResult
                {
                    ErrorMessage = exception.Message
                };
            }
        }
    }
}
