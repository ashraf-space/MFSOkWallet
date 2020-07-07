using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources.Utility
{
    public class BatchUpdateModel
    {
        public string Param { get; set; }
        public IEnumerable<dynamic> List { get; set; }
    }
}
