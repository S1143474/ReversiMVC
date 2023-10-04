using Infrastructure.Persistence.Seeder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Initializer
{
    public class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            var seeder = new ApplicationDbSeeder(_userManager, _roleManager);
            
            if (!_context.Users.Any())
            {
                await seeder.CreateUsers();
            }

            if (!_context.Roles.Any())
            {
                await seeder.CreateRoles();
            }

            if (!_context.UserRoles.Any())
            {
                await seeder.AddUserTolRoles();
            }
        }
    }
}
