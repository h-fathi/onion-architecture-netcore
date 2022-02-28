using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core.FluentValidationExtensions
{
    public static class Extensions
    {
        public static IRuleBuilderOptions<T, string> IsMobileNumber<T>(this IRuleBuilder<T, string> rule)
            => rule.Matches(@"^(1-)?\d{3}-\d{3}-\d{4}$").WithMessage("Invalid phone number");

        public static IRuleBuilderOptions<T, string> IsUrl<T>(this IRuleBuilder<T, string> rule)
            => rule.Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$").WithMessage("Invalid Url");

        public static IRuleBuilderOptions<T, string> Contains<T>(this IRuleBuilder<T, string> rule, string value)
           => rule.Must(s=> s.Contains(value)).WithMessage("Not contains :" + value);

        public static IRuleBuilder<T, string> IsPassword<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 14)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage("Password cant be empty")
                .MinimumLength(minimumLength).WithMessage("Minimum length pf password must be "+ minimumLength)
                .Matches("[A-Z]").WithMessage("Use uppercase words in password")
                .Matches("[a-z]").WithMessage("Use lowercase words in password")
                .Matches("[0-9]").WithMessage("Use digits in password")
                .Matches("[^a-zA-Z0-9]").WithMessage("use special character in password");
            return options;
        }
    }
}
