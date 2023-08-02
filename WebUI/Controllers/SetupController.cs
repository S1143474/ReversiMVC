using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    [Route("[controller]")]
    public class SetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;

        public SetupController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<SetupController> logger
            ) 
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            // Check if role exists
            var roleExists = await _roleManager.RoleExistsAsync(name);

            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

                // We need to check if the role has been added succesfully
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The Role {name} has been added successfully.");
                    return Ok(new { 
                        result = $"The role {name} has been added successfully."
                    });
                } else
                {
                    _logger.LogInformation($"The Role {name} has not been added.");
                    return BadRequest(new
                    {
                        error = $"The role {name} has not been added."
                    });
                }
            }

            return BadRequest(new { error = "Role already exists" });
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AssignRole")]
        public async Task<IActionResult> AssignRoleToUser(string email, string roleName)
        {
            // Check if the user exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"The use with the email: {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                _logger.LogInformation($"The role {roleName} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.UpdateSecurityStampAsync(user);

            // check if the user is assigned to the role successfully
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "Success, user has been added to the role"
                });
            } else
            {
                _logger.LogInformation($"The user was not able to be added to the role {roleName} does not exist");
                return BadRequest(new
                {
                    error = $"The user was not able to be added to the role {roleName} does not exist"
                });
            }

        }

        [HttpGet]
        [Route("UserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            // Check if the email is valid
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"The use with the email: {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }

            // return roles
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            // Check if the email is valid
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation($"The use with the email: {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            await _userManager.UpdateSecurityStampAsync(user);

            if (!roleExists)
            {
                _logger.LogInformation($"The role {roleName} does not exist");
                return BadRequest(new
                {
                    error = "Role does not exist"
                });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"User {email} has been removed from role{roleName}"
                });
            }

            return BadRequest(new
            {
                error = $"Unable to remove user {email} from role {roleName}"
            });
        }
    }
}
