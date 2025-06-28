using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IPermissionService
    {
        Task<IEnumerable<string>> GetUserPermissionsAsync(string userId);
        Task<bool> UserHasPermissionAsync(string userId, string permissionName);
        Task AssignPermissionToUserAsync(string userId, string permissionName);
        Task RemovePermissionFromUserAsync(string userId, string permissionName);

        Task<UserPermissionsDto> GetPermissionsByEmailAsync(string email);
    }
}
