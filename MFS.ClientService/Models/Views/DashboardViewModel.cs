using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models.Views
{
    public class DashboardViewModel
    {
        public object TotalClientCount { get; set; }
        public object ClientCountByMonth { get; set; }
        public object ClientCountByYear { get; set; }

        public object TotalTransaction { get; set; }
        public object TransactionByMonth { get; set; }
        public object TransactionByYear { get; set; }

        public object TotalCommunication { get; set; }
        public dynamic TotalAsset { get; internal set; }
        public dynamic PendingClientCount { get; internal set; }
    }
}
