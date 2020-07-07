using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.EnvironmentService.Models
{
    public class Bankbranch 
    {
        //public int Id { get; set; }

        public string Branchcode { get; set; }
        public string Branchname { get; set; }
        public string Routingno { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        //public string IsActive { get; set; }
    }
}
