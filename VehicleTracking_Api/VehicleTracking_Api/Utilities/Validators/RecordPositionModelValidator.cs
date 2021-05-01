using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Api.Utilities.General;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.Utilities.Validators
{
    public class RecordPositionModelValidator : ParentModelValidator
    {
        public RecordPositionModelValidator(RecordPositionModel recordPositionModel)
        {
            RuleFor(model => recordPositionModel.UserName).NotEmpty().EmailAddress();
            RuleFor(model => recordPositionModel.VehicleReg).NotEmpty();
            RuleFor(model => recordPositionModel.Latitude).NotEmpty().InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees inclusive.");
            RuleFor(model => recordPositionModel.Longitude).NotEmpty().InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees inclusive.");
            RuleFor(model => recordPositionModel.Timestamp).Must(Util.BeAValidDateTime).WithMessage("Invalid date time should be of format yyyy-MM-dd HH:mm:ss.fff");

        }


    }
}
