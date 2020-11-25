using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class PasswordPolicy
    {
        public int PassMinLength { get; set; }
        public int PassMaxLength { get; set; }
        public string PassAlphaLower { get; set; }
        public string PassAlphaUpper { get; set; }
        public string PassNumber { get; set; }
        public string PassSpecialChar { get; set; }
        public int PassHistoryTake { get; set; }
    }
}
