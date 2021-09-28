﻿using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class DisbursementUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PlainId { get; set; }
        public string PlainPassword { get; set; }
        public string Sha1Password { get; set; }
        public string Md5Password { get; set; }
        public string EmployeeId { get; set; }
        public string Ustatus { get; set; }
        public string Pstatus { get; set; }
        public string BranchCode { get; set; }
        public DateTime? LockDt { get; set; }
        public string CreatedBy { get; set; }
        public string UserSesId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int TranAmtLimit { get; set; }

        public bool Is_validated { get; set; }
        
		public string LogInStatus { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int  CompanyId { get; set; }
        public string Company_Name { get; set; }
        public double Bala_nce { get; set; }

    }
}