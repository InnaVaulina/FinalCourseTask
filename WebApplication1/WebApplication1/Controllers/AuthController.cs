using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Controllers.UserModel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            AppUser user = new AppUser { UserName = model.LoginProp };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "myrole");
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login_(EntryViewModel model)
        {
            AppUser targetUser = null;
            var userList = _userManager.Users.Where(x => x.UserName == model.LoginProp);
            foreach (var user in userList)
            {
                PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
                if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    targetUser = user;
                    break;
                }
            }

            if (targetUser != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, targetUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier,targetUser.Id)
                    };
                var roles = await _userManager.GetRolesAsync(targetUser);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var symmetricSecurityKey = new SymmetricSecurityKey(signingKey);

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    //Expires = DateTime.UtcNow.AddDays(1),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(securityToken);

                return Ok(new
                {
                    Token = tokenString
                });

            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ChangeCurrentUserPassword(PasswordChangeModel model)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            IdentityResult result =
                await _userManager.ChangePasswordAsync(curUser, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
