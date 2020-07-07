using MFS.SecurityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class AuthUserModel
    {
        public bool IsAuthenticated { get; set; }
        public dynamic FeatureList { get; set; }
        public ApplicationUser User { get; set; }
        public string BearerToken { get; set; }
    }
}
