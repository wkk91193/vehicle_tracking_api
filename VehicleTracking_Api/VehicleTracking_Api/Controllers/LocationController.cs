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
                            _logger.LogError(ApiConstants.USERNAME_OR_VEHICLE_REGISTERATION_DOES_NOT_MATCH + " {@exception}", vehicleLocation);
                            return StatusCode(StatusCodes.Status400BadRequest, ApiConstants.USERNAME_OR_VEHICLE_REGISTERATION_DOES_NOT_MATCH);
                        }
                    }

                    //Append position to location array                              
                    var cosmoResult = await this._locationService.RecordPosition(vehicleLocation);
                    _logger.LogInformation(ApiConstants.LOCATION_RECORDED_SUCCESSFULLY + "@{object}", vehicleLocation);

                    return Ok(new Response { Status = ApiConstants.STATUS_SUCCESS, Message = ApiConstants.LOCATION_RECORDED_SUCCESSFULLY });

                }
                else
                {
                    _logger.LogError(ApiConstants.INVALID_PARAMS_GIVEN + "{@errors}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiConstants.SOMETHING_WENT_WRONG);

            }
        }

    }
}
