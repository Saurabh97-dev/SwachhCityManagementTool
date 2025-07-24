
using SwachhCityManagementTool.Classes;
using SwachhCityManagementTool.Models;
using SwachhCityManagementTool.Models.SC;
using System.Net.Http;

namespace SwachhCityManagementTool;

internal class Program
{
    private static readonly HttpClient httpClient = new HttpClient();
    static async Task Main(string[] args)
    {
        // Set the console's output encoding to UTF-8
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool showMenu = true;
        while (showMenu)
        {
            showMenu = await MainMenu();
        }
    }

    private static async Task<bool> MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Choose an option: Swachh Gwalior App");
        Console.WriteLine("1) Auto Assigned Complaints");
        Console.WriteLine("2) CF WEB (Batch)");
        Console.WriteLine("3) CF API (Get Questions)");
        Console.WriteLine("4) CF API (Post Feedback)");
        Console.WriteLine("-------- Reports ----------");
        Console.WriteLine("5) Today's Status Report");
        Console.WriteLine("6) Weekly Status Report");
        Console.WriteLine("7) Monthly Status Report");

        Console.WriteLine("15) Import Excel");
        Console.WriteLine("100) Exit");
        Console.Write("\r\nSelect an option: ");

        string selectedOption = Console.ReadLine();
        switch (selectedOption)
        {
            case "1":                

                return true;
            case "2":
                Console.WriteLine("Please enter no of pages:");
                string pages = Console.ReadLine();
                int _pages = Convert.ToInt32(pages);

                Console.WriteLine("Please enter no of post per page: 5 to 15");
                string Post = Console.ReadLine();
                int _post = Convert.ToInt32(Post);

                Console.WriteLine("Please enter no of records to SKIP: Unique for all Machines");
                string Skip = Console.ReadLine();
                int _skip = Convert.ToInt32(Skip);

                Console.ReadLine();
                return true;
   

            case "3":
                Console.ReadLine();
                return true;

            case "5":
            case "6":
            case "7":
                var timeRange = selectedOption switch
                {
                    "5" => TimeRange.Today,
                    "6" => TimeRange.Weekly,
                    "7" => TimeRange.Monthly,
                    _ => TimeRange.Today
                };
                await GenerateStatusReport(timeRange);
                return true;

            case "100":
                return false;
            default:
                return true;
        }
    }
    private static async Task GenerateStatusReport(TimeRange timeRange)
    {
        DateTime toDate = DateTime.Today;
        DateTime fromDate = GetFromDateBasedOnTimeRange(toDate, timeRange);

        var summaryFetcher = new ComplaintSummaryFetcher(httpClient, fromDate, toDate);
        await summaryFetcher.FetchAllSummariesAsync();
        List<long> complaintIds = summaryFetcher.ComplaintIds;
        var detailFetcher = new ComplaintDetailFetcher(httpClient);
        List<ComplaintDetail> detailedComplaints = await detailFetcher.FetchDetailsAsync(complaintIds);

        List<EmpWiseReportModel> ReportData = ReportHelper.Convert_To_EmpWiseReportModel(detailedComplaints);
        GenerateReport.GenarateEmployeeWiseREportExcel(ReportData);

        Console.WriteLine($"\n✅ Fetched {detailedComplaints.Count} detailed complaints for {timeRange} report ({fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}).");
    }


    private static DateTime GetFromDateBasedOnTimeRange(DateTime toDate, TimeRange timeRange)
    {
        return timeRange switch
        {
            TimeRange.Today => toDate,
            TimeRange.Weekly => toDate.AddDays(-6), 
            TimeRange.Monthly => toDate.AddMonths(-1).AddDays(1),
            _ => toDate
        };
    }

    public enum TimeRange
    {
        Today,
        Weekly,
        Monthly
    }
    public class ComplaintResponse
    {
        public int httpCode { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public List<Complaint> complaints { get; set; }
    }

    public class Complaint
    {
        public long complaintId { get; set; }
        public string complaintGenericId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string complaintLocation { get; set; }
        public string landmark { get; set; }
        public string title { get; set; }
        public long mobile_number { get; set; }
        public string full_name { get; set; }
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
