using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VehicleTracking_Api.Constants;
using VehicleTracking_Api.Utilities.Validators;
using VehicleTracking_Data.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, ILogger<UserController> logger,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)

        {
            this._userService = userService;
            this._logger = logger;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;

        }
     

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/RegisterUser
        ///     {        
        ///       "firstName": "Micheal",
        ///       "lastName": "Andrew",
        ///       "userName": "mikeandrew@gmail.com",
        ///       "password": "DDF323PFGssd"
        ///     }
        /// </remarks>
        /// 
        /// <param name="newUser"></param>    
        /// <returns>Success or Error</returns>
        /// <response code="201">Successfully created user</response>
        /// <response code="400">Returns, errors,if the user object is invalid<</response>  
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserModel newUser)
        {
            try
            {

                if (newUser.IsValid(out IEnumerable<string> errors))
                {

                    var userExists = await _userManager.FindByNameAsync(newUser.UserName);
                    if (userExists != null)
                    {
                        _logger.LogError(ApiConstants.USER_ALREADY_EXISTS_MESSAGE + " {@user}", newUser);
                        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = ApiConstants.STATUS_ERROR, Message = ApiConstants.USER_ALREADY_EXISTS_MESSAGE });
                    }

                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = newUser.UserName,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Email = newUser.UserName
                    };

                    var result = await _userManager.CreateAsync(user, newUser.Password);
                    if (!result.Succeeded)
                    {
                        _logger.LogError(ApiConstants.USER_CREATION_FAILED + " {@result}", result);
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = ApiConstants.STATUS_ERROR, Message = ApiConstants.USER_CREATION_FAILED });
                    }

                    if (!await _roleManager.RoleExistsAsync(ApplicationUserRoles.User))
                        await _roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

                    await _userManager.AddToRoleAsync(user, ApplicationUserRoles.User);

                    return Ok(new Response { Status = ApiConstants.STATUS_SUCCESS, Message = ApiConstants.USER_CREATED_SUCCESSFULLY });

                    //var cosmoResult = await _userService.RegisterUser(newUser);

                    //_logger.LogInformation("Succesfully registered user @{object}", newUser);              

                }
                else
                {
                    _logger.LogError(ApiConstants.INVALID_PARAMS_FOR_USER_REGISTRATION + " {@errors}, Object:{@object}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ApiConstants.INTERNAL_SERVER_ERROR_FOR_USER_REGISTRATION + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiConstants.SOMETHING_WENT_WRONG);

            }
        }


    }
}
