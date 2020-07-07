using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class ChangePasswordModel
    {
        public int ApplicationUserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
