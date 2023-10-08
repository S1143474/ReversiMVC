using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeder
{
    public class SpelerIdentity
    {

        public static List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole("Speler"),
            new IdentityRole("Moderator"),
            new IdentityRole("Admin")
        };


        public static List<string> identityPassword = new List<string>
        {
            "!12Admin!12Admin",
            "@34Moderator@34Moderator",
            "#56Speler#56Speler"
        };

        public static List<IdentityUser> identiyUsers = new List<IdentityUser>
        {
            new IdentityUser
            {
                UserName = "admin",
                Email = "admin@reversi.nl",
                EmailConfirmed = true,
            },

            new IdentityUser
            {
                UserName = "moderator",
                Email = "moderator@reversi.nl",
                EmailConfirmed = true,
            },

            new IdentityUser
            {
                UserName = "speler",
                Email = "speler@reversi.nl",
                EmailConfirmed = true,
            }
        };

        public static List<Speler> Spelers = new List<Speler>
        {
            new Speler(identiyUsers[0].Id, identiyUsers[0].UserName),
            new Speler(identiyUsers[1].Id, identiyUsers[1].UserName),
            new Speler(identiyUsers[2].Id, identiyUsers[2].UserName)
        };
    }
}
