using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.DistributionService.Models
{
    public class DormantAcc
    {
        public string Mphone { get; set; }
        public string CatId { get; set; }
        public DateTime? Date { get; set; }
		public string _ActionBy { get; set; }
    }
}
