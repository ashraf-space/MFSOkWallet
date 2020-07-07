using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models.Utility
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public int FeatureId { get; set; }
        public int IsViewPermitted { get; set; }
        public int IsAddPermitted { get; set; }
        public int IsEditPermitted { get; set; }
        public int IsDeletePermitted { get; set; }
        public int IsSecuredviewPermitted { get; set; }
        public int IsRegistrationPermitted { get; set; }
        public string CategoryName { get; set; }
        public string FeatureName { get; set; }
    }
}
