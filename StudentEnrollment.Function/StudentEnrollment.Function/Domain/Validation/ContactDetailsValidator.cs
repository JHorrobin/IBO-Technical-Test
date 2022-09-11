using FluentValidation;
using StudentEnrollment.Function.Domain.Models;

namespace StudentEnrollment.Function.Domain.Validation
{
    public class ContactDetailsValidator : AbstractValidator<ContactDetails>
    {
        public ContactDetailsValidator()
        {
            RuleFor(x => x.Address).NotEmpty().Length(1, 200);
            RuleFor(x => x.Mobile).NotEmpty().Length(1, 20);
            RuleFor(x => x.Email).NotEmpty().Length(1, 200);
            RuleFor(x => x.Phone).NotEmpty().Length(1, 20);
            RuleFor(x => x.PreferredContactMethod).NotNull();
        }
    }
}
