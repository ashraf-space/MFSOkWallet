using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblDisburseCompanyInfo
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SalAcc { get; set; }
        public string RemAcc { get; set; }
        public string CabAcc { get; set; }
        public string CatAcc { get; set; }
        public string RwdAcc { get; set; }
        public string IncAcc { get; set; }
        public string EftAcc { get; set; }
        public string TargetCatId { get; set; }



        //for non mapped purpose
        public double bala_nce { get; set; }
        public string entry_user { get; set; }
        public string enterprize_AccCode { get; set; }
    }
}
