using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public class RegisterAdminUserModelValidator : ParentModelValidator
    {
        public RegisterAdminUserModelValidator(RegisterAdminUserModel model)
        {
            RuleFor(userModel => model.UserName).NotEmpty().EmailAddress();

            RuleFor(p => model.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");


            RuleFor(userModel => model.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20).WithMessage("firstName must not be empty");
            RuleFor(userModel => model.LastName).NotEmpty().MinimumLength(2).MaximumLength(20).WithMessage("lastName must not be empty");
  

        }
    
    }
}
