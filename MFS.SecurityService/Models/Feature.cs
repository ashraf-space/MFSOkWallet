using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public int CategoryId { get; set; }
        public int OrderNo { get; set; }
        public string Alias { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string IsRegistrationAllowed { get; set; }
    }
}
