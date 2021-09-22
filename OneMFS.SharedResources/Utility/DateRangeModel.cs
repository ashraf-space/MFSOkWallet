using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{

    public class DateRangeModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
		public DateTime? FromDateNullable { get; set; }
		public DateTime? ToDateNullable { get; set; }
	}
}
