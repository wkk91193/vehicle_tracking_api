using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleTracking_Api.Utilities.Validators;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Models.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleTracking_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this._userService = userService;
            this._logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/RegisterUser
        ///     {        
        ///       "Email": "randomemail@gmail.com",
        ///       "Password": "p@AworD"
        ///     }
        /// </remarks>
        /// <param name="User"></param>    
        /// <returns>Token value for the user</returns>
        /// <response code="201">Returns token string for the user</response>
        /// <response code="400">Returns, errors,if the user object is invalid</response>     
        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            try
            {

                if (user.IsValid(out IEnumerable<string> errors))
                {
                    var result = await _userService.RegisterUser(user);

                    _logger.LogInformation("Succesfully registered user @{object}", result.Email);

                    return Ok("Success");

                }
                else
                {
                    _logger.LogError("Invalid params at RegisterUser Errors: {@errors}, Object:{@object}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server error at CreateApplicant {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");

            }
        }


    }
}
