using System;
using FluentValidation;
using FluentValidation.Validators;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(userModel => userModel.Email).NotEmpty().EmailAddress();
            RuleFor(userModel => userModel.Password).MinimumLength(6);
        }

    }
}
