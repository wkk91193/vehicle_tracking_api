using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public static class ValidationExtension
    {
        public static bool CheckModelDataValidity(ParentModelValidator validator, ParentModel model,out IEnumerable<string> errors)
        {
            var validateResult=validator.Validate(model);
            errors = AggregateErrors(validateResult);
            return validateResult.IsValid;
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
