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
    [Authorize(Roles = "admin")]
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


        [HttpGet]
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try 
            {
                var curUser = await _userManager.GetUserAsync(HttpContext.User);
                if (curUser.Id == id)
                {
                    ModelState.AddModelError(string.Empty, "Вы пытались удалить себя.");
                    return BadRequest(ModelState);
                }

                var targetUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteUserAsync(targetUser);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                var user = await _userManager.SelectUser(id);
                return Ok(user);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }


        [HttpPut]
        public async Task<IActionResult> ChangeUserRole(string id, List<string> newroles)
        {
            try 
            {
                var curUser = await _userManager.GetUserAsync(HttpContext.User);
                if (curUser.Id == id)
                {
                    ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить роль администратора.");
                    return BadRequest(ModelState);
                }

                var targetUser = await _userManager.FindByIdAsync(id);
                var result = await _userManager.ChangeUserRoles(targetUser, newroles);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                var user = await _userManager.SelectUser(id);
                return Ok(user);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
            
        }

      

        //[HttpPost]
        //public async Task<IActionResult> ChangeUserPassword(string id, PasswordModel model) 
        //{
        //    var curUser = await _userManager.GetUserAsync(HttpContext.User);
        //    if (curUser.Id != id)
        //    {
        //        AppUser targetUser = await _userManager.FindByIdAsync(id);
        //        if (targetUser != null)
        //        {
        //            var _passwordValidator =
        //                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<AppUser>)) as IPasswordValidator<AppUser>;
        //            var _passwordHasher =
        //                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<AppUser>)) as IPasswordHasher<AppUser>;
        //            try
        //            {
        //                IdentityResult result =
        //                await _passwordValidator.ValidateAsync(_userManager, targetUser, model.Password);
        //                if (result.Succeeded)
        //                {
        //                    targetUser.PasswordHash = _passwordHasher.HashPassword(targetUser, model.Password);
        //                    await _userManager.UpdateAsync(targetUser);
        //                    return Ok();
        //                }
        //                else
        //                {
        //                    foreach (var error in result.Errors)
        //                    {
        //                        ModelState.AddModelError(string.Empty, error.Description);
        //                    }
        //                }
        //            }
        //            catch (Exception ex) 
        //            {
        //                ModelState.AddModelError(string.Empty, ex.Message);
        //            }
                    
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Пользователь не найден");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить свой пароль.");
        //    }
        //    return BadRequest(ModelState);
        //}



        [HttpGet]
        public async Task<IActionResult> GeneratePasswordResetToken(string id)
        {
            var curUser = await _userManager.GetUserAsync(HttpContext.User);
            if (curUser.Id == id) 
            {
                ModelState.AddModelError(string.Empty, "Вы пытаетесь изменить свой пароль.");
                return BadRequest(ModelState);
            }

            try 
            {
                var user = await _userManager.FindByIdAsync(id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }            
        }


    }
}
