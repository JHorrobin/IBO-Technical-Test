using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StudentEnrollment.Function.Domain;
using StudentEnrollment.Function.Domain.Models;
using StudentEnrollment.Function.Domain.Repository;
using StudentEnrollment.Function.Domain.Services;
using StudentEnrollment.Function.Domain.Validation;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(StudentEnrollment.Function.Startup))]
namespace StudentEnrollment.Function
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddTransient<IStudentEnrollmentService, StudentEnrollmentService>()
                .AddTransient<ICoursesRepository, CourseSqlRepository>()
                .AddTransient<ICourseDetailsRepository, CourseDetailsSqlRepository>()
                .AddTransient<ICourseDetailsCache, CourseDetailsCache>()
                .AddTransient<ISqlDataContext, SqlDataContext>()
                .AddScoped<IValidator<StudentEnrollmentRequest>, StudentEnrollmentRequestValidator>();
        }
    }
}
