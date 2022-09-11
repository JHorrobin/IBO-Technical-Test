using StudentEnrollment.Function.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Services
{
    public class StudentEnrollmentService : IStudentEnrollmentService
    {
        private readonly ICoursesRepository coursesRepository;
        private readonly ICourseDetailsCache courseDetailsCache;

        public StudentEnrollmentService(ICoursesRepository coursesRepository, ICourseDetailsCache courseDetailsCache)
        {
            this.coursesRepository = coursesRepository;
            this.courseDetailsCache = courseDetailsCache;
        }

        public async Task<StudentEnrollmentResult> EnrollStudentsAsync(List<StudentEnrollmentRequest> enrollments)
        {
            try
            {
                var getCoursesResult = courseDetailsCache.Get();

                if (!getCoursesResult.Success)
                {
                    return new StudentEnrollmentResult
                    {
                        ErrorMessage = getCoursesResult.ErrorMessage
                    };
                }

                var courses = getCoursesResult.Result;

                var courseEnrollment = enrollments.GroupBy(e => e.CourseCode,
                    s => new Student
                    {
                        StudentId = s.StudentId,
                        Forename = s.Forename,
                        Surname = s.Surname,
                        BirthDate = s.Birthdate,
                        ContactDetails = s.StudentContactDetails
                    })
                    .Select(x => new Course
                    {
                        CourseId = x.Key,
                        Name = courses.FirstOrDefault(c => c.CourseId == x.Key).CourseName 
                            ?? throw new Exception($"Course with Id:{x.Key} not found."),
                        StartDate = DateTime.UtcNow,
                        Students = x.ToList()
                    });

                var result = await this.coursesRepository.UpdateAsync(courseEnrollment);

                return result.Success ?
                    new StudentEnrollmentResult 
                    { 
                        Result = courseEnrollment.ToList() 
                    } :
                    new StudentEnrollmentResult 
                    { 
                        ErrorMessage = "An error has occured whilst saving the data" 
                    };
            }
            catch
            {
                return new StudentEnrollmentResult
                {
                    ErrorMessage = "An error has occured whilst saving the data"
                };
            }
        }
    }
}
