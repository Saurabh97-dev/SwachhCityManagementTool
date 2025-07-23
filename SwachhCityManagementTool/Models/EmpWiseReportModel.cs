using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models
{
    public class EmpWiseReportModel
    {
        public string Full_Name { get; set; }
        public long Mobile_Number { get; set; }
        public int Total_Received_Complaints { get; set; }
        public int Total_On_Job_Complaints { get; set; }
        public int Total_Resolved_Complaints { get; set; }
        public int Total_Rejected_Complaints { get; set; }

    }
}
