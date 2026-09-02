using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Controllers.UserModel;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ServiceContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<AppUser> userManager,
                                 ServiceContext context,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
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
        public IActionResult Login_(EntryViewModel model)
        {
            AppUser targetUser = null;
            var userList = _context.Users.Where(x => x.UserName == model.LoginProp);
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
                var roles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            where ur.UserId == targetUser.Id
                            select new UserRole { Role = r.Name };
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var symmetricSecurityKey = new SymmetricSecurityKey(signingKey);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, targetUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier,targetUser.Id)
                    };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role));
                }

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




        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(EntryViewModel model)
        {
            AppUser targetUser = null;
            var userList = _context.Users.Where(x => x.UserName == model.LoginProp);
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
                var roles = from ur in _context.UserRoles
                           join r in _context.Roles on ur.RoleId equals r.Id
                           where ur.UserId == targetUser.Id
                           select new UserRole { Role = r.Name };
                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var symmetricSecurityKey = new SymmetricSecurityKey(signingKey);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, targetUser.UserName),
                        new Claim(ClaimTypes.NameIdentifier,targetUser.Id)
                    };
                foreach (var role in roles) 
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role));
                }

                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(  claims ),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(securityToken);

                return Ok(new
                {
                    UserName = targetUser.UserName,
                    UserRoles = roles.ToList<UserRole>(),
                    Token = tokenString
                }); 

            }
            return NotFound();

        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetUserList()
        {
            try
            {
                var users = from user in _context.Users
                            join userRole in _context.UserRoles on user.Id equals userRole.UserId
                            join roles in _context.Roles on userRole.RoleId equals roles.Id
                            select new UserModelForAdmin
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                UserRole = roles.Name,
                            };
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
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id != id)
            {

                var targetUser = _context.Users.SingleOrDefault(x => x.Id == id);
                if (targetUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(targetUser);
                    await _userManager.RemoveFromRolesAsync(targetUser, roles);
                    await _userManager.DeleteAsync(targetUser);
                    return Ok();
                }
                else ModelState.AddModelError(string.Empty, "Пользователь не найден.");
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Вы пытались удалить себя.");
                
            }
            return BadRequest(ModelState);

        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserRole(string id, List<UserRole> newroles)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id != id) 
            {
                AppUser targetUser = _context.Users.SingleOrDefault(x => x.Id == id);
                var roles = await _userManager.GetRolesAsync(targetUser);
                await _userManager.RemoveFromRolesAsync(targetUser, roles);

                if (newroles.Count > 0)
                {
                    foreach (var role in newroles)
                    {                       
                        var result = await _userManager.AddToRoleAsync(targetUser, role.Role);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        
                    }
                    if(ModelState.ErrorCount == 0) return Ok();
                }
                else 
                {
                    await _userManager.AddToRoleAsync(targetUser, "myrole");
                }
            }        
            else
            {
                ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить роль администратора.");
            }
                    
            return BadRequest(ModelState);

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser(RegisterViewModelForAdmin model)
        {
            AppUser user = new AppUser { UserName = model.LoginProp };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.UserRole);
                return Ok();
            }
            else return BadRequest();

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
