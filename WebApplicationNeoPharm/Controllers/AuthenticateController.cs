using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplicationNeoPharm.Authenticate;

namespace WebApiAvi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {


        // GET: api/Authenticate
        //private readonly UserManager<ApplicationUser> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        //public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        //{
        //    this.userManager = userManager;
        //    this.roleManager = roleManager;
        //    _configuration = configuration;
        //}
        public AuthenticateController(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        //public async Task<IActionResult> Login(LoginModel model)
        public IActionResult Login(LoginModel model)
        {


            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Email, "uri.dreiblatt@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            //authClaims.Add(new Claim(ClaimTypes.Role, UserRoles));

            //foreach (var userRole in userRoles)
            //    {
            //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //    }
            if (model.Username.Equals("urid"))
                authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Admin));
            else
                authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.User));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
            //}
            //return Unauthorized();
        }


        [HttpPost]
        [Route("Register")]
        //public async Task<IActionResult> Login(LoginModel model)
        public IActionResult Register(RegisterModel model)
        {
            //Cls_result_api c_ret = new Cls_result_api();

            try
            {
                return Ok(new { Info = "Getcards.get" });
            }
            catch (Exception er)
            {

                return StatusCode(500, new { Info = er.Message, Source = "Register.Post" });
            }


        }




    }
}
