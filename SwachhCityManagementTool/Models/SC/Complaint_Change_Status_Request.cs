using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class Complaint_Change_Status_Request
    {
        public int statusId; //StatusId;
        public int complaintId;
        public string deviceOs = string.Empty;
        public string vendor_name = string.Empty;
        public string access_key = string.Empty;
        public string apiKey = string.Empty;
        public string commentDescription = string.Empty;
        public string engineer_id = string.Empty;
    }
}
