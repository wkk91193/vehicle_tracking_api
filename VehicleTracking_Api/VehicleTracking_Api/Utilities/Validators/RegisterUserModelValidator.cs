using System;
using FluentValidation;
using FluentValidation.Validators;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidator()
        {
            RuleFor(userModel => userModel.UserName).NotEmpty().EmailAddress();

            RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
                   

            RuleFor(userModel => userModel.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
            RuleFor(userModel => userModel.LastName).NotEmpty().MinimumLength(2).MaximumLength(20);
        }

  
        
       
    }
}
