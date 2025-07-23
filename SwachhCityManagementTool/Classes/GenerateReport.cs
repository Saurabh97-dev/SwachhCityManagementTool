using ClosedXML.Excel;
using SwachhCityManagementTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public class GenerateReport
    {

        public static void GenarateEmployeeWiseREportExcel(List<EmpWiseReportModel> data)
        {
            if (data == null || !data.Any())
            {
                Console.WriteLine("No data available to generate the report.");
                return;
            }

            // Add total row
            var totalRow = new EmpWiseReportModel
            {
                Full_Name = "TOTAL",
                Total_Received_Complaints = data.Sum(x => x.Total_Received_Complaints),
                Total_On_Job_Complaints = data.Sum(x => x.Total_On_Job_Complaints),
                Total_Resolved_Complaints = data.Sum(x => x.Total_Resolved_Complaints),
                Total_Rejected_Complaints = data.Sum(x => x.Total_Rejected_Complaints),
            };
            data.Add(totalRow);

            // Define columns
            var _columnsConfig = new List<ColumnConfig<EmpWiseReportModel>>
            {
                new ColumnConfig<EmpWiseReportModel>
                {
                    Header = "S.No", Selector = x => 0, Width = 8,
                    StyleAction = (cell, value, row) =>
                    {
                        int index = data.IndexOf(row) + 1;
                        cell.Value = index;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                },
                new ColumnConfig<EmpWiseReportModel> { Header = "Employee Name", Selector = x => x.Full_Name, Width = 30 },
                new ColumnConfig<EmpWiseReportModel> { Header = "Mobile Number", Selector = x => x.Mobile_Number, Width = 20 },
                new ColumnConfig<EmpWiseReportModel> { Header = "Total Received Complaints", Selector = x => x.Total_Received_Complaints, Width = 25 },
                new ColumnConfig<EmpWiseReportModel> { Header = "Total On Job Complaints", Selector = x => x.Total_On_Job_Complaints, Width = 25 },
                new ColumnConfig<EmpWiseReportModel> { Header = "Total Resolved Complaints", Selector = x => x.Total_Resolved_Complaints, Width = 25 },
                new ColumnConfig<EmpWiseReportModel> { Header = "Total Rejected Complaints", Selector = x => x.Total_Rejected_Complaints, Width = 25 }
            };

            // Prepare file path on desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"EmployeeWiseReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string fullPath = Path.Combine(desktopPath, fileName);

            // Export using helper and save
            using (var stream = new MemoryStream())
            {
                ExcelHelper.ExportToExcel("Employee Wise Report", data, _columnsConfig, stream);
                File.WriteAllBytes(fullPath, stream.ToArray());
            }

            Console.WriteLine($"Excel report saved to: {fullPath}");
        }




        //public static void GenarateEmployeeWiseREportExcel(List<EmpWiseReportModel> data)
        //{


        //    if (data.Any())
        //    {
        //        var totalRow = new EmpWiseReportModel
        //        {
        //            Full_Name = "TOTAL",

        //            Total_Received_Complaints = data.Sum(x => x.Total_Received_Complaints),
        //            Total_On_Job_Complaints = data.Sum(x => x.Total_On_Job_Complaints),
        //            Total_Resolved_Complaints = data.Sum(x => x.Total_Resolved_Complaints),
        //            Total_Rejected_Complaints = data.Sum(x => x.Total_Rejected_Complaints),
    
        //        };

        //        data.Add(totalRow);
        //    }
        

        //var _columnsConfig = new List<ColumnConfig<EmpWiseReportModel>>
        //    {
        //        new ColumnConfig<EmpWiseReportModel> { Header = "S.No", Selector = x => 0, Width = 8,
        //         StyleAction = (cell, value, row) => {
        //                if (value is int d) {
        //                    cell.Value = d;
        //                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                }
        //            }
        //        },

        //        new ColumnConfig<EmpWiseReportModel> { Header = "Employee Name", Selector = x => x.Full_Name, Width = 80 },
        //        new ColumnConfig<EmpWiseReportModel> { Header = "Mobile Number", Selector = x => x.Mobile_Number, Width = 60, IsVisible = true },
        //        new ColumnConfig<EmpWiseReportModel> { Header = "Total_Received_Complaints", Selector = x => x.Total_Received_Complaints, Width = 20, IsVisible = true },
        //        new ColumnConfig<EmpWiseReportModel> { Header = "Total_On_Job_Complaints", Selector = x => x.Total_On_Job_Complaints, Width = 20, IsVisible = true },
        //        new ColumnConfig<EmpWiseReportModel> { Header = "Total_Resolved_Complaints", Selector = x => x.Total_Resolved_Complaints, Width = 20, IsVisible = true },
        //        new ColumnConfig<EmpWiseReportModel> { Header = "Total_Rejected_Complaints", Selector = x => x.Total_Rejected_Complaints, Width = 20, IsVisible = true },
        //        //new ColumnConfig<EmpWiseReportModel> { Header = "Format Type", Selector = x => x.FormatType, Width = 20 },
        //        //new ColumnConfig<EmpWiseReportModel> { Header = "Installation Type", Selector = x => x.InstallationType, Width = 20 },

        //        //new ColumnConfig<EmpWiseReportModel> { Header = "Sides", Selector = x => x.NumberOfSides, Width = 10, IsVisible = false,
        //        //    StyleAction = (cell, value, row) => {
        //        //            if (value is int d) {
        //        //                cell.Value = d;
        //        //                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //            }
        //        //        }
        //        //},

        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Width (ft)", Selector = x => x.WidthFt, Width = 12, IsVisible = false,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Height (ft)", Selector = x => x.HeightFt, Width = 12, IsVisible = false,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total Area (sqft)", Selector = x => x.TotalAreaSqFt, Width = 15, IsVisible = false,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "License Fee", Selector = x => x.LicenseFee, Width = 15, IsVisible = false,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Status", Selector = x => x.Status, Width = 12,
        //        //    StyleAction = (cell, value, row) => {
        //        //        var status = value?.ToString() ?? "";
        //        //        cell.Value = status;
        //        //        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        //        if (status == "EXPIRED")
        //        //            cell.Style.Fill.BackgroundColor = XLColor.LightPink;
        //        //        else if (status == "ACTIVE")
        //        //            cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Approval Date", Selector = x => x.ApprovalDate, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is DateTime dt)
        //        //            cell.Value = dt.ToString("dd/MM/yyyy");
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Valid Up To", Selector = x => x.ValidUpTo, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is DateTime dt)
        //        //            cell.Value = dt.ToString("dd/MM/yyyy");
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total EMI Amount", Selector = x => x.TotalEmiAmount, Width = 18,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total Penalty", Selector = x => x.TotalPenaltyAmount, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total Rebate", Selector = x => x.TotalRebateAmount, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total Due", Selector = x => x.TotalDueAmount, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Total Paid", Selector = x => x.TotalPaidAmount, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = "Balance Due", Selector = x => x.TotalBalanceDue, Width = 15,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

        //        //            if (d > 0) {
        //        //                cell.Style.Fill.BackgroundColor = XLColor.Yellow;
        //        //                cell.Style.Font.Bold = true;
        //        //            }
        //        //        }
        //        //    }
        //        //},
        //        //new ColumnConfig<EmpWiseReportModel> {
        //        //    Header = $"Due Till Date ({DateTime.Now.ToString("dd/MM/yyyy")})", Selector = x => x.TotalDueTillDate, Width = 18,
        //        //    StyleAction = (cell, value, row) => {
        //        //        if (value is decimal d) {
        //        //            cell.Value = d;
        //        //            cell.Style.NumberFormat.Format = "₹#,##0.00";
        //        //            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //        //        }
        //        //    }
        //        //}
        //    };


        //    using (var stream = new MemoryStream())
        //    {
        //        ExcelHelper.ExportToExcel("Swachh Gwalior App - Employee Wise Report", data, _columnsConfig, stream);
        //        stream.Position = 0;
        //        return File(stream.ToArray(),
        //            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //            $"OMD_Ledger_{DateTime.Now:dd_MM_yyyy_hh_mm_tt}.xlsx");
        //    }

        //}
    }
}
