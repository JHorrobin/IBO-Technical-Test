using FluentValidation;
using StudentEnrollment.Function.Domain.Models;

namespace StudentEnrollment.Function.Domain.Validation
{
    public class StudentEnrollmentRequestValidator : AbstractValidator<StudentEnrollmentRequest>
    {
        public StudentEnrollmentRequestValidator()
        {
            RuleFor(x => x.StudentId).NotNull();
            RuleFor(x => x.Forename).NotEmpty().Length(1, 200);
            RuleFor(x => x.Surname).NotEmpty().Length(1, 200);
            RuleFor(x => x.Birthdate).NotNull();
            RuleFor(x => x.CourseCode).NotNull();
            RuleFor(x => x.StudentContactDetails).SetValidator(new ContactDetailsValidator());
        }
    }
}
