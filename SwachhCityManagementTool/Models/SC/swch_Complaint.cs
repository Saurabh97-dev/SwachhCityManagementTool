using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class swch_Complaint
    {
        public int complaintId { get; set; }
        public string complaintGenericId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string complaintLocation { get; set; }
        public object landmark { get; set; }
        public string title { get; set; }
        public object mobile_number { get; set; }
        public string full_name { get; set; }
        public string userLocation { get; set; }
        public string district { get; set; }
        public string ulb { get; set; }
        public int cityId { get; set; }
        public int wardId { get; set; }
        public int wardNo { get; set; }
        public string wardName { get; set; }
        public string complaint_image { get; set; }
        public string city { get; set; }
        public int status { get; set; }
        public string created_at { get; set; }
    }
}
