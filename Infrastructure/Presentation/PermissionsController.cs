using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.DTOs;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetPermissions(string email)
        {
            var permissions = await _permissionService.GetPermissionsByEmailAsync(email);
            return Ok(permissions);
        }
    }
}
