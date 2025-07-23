using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SwachhCityManagementTool.Program;

namespace SwachhCityManagementTool.Classes
{
    public class ComplaintSummaryFetcher
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public List<long> ComplaintIds { get; private set; } = new List<long>();

        public ComplaintSummaryFetcher(HttpClient client, DateTime fromDate, DateTime toDate)
        {
            _client = client;
            _fromDate = fromDate;
            _toDate = toDate;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task FetchAllSummariesAsync()
        {
            var baseUrl = $"https://api.swachh.city/sbm/v1/getComplaints?vendor_name=gwalior&access_key=7tcozS5ax&from_date={_fromDate:yyyy-MM-dd}&to_date={_toDate:yyyy-MM-dd}";
            int page = 1;

            while (true)
            {
                var url = $"{baseUrl}&page={page}";
                try
                {
                    var response = await _client.GetStreamAsync(url);
                    var summary = await JsonSerializer.DeserializeAsync<ComplaintResponse>(response, _options);

                    if (summary?.Complaints == null || summary.Complaints.Count == 0)
                        break;

                    foreach (var item in summary.Complaints)
                        ComplaintIds.Add(item.ComplaintId);

                    Console.WriteLine($"📄 Page {page}: {summary.Complaints.Count} complaints");
                    page++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error on page {page}: {ex.Message}");
                    break;
                }
            }

            Console.WriteLine($"\n📌 Total complaints (summary) fetched: {ComplaintIds.Count}\n");
        }
    }

    public class ComplaintResponse
    {
        public int HttpCode { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ComplaintSummary> Complaints { get; set; }
    }

    public class ComplaintSummary
    {
        public long ComplaintId { get; set; }
        public string ComplaintGenericId { get; set; }
    }
}
