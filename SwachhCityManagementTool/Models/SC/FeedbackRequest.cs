using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class FeedbackRequest
    {
        public string vendor_name { get; set; }
        public string access_key { get; set; }
        public string user_mobile_number { get; set; }
        public string feedback_option_id { get; set; }
        public string timestamp { get; set; }
        public string comment { get; set; }
    }
}
