using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class swch_chng_status_Result
    {
        public int httpCode { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public swch_chng_status_Complaint complaint { get; set; }
        public List<Error> errors { get; set; }
    }
    public class swch_chng_status_Complaint
    {
        public int id { get; set; }
    }
}
