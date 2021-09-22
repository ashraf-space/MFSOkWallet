using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.TransactionService.Models
{
    public class TblDisburseInvalidData
    {
        public string AcNo { get; set; }
        public string BranchCode { get; set; }
        public double Amount { get; set; }
        public string MakerId { get; set; }
        public DateTime? Entrydate { get; set; }
        public string Remarks { get; set; }
        public string BeneficiaryName { get; set; }
        public string CancelParticular { get; set; }
        public string Batchno { get; set; }
        public string CMStatus { get; set; }
        public int Sl { get; set; }
        public int OrganizationId { get; set; }

        public string TransNo { get; set; }
        public string ChkId { get; set; }
        public DateTime? ChkDate { get; set; }


        //not mapped
        public double TotalSum { get; set; }
        public string DisburseType { get; set; }

        public string  Name { get; set; }
    }
}
