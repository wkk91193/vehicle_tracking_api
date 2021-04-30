using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VehicleTracking_Api.Constants;
using VehicleTracking_Api.Utilities.Cleanup;
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
    public class UserController : ControllerBase
    {
        private readonly IVehicleUserService _userService;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(IVehicleUserService userService, ILogger<UserController> logger,
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
        ///     POST api/User/RegisterUser
        ///     {        
        ///       "firstName": "Micheal",
        ///       "lastName": "Andrew",
        ///       "userName": "mikeandrew@gmail.com",
        ///       "password": "DDF323PFGssd",
        ///       "vehicleInfo":{
        ///             "vehicleReg":"GA-2345"          
        ///        }
        ///     }
        /// </remarks>
        /// 
        /// <param name="newUser"></param>    
        /// <returns>Success or Error</returns>
        /// <response code="201">Successfully created user</response>
        /// <response code="400">Returns, errors,if the user object is invalid</response>  
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserModel newUser)
        {
            try
            {

                if (ValidationExtension.CheckModelDataValidity(new RegisterUserModelValidator(newUser), newUser, out IEnumerable<string> errors))
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

                    var cosmoResult = await _userService.SaveUserToCosmo(newUser);
                    if (!cosmoResult)
                    {
                        await CleanupResources.RemoveUserFromDBAsync(user, this._userManager);
                        _logger.LogError(ApiConstants.VEHICLE_REGISTRATION_ALREADY_FOUND);
                        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = ApiConstants.STATUS_ERROR, Message = ApiConstants.VEHICLE_REGISTRATION_ALREADY_FOUND });
                    }

                    _logger.LogInformation("Succesfully registered user @{object}", newUser);
                    return Ok(new Response { Status = ApiConstants.STATUS_SUCCESS, Message = ApiConstants.USER_CREATED_SUCCESSFULLY });

                }
                else
                {
                    _logger.LogError(ApiConstants.INVALID_PARAMS_GIVEN + "{@errors}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = newUser.UserName
                };
                await CleanupResources.RemoveUserFromDBAsync(user, this._userManager);
                _logger.LogError(ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiConstants.SOMETHING_WENT_WRONG);

            }
        }


        /// <summary>
        /// Registers a new admin user.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/User/RegisterAdminUser
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
        /// <response code="400">Returns, errors,if the user object is invalid </response>  
        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterAdminUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterAdminUser([FromBody] RegisterAdminUserModel newUser)
        {
            try
            {

                if (ValidationExtension.CheckModelDataValidity(new RegisterAdminUserModelValidator(newUser), newUser, out IEnumerable<string> errors))
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


                    await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);

                    await _userService.SaveAdminToCosmo(newUser);

                    _logger.LogInformation("Succesfully registered admin user @{object}", newUser);

                    return Ok(new Response { Status = ApiConstants.STATUS_SUCCESS, Message = ApiConstants.USER_CREATED_SUCCESSFULLY });

                }
                else
                {
                    _logger.LogError(ApiConstants.INVALID_PARAMS_GIVEN + "{@errors}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                var appUser = await _userManager.FindByNameAsync(newUser.UserName);
                if (appUser != null)
                {
                    await _userManager.DeleteAsync(appUser);
                }
                _logger.LogError(ApiConstants.INTERNAL_SERVER_ERROR + " {@exception}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiConstants.SOMETHING_WENT_WRONG);

            }
        }

        /// <summary>
        /// Gets a token for the user
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/User/GetToken
        ///     {        
        ///       "username": "mikeandrew@gmail.com",
        ///       "password": "DDF323PFGssd"
        ///     }
        /// </remarks>
        /// 
        /// <param name="loginUser"></param>    
        /// <returns>Success or Error</returns>
        /// <response code="200">Returns, the token string</response>
        /// <response code="400">Returns, errors,if the user object is invalid</response>  
        [AllowAnonymous]
        [HttpPost("GetToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> GetToken([FromBody] LoginUserModel loginUser)
        {
            try
            {

                if (ValidationExtension.CheckModelDataValidity(new LoginUserModelValidator(loginUser), loginUser, out IEnumerable<string> errors))
                {

                    var userExists = await this._userManager.FindByNameAsync(loginUser.Username);
                    if (userExists == null)
                    {
                        _logger.LogError(ApiConstants.NO_SUCH_USER_EXISTS + " {@user}", loginUser);
                        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = ApiConstants.STATUS_ERROR, Message = ApiConstants.NO_SUCH_USER_EXISTS });
                    }
                    var passwordValid = await this._userManager.CheckPasswordAsync(userExists, loginUser.Password);
                    if (!passwordValid)
                    {
                        _logger.LogError(ApiConstants.INCORRECT_USERNAME_PASSWORD + " {@user}", loginUser);
                        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = ApiConstants.STATUS_ERROR, Message = ApiConstants.INCORRECT_USERNAME_PASSWORD });
                    }

                    var userVehicleInformation = await this._userService.GetVehicleUserInformationByUsername(loginUser.Username);

                    List<Claim> authClaims = null;
                    if (userVehicleInformation.vehicleInfo.vehicleReg == null)
                    {
                        authClaims = new List<Claim>{
                            new Claim(ClaimTypes.Role,userVehicleInformation.roleType),
                            new Claim("Username", userVehicleInformation.email),
                            new Claim("VehicleReg","")
                        };

                    }
                    else
                    {
                        authClaims = new List<Claim>{
                            new Claim(ClaimTypes.Role,userVehicleInformation.roleType),
                            new Claim("Username", userVehicleInformation.email),
                            new Claim("VehicleReg",userVehicleInformation.vehicleInfo.vehicleReg)
                        };
                    }



                    var token = JWTHelper.GetJwtToken(
                                          loginUser.Username,
                                          _configuration["JWT:Secret"],
                                          _configuration["JWT:ValidIssuer"],
                                          _configuration["JWT:ValidAudience"],
                                          TimeSpan.FromDays(Double.Parse(_configuration["JWT:TimeSpan"])),
                                          authClaims.ToArray());

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expires = token.ValidTo
                    });

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
