using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ReportingService.Models
{
    public class TransactionAnalysis
    {
        public string TransactionType { get; set; }
        public string SubType { get; set; }
        public double VolumeOne { get; set; }
        public int CountOne { get; set; }
        public double PercentageOne { get; set; }
        public double VolumeTwo { get; set; }
        public int CountTwo { get; set; }
        public double PercentageTwo { get; set; }
        public double VolumeThree { get; set; }
        public int CountThree { get; set; }
        public double PercentageThree { get; set; }
        public double VolumeFour { get; set; }
        public int CountFour { get; set; }
        public double PercentageFour { get; set; }
        public double VolumeFive { get; set; }
        public int CountFive { get; set; }
        public double PercentageFive { get; set; }

        public string CaptionOne { get; set; }
        public string CaptionTwo { get; set; }
        public string CaptionThree { get; set; }
        public string CaptionFour { get; set; }
        public string CaptionFive { get; set; }
        public string WhereCondition { get; set; }
        public int Serial { get; set; }

        public int MonthDaysOne { get; set; }
        public int MonthDaysTwo { get; set; }
        public int MonthDaysThree { get; set; }
        public int MonthDaysFour { get; set; }
        public int MonthDaysFive { get; set; }
    }
}
