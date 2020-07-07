using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int FailedAttempts { get; set; }
    }
}
