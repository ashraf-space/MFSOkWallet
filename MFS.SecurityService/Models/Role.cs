using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Discriminator { get; set; }
    }
}
