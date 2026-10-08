using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Controllers.UserModel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<AppUser> userManager,
                                 IConfiguration configuration)
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
                foreach(var error in result.Errors) 
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




        //[AllowAnonymous]
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(EntryViewModel model)
        //{
        //    AppUser targetUser = null;
        //    var userList = _userManager.Users.Where(x => x.UserName == model.LoginProp);
        //    foreach (var user in userList)
        //    {
        //        PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
        //        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
        //        {
        //            targetUser = user;
        //            break;
        //        }
        //    }

        //    if (targetUser != null)
        //    {
        //        var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, targetUser.UserName),
        //                new Claim(ClaimTypes.NameIdentifier,targetUser.Id)
        //            };
        //        var roles = await _userManager.GetRolesAsync(targetUser);
        //        foreach (var role in roles) 
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, role));
        //        }

        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        byte[] signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
        //        var symmetricSecurityKey = new SymmetricSecurityKey(signingKey);

        //        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(  claims ),
        //            Expires = DateTime.UtcNow.AddDays(1),
        //            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        //        };

        //        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //        string tokenString = tokenHandler.WriteToken(securityToken);

        //        return Ok(new
        //        {
        //            UserName = targetUser.UserName,
        //            UserRoles = roles.ToList<string>(),
        //            Token = tokenString
        //        }); 

        //    }
        //    return NotFound();

        //}


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var users = await _userManager.SelectAllUsers();
                return Ok(users);
            }
            catch 
            {
                return NotFound(ModelState);
            } 
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try 
            {
                var curUser = await _userManager.GetUserAsync(HttpContext.User);
                if (curUser.Id != id)
                {
                    (await _userManager.FindByIdAsync(id))
                        .DeleteUser(_userManager);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Вы пытались удалить себя.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return BadRequest(ModelState);
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserRole(string id, List<string> newroles)
        {
            try 
            {
                var curUser = await _userManager.GetUserAsync(HttpContext.User);
                if (curUser.Id != id)
                {
                    (await _userManager.FindByIdAsync(id))
                        .ChangeUserRoles(_userManager, newroles);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить роль администратора.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(RegisterViewModelForAdmin model)
        {
            try 
            {
                AppUser user = new AppUser { UserName = model.LoginProp };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRolesAsync(user, model.UserRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserPassword(string id, PasswordModel model) 
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id != id)
            {
                AppUser targetUser = await _userManager.FindByIdAsync(id);
                if (targetUser != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUser>)) as IPasswordValidator<AppUser>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUser>)) as IPasswordHasher<AppUser>;
                    try
                    {
                        IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, targetUser, model.Password);
                        if (result.Succeeded)
                        {
                            targetUser.PasswordHash = _passwordHasher.HashPassword(targetUser, model.Password);
                            await _userManager.UpdateAsync(targetUser);
                            return Ok();
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить свой пароль.");
            }
            return BadRequest(ModelState);
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
