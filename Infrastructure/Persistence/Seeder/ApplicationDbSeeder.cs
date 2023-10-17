using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeder
{
    public class ApplicationDbSeeder
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateUsers()
        {
            for (int i = 0; i < SpelerIdentity.identiyUsers.Count; i++)
            {
                var user = SpelerIdentity.identiyUsers[i];
                var pass = SpelerIdentity.identityPassword[i];

                await _userManager.CreateAsync(user, pass);
            }
        }

        public async Task CreateRoles()
        {
            foreach (var identityRole in SpelerIdentity.roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(identityRole.Name);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(identityRole);
                }

            } 
        }

        public async Task AddUserTolRoles()
        {
            for (int i = 0; i < SpelerIdentity.identiyUsers.Count; i++)
            {
                var user = SpelerIdentity.identiyUsers[i];

                await _userManager.AddToRoleAsync(user, "Speler");

                if (i == 0)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                if (i < 2)
                {
                    await _userManager.AddToRoleAsync(user, "Moderator");
                }
            }
        }
    }
}
