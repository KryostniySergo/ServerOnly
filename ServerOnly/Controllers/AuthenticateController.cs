using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerOnly.Model.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerOnly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly IConfiguration _configuration;


        //Брал инфу отсюда
        //https://www.c-sharpcorner.com/article/jwt-authentication-and-authorization-in-net-6-0-with-identity-framework/

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null
                ||
                !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Введенные данные не верны");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AuthModel model)
        {
            var userExits = await _userManager.FindByNameAsync(model.Username);
            if(userExits != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Пользователь уже создан!" });
            }

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };


            //TODO-> СДЕЛАТЬ НОРМАЛЬНЫЕ ОТВЕТЫ ОТ СЕРВА!
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new {
                        Status = "Error", 
                        Message = "При создании пользователя произошла ошибка!",
                        Errors = result.Errors.ToArray()
                    }
                    );
            }

            return Ok("Пользователь успешно создан");
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AuthModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new Response { Status = "Error", Message = "Пользователь уже создан!" });
            }

            IdentityUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "При создании пользователя произошла ошибка! Пожалуйста проверьте данные пользователя и попробуйте снова." });
            }

            if(!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRolesAsync(user,
                    new[]
                    {
                        UserRoles.Admin,
                        UserRoles.User,
                    });
            }

            return Ok("Админ создан успешно!");

        }

        JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:ValidIssuer"],
                 audience: _configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddHours(3),
                 claims: authClaims,
                 signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }
}
