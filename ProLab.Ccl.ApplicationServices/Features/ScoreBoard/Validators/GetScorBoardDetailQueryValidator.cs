using FluentValidation;
using ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Commands;
using ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Queries;
using ProLab.Infrastructure.Core.FluentValidationExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Validators
{
    public class CreateScoreBoardCommandValidator : AbstractValidator<CreateScoreBoardCommand>
    {
        public CreateScoreBoardCommandValidator()
        {
            //RuleFor(p => p.IdentityId).GreaterThan(0).WithErrorCode("Ccl-106");
            //RuleFor(p => p.Name).IsUrl().WithErrorCode("Ccl-102");


            //// condition rule
            //When(p => p.AreYouIn , () =>
            //{
            //    RuleFor(p => p.IdentityId).GreaterThan(0);
            //}).Otherwise(() =>
            //{
            //    RuleFor(p => p.IdentityId).NotEmpty().WithErrorCode("Ccl-105");
            //});
        }

       
    }

    

}
