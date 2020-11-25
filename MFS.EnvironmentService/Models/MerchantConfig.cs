using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.EnvironmentService.Models
{
    public class MerchantConfig
    {
        //public int ConfigId { get; set; }
        public string Sname { get; set; }
        public string Mphone { get; set; }
        public string Status { get; set; }
        //public string SemiconRevPhone { get; set; }
        public double CustomerServiceChargePer { get; set; }
		//public double CustomerServiceChargeFxt { get; set; }
		public double CustomerServiceChargeMin { get; set; } = 0;
		public double CustomerServiceChargeMax { get; set; } = 0;
		public string SchargeFormula { get; set; }
		//public string Ai { get; set; }
		//public string Bi { get; set; }
		public string Ci { get; set; }
		//public double MerchantCashoutCharge { get; set; }
		public double? MaxTransAmt { get; set; } = -1;
		public double? MinTransAmt { get; set; } = -1;
        //public double SemiconDbCharge { get; set; }
        public string PreFuncProcName { get; set; }
        public string PostFuncProcName { get; set; }
        public string MerchantSmsNotification { get; set; }
        //public string MerchantOutSlNo { get; set; }
        //public string Garbage { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Category { get; set; }
        public string Mcode { get; set; }
       
        public string Di { get; set; }
		public string _CompanyName { get; set; }
	}
}
