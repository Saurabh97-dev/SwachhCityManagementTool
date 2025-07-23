
using SwachhCityManagementTool.Classes;
using SwachhCityManagementTool.Models.SC;

namespace SwachhCityManagementTool;

internal class Program
{
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

        switch (Console.ReadLine())
        {
            case "1":
                var httpClient = new HttpClient();
                var summaryFetcher = new ComplaintSummaryFetcher(httpClient);
                await summaryFetcher.FetchAllSummariesAsync();
                List<long> complaintIds = summaryFetcher.ComplaintIds;
                var detailFetcher = new ComplaintDetailFetcher(httpClient);
                List<ComplaintDetail> detailedComplaints = await detailFetcher.FetchDetailsAsync(complaintIds);
                Console.WriteLine($"\n✅ Fetched {detailedComplaints.Count} detailed complaints.");

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

               // await CF_WEB.Auto_Post_CF_Web_Batch_Async(_pages, _post, _skip);
                Console.ReadLine();
                return true;
   

            case "3":
                Console.ReadLine();
                return true;
        
            case "100":
                return false;
            default:
                return true;
        }
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
