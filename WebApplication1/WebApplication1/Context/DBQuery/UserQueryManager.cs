using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.UserModel;
using WebApplication1.Models;

namespace WebApplication1.Context.DBQuery
{
    public static class UserQueryManager
    {
        public static async Task<List<UserModel>> SelectAllUsers(this UserManager<AppUser> _userManager)
        {
            var modelList = await _userManager.Users.Select(u => new UserModel
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToListAsync();

            foreach (var model in modelList) 
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                var roles = await _userManager.GetRolesAsync(user);
                model.Roles = roles.Select(r => new UserRole { Role = r }).ToList();
            }
            return modelList;
        }

        public static async Task ChangeUserRoles(this AppUser targetUser, UserManager<AppUser> _userManager, List<string> newroles) 
        {
            var roles = await _userManager.GetRolesAsync(targetUser);
            await _userManager.RemoveFromRolesAsync(targetUser, roles);
            await _userManager.AddToRolesAsync(targetUser, newroles);
        }

        public static async Task DeleteUser(this AppUser targetUser, UserManager<AppUser> _userManager) 
        {           
            var roles = await _userManager.GetRolesAsync(targetUser);
            await _userManager.RemoveFromRolesAsync(targetUser, roles);
            await _userManager.DeleteAsync(targetUser);            
        }
    }
}
