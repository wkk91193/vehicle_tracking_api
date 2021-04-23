using System;
using System.Collections.Generic;
using FluentValidation.Results;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public static class ValidationExtension
    {
        public static bool IsValid(this RegisterUserModel registerUserModel, out IEnumerable<string> errors)
        {
            var validator = new RegisterUserModelValidator();

            var validationResult = validator.Validate(registerUserModel);

            errors = AggregateErrors(validationResult);

            return validationResult.IsValid;
        }

        private static List<string> AggregateErrors(ValidationResult validationResult)
        {
            var errors = new List<string>();

            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    errors.Add(error.ErrorMessage);

            return errors;
        }
    }
}
