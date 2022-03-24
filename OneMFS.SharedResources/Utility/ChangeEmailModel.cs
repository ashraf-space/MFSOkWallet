using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class ChangeEmailModel
    {
        public int ApplicationUserId { get; set; }
        public string EmployeeId { get; set; }
        public string EmailId { get; set; }
        public string ConfirmEmailId { get; set; }
    }
}
