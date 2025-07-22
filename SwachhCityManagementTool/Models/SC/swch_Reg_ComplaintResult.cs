using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SwachhCityManagementTool.Models.SC
{
    public class swch_Reg_ComplaintResult
    {
        public int httpCode { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public swch_Reg_Complaint complaint { get; set; }
        public List<Error> errors { get; set; }
    }
}
