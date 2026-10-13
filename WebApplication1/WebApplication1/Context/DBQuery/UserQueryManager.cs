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

        public static async Task<UserModel> SelectUser(this UserManager<AppUser> _userManager, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var model = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            var roles = await _userManager.GetRolesAsync(user);
            model.Roles = roles.Select(r => new UserRole { Role = r }).ToList();

            return model;
        }

        
        public static async Task<IdentityResult> ChangeUserRoles(this UserManager<AppUser> _userManager, AppUser targetUser, List<string> newroles) 
        {
            var roles = await _userManager.GetRolesAsync(targetUser);
            var removeRes = await _userManager.RemoveFromRolesAsync(targetUser, roles);
            if (!removeRes.Succeeded) return removeRes;
            var addRes = await _userManager.AddToRolesAsync(targetUser, newroles);
            if (!addRes.Succeeded) return addRes;
            return IdentityResult.Success;
        }



        public static async Task<IdentityResult> DeleteUserAsync(this UserManager<AppUser> userManager, AppUser targetUser) 
        { 
            var roles = await userManager.GetRolesAsync(targetUser); 
            var removeRes = await userManager.RemoveFromRolesAsync(targetUser, roles); 
            if (!removeRes.Succeeded) return removeRes; 
            return await userManager.DeleteAsync(targetUser); 
        }

    }
}
