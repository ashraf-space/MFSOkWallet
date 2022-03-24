using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class ForgotPassReset
    {
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public string MobileNo { get; set; }
        public string OfficialEmail { get; set; }
    }
}
