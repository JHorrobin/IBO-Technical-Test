using Microsoft.Extensions.Caching.Memory;
using StudentEnrollment.Function.Domain.Models;
using System;

namespace StudentEnrollment.Function.Domain.Repository
{
    public class CourseDetailsCache : ICourseDetailsCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICourseDetailsRepository courseDetailsRepository;
        private readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(60);
        private const string CacheKey = "CourseDetails";

        public CourseDetailsCache(ICourseDetailsRepository courseDetailsRepository, IMemoryCache memoryCache)
        {
            this.courseDetailsRepository = courseDetailsRepository;
            this.memoryCache = memoryCache;
        }

        public ReadCourseDetailsDatabaseResult Get()
        {
            return this.memoryCache.GetOrCreate(CacheKey, (cache) =>
            {
                cache.SlidingExpiration = this.CacheExpiration;
                return this.courseDetailsRepository.Read();
            });
        }
    }
}
