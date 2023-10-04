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

        public void Seed()
        {

        }

        public async Task CreateUsers()
        {
            for (int i = 0; i < identiyUsers.Count; i++)
            {
                var user = identiyUsers[i];
                var pass = identityPassword[i];

                await _userManager.CreateAsync(user, pass);
            }
        }

        public async Task CreateRoles()
        {
            foreach (var identityRole in roles)
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
            for (int i = 0; i < identiyUsers.Count; i++)
            {
                var user = identiyUsers[i];

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

        private static List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole("Speler"),
            new IdentityRole("Moderator"),
            new IdentityRole("Admin")
        };


        private static List<string> identityPassword = new List<string>
        {
            "!12Admin!12Admin",
            "@34Moderator@34Moderator",
            "#56Speler#56Speler"
        };

        private static List<IdentityUser> identiyUsers = new List<IdentityUser>
        {
            new IdentityUser
            {
                UserName = "admin",
                Email = "admin@reversi.nl"
            },

            new IdentityUser
            {
                UserName = "moderator",
                Email = "moderator@reversi.nl"
            },

            new IdentityUser
            {
                UserName = "speler",
                Email = "speler@reversi.nl"
            }
        };
    }
}
