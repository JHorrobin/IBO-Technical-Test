using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using StudentEnrollment.Function.Domain.Repository;
using System.Data;
using System.Data.SqlClient;

namespace StudentEnrollment.Function.Tests
{
    [TestClass]
    public class CourseDetailsSqlRepositoryTests
    {
        private readonly ICourseDetailsRepository courseDetailsSqlRepository;
        private readonly Mock<ISqlDataContext> mockSqlDataContext;

        public CourseDetailsSqlRepositoryTests()
        {
            var mockRecord = new Mock<IDataRecord>();
            mockRecord.Setup(col => col["CourseId"]).Returns("1234");
            mockRecord.Setup(col => col["CourseName"]).Returns("TestCourse");

            this.mockSqlDataContext = new Mock<ISqlDataContext>();
            this.mockSqlDataContext.Setup(s =>
                s.ExecuteReader(It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>()))
                .Returns(new List<IDataRecord>
                {
                    mockRecord.Object
                });


            this.courseDetailsSqlRepository = new CourseDetailsSqlRepository(this.mockSqlDataContext.Object);
        }

        [TestMethod]
        public void Read_Success_ReturnsSuccessResult()
        {
            var result = this.courseDetailsSqlRepository.Read();

            result.ShouldBeEquivalentTo(new ReadCourseDetailsDatabaseResult
            {
                Result = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        CourseId = 1234,
                        CourseName = "TestCourse"
                    }
                }
            });
        }

        [TestMethod]
        public void Read_ExecuteReaderThrows_ReturnsFailure()
        {
            this.mockSqlDataContext.Setup(s =>
                s.ExecuteReader(It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>()))
                .Throws(new Exception("An error"));

            var result = this.courseDetailsSqlRepository.Read();

            result.Success.ShouldBeFalse();
        }

        [TestMethod]
        public void Read_ExecuteReaderThrows_ReturnsFailureResult()
        {
            this.mockSqlDataContext.Setup(s =>
                s.ExecuteReader(It.IsAny<string>(), It.IsAny<IEnumerable<SqlParameter>>()))
                .Throws(new Exception("An error"));

            var result = this.courseDetailsSqlRepository.Read();

            result.ShouldBeEquivalentTo(new ReadCourseDetailsDatabaseResult
            {
                ErrorMessage = "An error"
            });
        }
    }
}
