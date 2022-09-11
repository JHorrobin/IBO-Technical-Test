using Microsoft.Extensions.Logging;
using StudentEnrollment.Function.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Repository
{
    public class CourseSqlRepository : ICoursesRepository
    {
        private const string UpdateCoursesProcedure = "UpdateCoursesProcedure";
        private const string UpdateStudentProcedure = "UpdateStudentProcedure";

        private readonly ISqlDataContext sqlDataContext;
        private readonly ILogger<CourseSqlRepository> logger;

        public CourseSqlRepository(ISqlDataContext sqlDataContext, ILogger<CourseSqlRepository> logger)
        {
            this.sqlDataContext = sqlDataContext;
            this.logger = logger;
        }

        public async Task<DatabaseResult> UpdateAsync(IEnumerable<Course> courses)
        {
            var errorStringBuilder = new StringBuilder();

            foreach (var course in courses)
            {
                var result = await this.UpdateCourse(course, errorStringBuilder);

                if (result.Success == false)
                {
                    errorStringBuilder.AppendLine($"Failed to update course {course.CourseId}: {result.ErrorMessage}");
                }
            }

            var errors = errorStringBuilder.ToString();

            return errors == string.Empty ?
                new DatabaseResult { } :
                new DatabaseResult { ErrorMessage = errors };
        }

        private async Task<DatabaseResult> UpdateCourse(Course course, StringBuilder errorsStringBuilder)
        {
            try
            {
                await this.sqlDataContext.ExecuteQueryAsync(
                    UpdateCoursesProcedure,
                    new[]
                    {
                        new SqlParameter("@CourseId", course.CourseId),
                        new SqlParameter("@StartDate", course.StartDate)
                    });

                foreach (var student in course.Students)
                {
                    var result = await this.UpdateStudent(course.CourseId, student);

                    if (result.Success == false)
                    {
                        errorsStringBuilder.AppendLine($"\tFailed to update student {student.StudentId}: {result.ErrorMessage}");
                    }
                }

                return new DatabaseResult { };
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return new DatabaseResult { ErrorMessage = exception.Message };
            }
        }

        private async Task<DatabaseResult> UpdateStudent(int courseId, Student student)
        {
            try
            {
                await this.sqlDataContext.ExecuteQueryAsync(
                    UpdateStudentProcedure,
                    new[]
                    {
                        new SqlParameter("@CourseId", courseId),
                        new SqlParameter("@StudentId", student.StudentId),
                        new SqlParameter("@ForeName", student.Forename),
                        new SqlParameter("@Surname", student.Surname),
                        new SqlParameter("@BirthDate", student.BirthDate),
                        new SqlParameter("@Address", student.ContactDetails.Address),
                        new SqlParameter("@Email", student.ContactDetails.Email),
                        new SqlParameter("@Phone", student.ContactDetails.Phone),
                        new SqlParameter("@Mobile", student.ContactDetails.Mobile),
                        new SqlParameter("@PreferredContactMethod", (int)student.ContactDetails.PreferredContactMethod),
                    });

                return new DatabaseResult { };
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, exception.Message);
                return new DatabaseResult { ErrorMessage = exception.Message };
            }
        }
    }
}
