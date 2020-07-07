using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class Permission
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public int FeatureId { get; set; }
        public string IsViewPermitted { get; set; }
        public string IsAddPermitted { get; set; }
        public string IsEditPermitted { get; set; }
        public string IsDeletePermitted { get; set; }
        public string IsSecuredviewPermitted { get; set; }
        public string IsRegistrationPermitted { get; set; }
    }
}
