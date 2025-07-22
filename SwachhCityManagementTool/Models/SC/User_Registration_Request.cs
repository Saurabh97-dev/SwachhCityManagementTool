using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    internal class User_Registration_Request
    {
        public string vendor_name = string.Empty;
        public string access_key = string.Empty;
        public string mobileNumber = string.Empty;
        public string macAddress = string.Empty;//"74:E5:0B:31:AE:2A" 
        public string deviceToken = string.Empty;
        public string deviceOs = string.Empty;
        public string apiKey = string.Empty;
        public string lang = string.Empty;
    }
}
