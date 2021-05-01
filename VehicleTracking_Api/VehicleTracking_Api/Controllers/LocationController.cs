using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VehicleTracking_Api.Constants;
using VehicleTracking_Api.Utilities.General;
using VehicleTracking_Api.Utilities.Security;
using VehicleTracking_Api.Utilities.Validators;
using VehicleTracking_Data.Identity;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Models.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleTracking_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly IVehicleLocationService _locationService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public LocationController(IVehicleLocationService locationService, ILogger<UserController> logger,
                                  IConfiguration configuration)
        {
            this._locationService = locationService;
            this._logger = logger;
            this._configuration = configuration;

        }


        /// <summary>
        /// Vehicle sends GPS coordinates with a timestamp.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH api/Location/RecordPosition
        ///     {        
        ///       "userName": "dammy12@yahoo.com",
        ///       "vehicleReg": "DF-3456",
        ///       "latitude": "13.678639",
        ///       "longitude": "100.616405",
        ///       "timestamp":"2020-07-25T07:26:51.2395361Z"
        ///     }
        /// </remarks>
        /// 
        /// <param name="vehicleLocation"></param>    
        /// <returns>Success or Error</returns>
        /// <response code="201">Successfully sent location for time t </response>
        /// <response code="400">Returns, errors,if the vehicleLocation object is invalid</response>  
        [Authorize(Roles = "User")]
        [HttpPatch("RecordPosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> RecordPosition([FromBody] RecordPositionModel vehicleLocation)
        {
            try
            {

                if (ValidationExtension.CheckModelDataValidity(new RecordPositionModelValidator(vehicleLocation), vehicleLocation, out IEnumerable<string> errors))
                {
                    //check JWT token for username match,vehicle registeration match
                    var identity = User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        var username = claims.Where(p => p.Type == "Username").FirstOrDefault()?.Value;
                        var vehiclReg = claims.Where(p => p.Type == "VehicleReg").FirstOrDefault()?.Value;
                        if (!username.Equals(vehicleLocation.UserName) || !vehiclReg.Equals(vehicleLocation.VehicleReg))
                        {
                            _logger.LogError(Constants.ApiConstants.USERNAME_OR_VEHICLE_REGISTERATION_DOES_NOT_MATCH + " {@exception}", vehicleLocation);
                            return StatusCode(StatusCodes.Status400BadRequest, Constants.ApiConstants.USERNAME_OR_VEHICLE_REGISTERATION_DOES_NOT_MATCH);
                        }
                    }

                    //Append position to location array                              
                    var cosmoResult = await this._locationService.RecordPosition(vehicleLocation);
                    _logger.LogInformation(Constants.ApiConstants.LOCATION_RECORDED_SUCCESSFULLY + "@{object}", vehicleLocation);

                    return Ok(new ResponseModel { Status = Constants.ApiConstants.STATUS_SUCCESS, Message = Constants.ApiConstants.LOCATION_RECORDED_SUCCESSFULLY });

                }
                else
                {
                    _logger.LogError(Constants.ApiConstants.INVALID_PARAMS_GIVEN + "{@errors}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Constants.ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return  StatusCode(StatusCodes.Status500InternalServerError, Constants.ApiConstants.SOMETHING_WENT_WRONG);

            }
        }


        /// <summary>
        /// Admin receives current/latest GPS coordinates of a vehicle.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Location/GetVehicleLastLocation?vehicleRegNo=DF-3461
        /// </remarks>
        /// 
        /// <param name="vehicleRegNo"></param>    
        /// <response code="200">Vehicle location with the latest timestamp </response>
        /// <response code="400">Returns, errors,if the vehicleRegNo is invalid</response>  
        [Authorize(Roles = "Admin")]
        [HttpGet("GetVehicleLastLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> GetVehicleLastLocation(string vehicleRegNo)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(vehicleRegNo))
                {
                    _logger.LogError(Constants.ApiConstants.INVALID_PARAMS_GIVEN);
                    return BadRequest(Constants.ApiConstants.INVALID_PARAMS_GIVEN);
                }

                var cosmoResult = await this._locationService.GetLatestLocationOfVehicle(vehicleRegNo);
                if (cosmoResult == null)
                {
                    _logger.LogInformation(Constants.ApiConstants.NO_LOCATION_RECORDED_FOR_THE_VEHICLE + "@{object}", cosmoResult);
                    return Ok(new ResponseModel { Status = Constants.ApiConstants.STATUS_SUCCESS, Message = Constants.ApiConstants.NO_LOCATION_RECORDED_FOR_THE_VEHICLE });
                }

                _logger.LogInformation(Constants.ApiConstants.LATEST_LOCATION_RETRIEVED_SUCCESSFULLY + "@{object}", cosmoResult);
                return Ok(cosmoResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(Constants.ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.ApiConstants.SOMETHING_WENT_WRONG);

            }
        }

        /// <summary>
        /// Admin receives GPS coordinates of a vehicle for a time interval
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Location/GetVehicleLocationForTimeInterval?vehicleRegNo=DF-3461&lowerBoundTime=2020-07-25 07:26:51.239&upperBoundTime=2020-07-25 07:40:51.239
        ///     
        /// </remarks>
        /// 
        /// <param name="vehicleRegNo"></param>    
        /// <response code="200">Vehicle location with the latest timestamp </response>
        /// <response code="400">Returns, errors,if the vehicleRegNo is invalid</response>  
        [Authorize(Roles = "Admin")]
        [HttpGet("GetVehicleLocationForTimeInterval")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> GetVehicleLocationForTimeInterval(string vehicleRegNo,string lowerBoundTime, string upperBoundTime)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(vehicleRegNo))
                {
                    _logger.LogError(Constants.ApiConstants.INVALID_PARAMS_GIVEN);
                    return BadRequest(Constants.ApiConstants.INVALID_PARAMS_GIVEN);
                }
                if (!Util.BeAValidDateTime(lowerBoundTime)||
                    !Util.BeAValidDateTime(upperBoundTime))
                {
                    _logger.LogError(Constants.ApiConstants.INVALID_DATETIME_FORMAT);
                    return BadRequest(Constants.ApiConstants.INVALID_DATETIME_FORMAT);
                }

                var cosmoResult = await this._locationService.GetLocationForVehicleForGivenTime(vehicleRegNo,lowerBoundTime,upperBoundTime);
                if (cosmoResult == null)
                {
                    _logger.LogInformation(Constants.ApiConstants.NO_LOCATION_RECORDED_FOR_THE_VEHICLE + "@{object}", cosmoResult);
                    return Ok(new ResponseModel { Status = Constants.ApiConstants.STATUS_SUCCESS, Message = Constants.ApiConstants.NO_LOCATION_RECORDED_FOR_THE_VEHICLE });
                }

                _logger.LogInformation(Constants.ApiConstants.LOCATIONS_RECEIVED_SUCCESSFULLY + "@{object}", cosmoResult);
                return Ok(cosmoResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(Constants.ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.ApiConstants.SOMETHING_WENT_WRONG);

            }
        }


    }
}
