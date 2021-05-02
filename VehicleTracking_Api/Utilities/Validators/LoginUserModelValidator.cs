using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
 
    public class LoginUserModelValidator : ParentModelValidator { 
        public LoginUserModelValidator(LoginUserModel loginUserModel)
        {
            RuleFor(userModel => loginUserModel.Username).NotEmpty().EmailAddress();

            RuleFor(p => loginUserModel.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");

        }


    }
}
