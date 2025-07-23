using DocumentFormat.OpenXml.Spreadsheet;
using SwachhCityManagementTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public static class ReportHelper
    {

        public static List<EmpWiseReportModel> Convert_To_EmpWiseReportModel(List<ComplaintDetail> detailedComplaints)
        {
            var groupedData = detailedComplaints
                .Where(c => !string.IsNullOrEmpty(c.Engg_Name)) // Ensure engineer name is not null
                .GroupBy(c => new { c.Engg_Name, c.Engg_Mobile_Number })
                .Select(g => new EmpWiseReportModel
                {
                    Full_Name = g.Key.Engg_Name,
                    Mobile_Number = g.Key.Engg_Mobile_Number,
                    Total_Received_Complaints = g.Count(), // Total complaints received
                    Total_On_Job_Complaints = g.Count(c => c.Complaint_Status_Id == 3),
                    Total_Resolved_Complaints = g.Count(c => c.Complaint_Status_Id == 4),
                    Total_Rejected_Complaints = g.Count(c => c.Complaint_Status_Id == 6)
                })
                .ToList();

            return groupedData;
        }

    }
}
