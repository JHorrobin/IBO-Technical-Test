using System.Collections.Generic;

namespace StudentEnrollment.Function.Domain.Models
{
    public class ReadCourseDetailsDatabaseResult : DatabaseResult
    {
        public IEnumerable<CourseDetails> Result { get; set; }
    }
}
