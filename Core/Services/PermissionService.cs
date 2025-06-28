using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Abstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PermissionService : IPermissionService
    {
        private readonly SmartInventoryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(SmartInventoryDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(string userId)
        {
            var permissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            return permissions;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
        {
            return await _context.UserPermissions
                .Include(up => up.Permission)
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permissionName);
        }

        public async Task AssignPermissionToUserAsync(string userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionName);

            if (permission == null)
            {
                permission = new Permission { Name = permissionName };
                _context.Permissions.Add(permission);
                await _context.SaveChangesAsync();
            }

            if (!await UserHasPermissionAsync(userId, permissionName))
            {
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permission.Id
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemovePermissionFromUserAsync(string userId, string permissionName)
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionName);

            if (permission == null) return;

            var userPermission = await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permission.Id);

            if (userPermission != null)
            {
                _context.UserPermissions.Remove(userPermission);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<UserPermissionsDto> GetPermissionsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserPermissionsDto
            {
                Email = user.Email!,
                FullName = user.FullName ?? "",
                Roles = roles.ToList()
            };
        }
    }
}

