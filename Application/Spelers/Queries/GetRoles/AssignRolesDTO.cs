using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spelers.Queries.GetRoles
{
    public class AssignRolesDTO
    {
        public int UserCount { get; set; }
        public IEnumerable<AssignRolesUserDTO> Users { get; set; }
    }

    public class AssignRolesUserDTO
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}
