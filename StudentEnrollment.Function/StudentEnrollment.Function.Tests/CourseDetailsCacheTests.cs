using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using StudentEnrollment.Function.Domain.Repository;

namespace StudentEnrollment.Function.Tests
{
    [TestClass]
    public class CourseDetailsCacheTests
    {
        private readonly ICourseDetailsCache courseDetailsCache;
        
        private readonly Mock<ICourseDetailsRepository> mockCourseDetailsRepository;
        private readonly IMemoryCache mockMemoryCache;

        public CourseDetailsCacheTests()
        {
            this.mockMemoryCache = Create.MockedMemoryCache();
            this.mockCourseDetailsRepository = new Mock<ICourseDetailsRepository>();

            this.mockCourseDetailsRepository.Setup(s => s.Read()).Returns(new ReadCourseDetailsDatabaseResult
            {
                Result = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        CourseId = 1,
                        CourseName = "FromDatabase"
                    }
                }
            });

            this.courseDetailsCache = new CourseDetailsCache(this.mockCourseDetailsRepository.Object, this.mockMemoryCache);
        }

        [TestMethod]
        public void GetAsync_NoCachedValue_ReturnsCourseDetailsResult()
        {
            var result = this.courseDetailsCache.Get();

            result.Result.First().CourseName.ShouldBe("FromDatabase");
        }

        [TestMethod]
        public void GetAsync_NoCachedValue_CallsRepository()
        {
            this.courseDetailsCache.Get();
            this.mockCourseDetailsRepository.Verify(v => v.Read(), Times.Once);
        }

        [TestMethod]
        public void GetAsync_NoCachedValue_CreatesCachedValue()
        {
            this.courseDetailsCache.Get();
            var newCacheValue = mockMemoryCache.Get<ReadCourseDetailsDatabaseResult>("CourseDetails");
            newCacheValue.Result.First().CourseName.ShouldBe("FromDatabase");
        }

        [TestMethod]
        public void GetAsync_CachedValue_DoesNotCallRepository()
        {
            mockMemoryCache.Set("CourseDetails", new ReadCourseDetailsDatabaseResult
            {
                Result = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        CourseId = 1,
                        CourseName = "FromCache"
                    }
                }
            });

            this.courseDetailsCache.Get();

            this.mockCourseDetailsRepository.Verify(v => v.Read(), Times.Never);
        }

        [TestMethod]
        public void GetAsync_CachedValue_ReturnsCourseDetailsResult()
        {
            mockMemoryCache.Set("CourseDetails", new ReadCourseDetailsDatabaseResult
            {
                Result = new List<CourseDetails>
                {
                    new CourseDetails
                    {
                        CourseId = 1,
                        CourseName = "FromCache"
                    }
                }
            });

            var result = this.courseDetailsCache.Get();

            result.Result.First().CourseName.ShouldBe("FromCache");
        }
    }
}
