using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using StudentEnrollment.Function.Domain.Repository;
using System.Data.SqlClient;

namespace StudentEnrollment.Function.Tests
{
    [TestClass]
    public class CourseSqlRepositoryTests
    {
        private readonly ICoursesRepository coursesRepository;
        private readonly Mock<ISqlDataContext> mockSqlDataContext;
        private readonly Mock<ILogger<CourseSqlRepository>> mockLogger;

        private readonly List<Course> mockData = new List<Course>
        {
            new Course
            {
                CourseId = 1,
                Name = "TestCourse",
                StartDate = DateTime.MinValue,
                Students = new List<Student>
                {
                    new Student
                    {
                        StudentId = 1234,
                        Forename = "Test",
                        Surname = "Student",
                        BirthDate = DateTime.MinValue,
                        ContactDetails = new ContactDetails
                        {
                            Address = "TestAddress",
                            Phone = "Phone",
                            Mobile = "Mobile",
                            Email = "Email",
                            PreferredContactMethod = PreferredContactMethod.None
                        }
                    }
                }
            }
        };

        public CourseSqlRepositoryTests()
        {
            this.mockSqlDataContext = new Mock<ISqlDataContext>();
            this.mockLogger = new Mock<ILogger<CourseSqlRepository>>();
            this.coursesRepository = new CourseSqlRepository(this.mockSqlDataContext.Object, this.mockLogger.Object);
        }

        [TestMethod]
        public async Task Update_Success_ReturnsSuccessResult()
        {
            var result = await this.coursesRepository.UpdateAsync(this.mockData);
            result.Success.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Update_ExecutesCourseUpdateQuery()
        {
            await this.coursesRepository.UpdateAsync(this.mockData);
            
            this.mockSqlDataContext.Verify(v =>
                v.ExecuteQueryAsync("UpdateCoursesProcedure", It.IsAny<IEnumerable<SqlParameter>>()), 
                Times.Once);
        }

        [TestMethod]
        public async Task Update_ExecutesStudentUpdateQuery()
        {
            await this.coursesRepository.UpdateAsync(this.mockData);

            this.mockSqlDataContext.Verify(v =>
                v.ExecuteQueryAsync("UpdateStudentProcedure", It.IsAny<IEnumerable<SqlParameter>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task Update_CourseUpdateThrows_ReturnsFailure()
        {
            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateCoursesProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            var result = await this.coursesRepository.UpdateAsync(this.mockData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task Update_CourseUpdateThrows_ReturnsCorrectFailureMessage()
        {
            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateCoursesProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            var result = await this.coursesRepository.UpdateAsync(this.mockData);

            result.ErrorMessage.ShouldBe("Failed to update course 1: Database Error\r\n");
        }

        [TestMethod]
        public async Task Update_CourseUpdateFails_DoesNotExecuteStudentUpdateQuery()
        {
            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateCoursesProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            await this.coursesRepository.UpdateAsync(this.mockData);

            this.mockSqlDataContext.Verify(v =>
                v.ExecuteQueryAsync("UpdateStudentProcedure", It.IsAny<IEnumerable<SqlParameter>>()),
                Times.Never);
        }

        [TestMethod]
        public async Task Update_StudentUpdateFails_ReturnsFailure()
        {
            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateStudentProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            var result = await this.coursesRepository.UpdateAsync(this.mockData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task Update_StudentUpdateFails_ReturnsCorrectFailureMessage()
        {
            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateStudentProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            var result = await this.coursesRepository.UpdateAsync(this.mockData);

            result.ErrorMessage.ShouldBe("\tFailed to update student 1234: Database Error\r\n");
        }

        [TestMethod]
        public async Task Update_StudentUpdateFailsMultipleTimes_ReturnsCorrectFailureMessage()
        {
            this.mockData[0].Students.Add(new Student
            {
                StudentId = 4567,
                Forename = "Test2",
                Surname = "Student2",
                BirthDate = DateTime.MinValue,
                ContactDetails = new ContactDetails
                {
                    Address = "TestAddress2",
                    Phone = "Phone2",
                    Mobile = "Mobile2",
                    Email = "Email2",
                    PreferredContactMethod = PreferredContactMethod.None
                }
            });

            this.mockSqlDataContext.Setup(s => s.ExecuteQueryAsync(
                "UpdateStudentProcedure", It.IsAny<IEnumerable<SqlParameter>>()))
                .ThrowsAsync(new Exception("Database Error"));

            var result = await this.coursesRepository.UpdateAsync(this.mockData);

            result.ErrorMessage.ShouldBe("\tFailed to update student 1234: Database Error\r\n\tFailed to update student 4567: Database Error\r\n");
        }
    }
}
