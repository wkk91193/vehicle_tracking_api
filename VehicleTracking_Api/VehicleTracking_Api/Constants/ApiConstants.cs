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
        public const string INVALID_PARAMS_GIVEN = "Invalid params given";
        public const string NO_PARAMS_GIVEN = "No params given";
        public const string INTERNAL_SERVER_ERROR = "Internal Server error";
        public const string SOMETHING_WENT_WRONG = "Something went wrong!";
        public const string NO_SUCH_USER_EXISTS = "No such user matching the given username";
        public const string INCORRECT_USERNAME_PASSWORD = "Incorrect userName/password";
        public const string VEHICLE_REGISTRATION_ALREADY_FOUND = "Vehicle already registered";
        public const string NO_SUCH_REGISTERED_USER = "No such user found with the given username.Please register yourself first";
        public const string USERNAME_OR_VEHICLE_REGISTERATION_DOES_NOT_MATCH = "The given userName or vehicleReg does not match the records on file";
        public const string LOCATION_RECORDED_SUCCESSFULLY = "Location recorded successfully!";
        public const string LATEST_LOCATION_RETRIEVED_SUCCESSFULLY = "Latest location for vehicle retrieved successfully!";
        public const string NO_LOCATION_RECORDED_FOR_THE_VEHICLE = "No location recorded for the vehicle!";
        public const string INVALID_DATETIME_FORMAT = "Invalid datetime format please ensure it's yyyy-MM-dd HH:mm:ss.fff";
        public const string LOCATIONS_RECEIVED_SUCCESSFULLY = "Location(s) received successfully!";


    }
}
