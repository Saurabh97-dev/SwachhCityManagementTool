using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class ComplaintRequest
    {
        public string vendor_name { get; set; }
        public string access_key { get; set; }
        public string mobileNumber { get; set; }
        public string categoryId { get; set; }
        public string complaintLatitude { get; set; }
        public string complaintLongitude { get; set; }
        public string complaintLocation { get; set; }
        public string complaintLandmark { get; set; }
        public string fullName { get; set; }
        public string userLatitude { get; set; }
        public string userLongitude { get; set; }
        public string userLocation { get; set; }
        public string deviceOs { get; set; }
        public string file { get; set; }
    }
}
