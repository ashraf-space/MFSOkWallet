using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
	public class UtilityDashboard
	{
		public string Utility { get; set; }
		public double Amount { get; set; }
	}
	public class UtilityDashboardView
	{
		public object BarChartForCurrent { get; set; }
		public object BarChartForCurrentMonth { get; set; }
		public object BarChartForTotal { get; set; }
	}
}
