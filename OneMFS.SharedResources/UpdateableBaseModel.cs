using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources
{
    public class UpdateableBaseModel
    {
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }        
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
