using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public class ComplaintDetailFetcher
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public ComplaintDetailFetcher(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<ComplaintDetail>> FetchDetailsAsync(List<long> complaintIds)
        {
            var detailedComplaints = new List<ComplaintDetail>();
            var baseUrl = "https://api.swachh.city/sbm/v1/complaint?id=";

            foreach (var id in complaintIds)
            {
                try
                {
                    var response = await _client.GetStreamAsync($"{baseUrl}{id}");
                    var detail = await JsonSerializer.DeserializeAsync<ComplaintDetailResponse>(response, _options);

                    if (detail?.Complaint != null)
                    {
                        detailedComplaints.Add(detail.Complaint);
                        Console.WriteLine($"✔ Detail fetched for ID {id}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠ No detail for ID {id}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error fetching detail for ID {id}: {ex.Message}");
                }
            }

            return detailedComplaints;
        }
    }

    public class ComplaintDetailResponse
    {
        public int HttpCode { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public ComplaintDetail Complaint { get; set; }
    }

    public class ComplaintDetail
    {
        public long Id { get; set; }
        public string Generic_Id { get; set; }
        public int City_Id { get; set; }
        public long User_Id { get; set; }
        public int Category_Id { get; set; }
        public string Location { get; set; }
        public string Landmark { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category_Name { get; set; }
        public int Parent_Id { get; set; }
        public string Full_Name { get; set; }
        public long Mobile_Number { get; set; }
        public int Complaint_Status_Id { get; set; }
        public string Complaint_Status { get; set; }
        public string Complaint_Image { get; set; }
        public string User_Image { get; set; }
        public int Comment_Count { get; set; }
        public int Vote_Up_Count { get; set; }
        public string Event_Id { get; set; }
        public string Er_User_Id { get; set; }
        public int Is_Resolution_Accepted { get; set; }
        public int Is_Acknowledged { get; set; }
        public int Radius { get; set; }
        public string Posted_On { get; set; }
        public string User_Image_L1 { get; set; }
        public string Complaint_Image_L1 { get; set; }
        public string Complaint_Image_L2 { get; set; }
        public int Complaint_Image_Height { get; set; }
        public int Affected { get; set; }
        public string Complaint_Url { get; set; }
        public int Session_Lang_Id { get; set; }
        public string Engg_Name { get; set; }
        public long Engg_Mobile_Number { get; set; }
        public List<Comment> Comments { get; set; }
        public List<object> Voted_Up_Users { get; set; }
        public FeedbackCount Feedback_Count { get; set; }
    }

    public class Comment
    {
        public long Id { get; set; }
        public long User_Id { get; set; }
        public int Comment_Type_Id { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public string Posted_On { get; set; }
        public string Complaint_Status { get; set; }
        public JsonElement Complaint_Status_Id { get; set; }
        public string Comment_Image_Url { get; set; }
        public string Civic_Agency { get; set; }
        public string Feedback_Option { get; set; }
        public string User_Image_Url { get; set; }
    }

    public class FeedbackCount
    {
        public int Satisfaction { get; set; }
        public int Neutral { get; set; }
        public int Un_Satisfied { get; set; }
    }

}
