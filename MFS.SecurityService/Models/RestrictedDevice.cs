using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class RestrictedDevice
    {
        public string Identifier { get; set; }
        public DateTime DateLocked { get; set; }
        public string Remarks { get; set; }
    }
}
