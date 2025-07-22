

namespace SwachhCityManagementTool.Models.SC
{
    public class swch_ComplaintsResult
    {
        public int httpCode { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public List<swch_Complaint> complaints { get; set; }
        public List<Error> errors { get; set; }
    }
    public class Error
    {
        public int code { get; set; }
        public string field { get; set; }
        public string message { get; set; }
    }
}
