using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking_Api.Constants
{
    public class ApiConstants
    {
        public const string USER_ALREADY_EXISTS_MESSAGE = "User already exists!";
        public const string USER_CREATION_FAILED = "User creation failed! Please check user details and try again.";
        public const string STATUS_ERROR = "Error";
        public const string USER_CREATED_SUCCESSFULLY = "User created successfully!";
        public const string STATUS_SUCCESS = "Success";
        public const string INVALID_PARAMS_FOR_USER_REGISTRATION = "Invalid params at RegisterUser Errors";
        public const string INTERNAL_SERVER_ERROR_FOR_USER_REGISTRATION = "Internal Server error at RegisterUser";
        public const string SOMETHING_WENT_WRONG = "Something went wrong!";
    }
}
