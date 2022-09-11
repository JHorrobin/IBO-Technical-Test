using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using StudentEnrollment.Function.Domain.Services;

namespace StudentEnrollment.Function.Tests
{
    [TestClass]
    public class StudentEnrollmentServiceTests
    {
        private readonly IStudentEnrollmentService studentEnrollmentService;

        private readonly Mock<ICoursesRepository> mockCoursesRepository;
        private readonly Mock<ICourseDetailsCache> mockCourseDetailsCache;
        private readonly Mock<ILogger<StudentEnrollmentService>> mockLogger;

        private readonly List<StudentEnrollmentRequest> TestData = new List<StudentEnrollmentRequest>
            {
                new StudentEnrollmentRequest
                {
                    StudentId = 1234,
                    Forename = "Tester",
                    Surname = "Testerson",
                    Birthdate = DateTime.MinValue,
                    CourseCode = 1,
                    StudentContactDetails = new ContactDetails
                    {
                        Address = "TestAddress",
                        Email = "Test@Test.com",
                        Mobile = "12345678901",
                        Phone = "12345678901",
                        PreferredContactMethod = PreferredContactMethod.None
                    }
                }
            };

        public StudentEnrollmentServiceTests()
        {
            this.mockCourseDetailsCache = new Mock<ICourseDetailsCache>();

            this.mockCourseDetailsCache.Setup(s => s.Get())
                .Returns(new ReadCourseDetailsDatabaseResult
                {
                    Result = new List<CourseDetails>
                    {
                        new CourseDetails
                        {
                            CourseId = 1,
                            CourseName = "Test Course"
                        }
                    }
                });

            this.mockCoursesRepository = new Mock<ICoursesRepository>();

            this.mockCoursesRepository.Setup(s => s.UpdateAsync(It.IsAny<IEnumerable<Course>>()))
                .ReturnsAsync(new DatabaseResult());

            this.mockLogger = new Mock<ILogger<StudentEnrollmentService>>();

            this.studentEnrollmentService = new StudentEnrollmentService(
                this.mockCoursesRepository.Object, 
                this.mockCourseDetailsCache.Object,
                this.mockLogger.Object);
        }

        [TestMethod]
        public async Task EnrollStudents_Success_ReturnsSuccessResult()
        {
            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);
            result.Success.ShouldBeTrue();
        }

        [TestMethod]
        public async Task EnrollStudents_CallsTheCourseDetailsCache()
        {
            await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            this.mockCourseDetailsCache.Verify(v => v.Get(), Times.Once);
        }

        [TestMethod]
        public async Task EnrollStudents_CallsUpdateOnTheCoursesRepository()
        {
            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            this.mockCoursesRepository.Verify(v => v.UpdateAsync(It.IsAny<IEnumerable<Course>>()), Times.Once);
        }

        [TestMethod]
        public async Task EnrollStudents_Failure_ReturnsFailureResult()
        {
            this.mockCoursesRepository.Setup(s => s.UpdateAsync(It.IsAny<IEnumerable<Course>>()))
                .ReturnsAsync(new DatabaseResult
                {
                    ErrorMessage = "Some Database Error."
                });

            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task EnrollStudents_CourseRepositoryThrows_ReturnsFailureResult()
        {
            this.mockCoursesRepository.Setup(s => s.UpdateAsync(It.IsAny<IEnumerable<Course>>()))
                .ThrowsAsync(new Exception("Some Exception From Database Layer."));

            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task EnrollStudents_CacheReturnFailure_ReturnsFailureResult()
        {
            this.mockCourseDetailsCache.Setup(s => s.Get())
                .Returns(new ReadCourseDetailsDatabaseResult
                {
                    ErrorMessage = "Some Cache Error"
                });

            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task EnrollStudents_CourseIdNotInCacheList_ReturnsFailureResult()
        {
            this.mockCourseDetailsCache.Setup(s => s.Get())
                .Returns(new ReadCourseDetailsDatabaseResult
                {
                    Result = new List<CourseDetails>()
                });

            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public async Task EnrollStudents_CacheReturnFailure_DoesNotCallDatabaseLayer()
        {
            this.mockCourseDetailsCache.Setup(s => s.Get())
                .Returns(new ReadCourseDetailsDatabaseResult
                {
                    ErrorMessage = "Some Cache Error"
                });

            var result = await this.studentEnrollmentService.EnrollStudentsAsync(this.TestData);

            this.mockCoursesRepository.Verify(v => v.UpdateAsync(It.IsAny<IEnumerable<Course>>()), Times.Never);
        }
    }
}
