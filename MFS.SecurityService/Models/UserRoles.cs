using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Discriminator { get; set; }
    }
}
