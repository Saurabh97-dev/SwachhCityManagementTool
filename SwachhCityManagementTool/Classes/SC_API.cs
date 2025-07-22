using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwachhCityManagementTool.Models.SC;

namespace SwachhCityManagementTool.Classes
{
    internal class SC_API
    {
        #region Variables
        public static string vendor_name = "";
        public static string access_key = "";
        public static string apiKey = "af4e61d75d2782a33eac7641e42bba6f";
        #endregion


        #region API Functions
        public static async Task<List<swch_Complaint>> Fetch_Complaints(int PageNo, string from, string to, string status, string category)
        {
            List<swch_Complaint> complaints = new List<swch_Complaint>();
            swch_ComplaintsResult result = new swch_ComplaintsResult();


            //PageNo = 1;
            //from = "2017-12-23";
            //to = "2017-12-30";
            //status = "";
            //category = "";

            string req = @"vendor_name=" + vendor_name
            + "&access_key=" + access_key
            + "&page=" + PageNo
            + "&status=" + status
            + "&category=" + category
            + "&from_date=" + from
            + "&to_date=" + to;

            string url = "http://api.swachh.city/sbm/v1/getComplaints?" + req;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.swachh.city/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<swch_ComplaintsResult>(responseData);
            }
            else
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<swch_ComplaintsResult>(responseData);
            }

            string errors = "";
            if (result.errors != null)
            {
                errors = "<br/> ERRORS : <br/> ";
                foreach (var item in result.errors)
                {
                    errors += item.field + " | " + item.code + " | " + item.message + "<br/>";
                }

            }
            complaints = result.complaints;

            return complaints;
        }

        public static async Task<string> Register_Complaint(ComplaintRequest comp)
        {
            swch_Reg_ComplaintResult result = new swch_Reg_ComplaintResult();
            try
            {
                string imageUrl = "http://192.168.10.3:85/default/kachra";
                HttpClient imgclient = new HttpClient();
                HttpResponseMessage response = await imgclient.GetAsync("http://cdn.gwaliormunicipalcorporation.org/default/kachra");
                if (response.IsSuccessStatusCode)
                {
                    imageUrl = await response.Content.ReadAsStringAsync();
                }

                //  comp.complaintPostedDate = String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.UtcNow);
                comp.file = imageUrl;
                // comp.categoryId = "5";Sweeping not done


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://api.swachh.city/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("sbm/v1/post-complaint", comp);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_Reg_ComplaintResult>(responseData);

                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_Reg_ComplaintResult>(responseData);
                }
            }
            catch (Exception ex)
            {
                result.httpCode = 500;
                result.code = 5000;
                result.message = "Invalid Responce!" + ex.Message;
            }

            string errors = "";
            if (result.errors != null)
            {
                errors = "<br/> ERRORS : <br/> ";
                foreach (var item in result.errors)
                {
                    errors += item.field + " | " + item.code + " | " + item.message + "<br/>";
                }

            }
            string compcode = "";
            if (result.complaint != null)
            {
                compcode = " Complaint Code: " + result.complaint.generic_id;
            }

            return "Http Code: " + result.httpCode + " Code: " + result.code + " Message: " + result.message + " | " + compcode + errors;
        }

        public static string Vote_on_Complaint(string complaintId, string mobileNo)
        {
            //int complaintId = 3594455;
            //string mobileNo = "9630455090";

            string req = @"vendor_name=" + vendor_name
                + "&access_key=" + access_key
                + "&mobileNumber=" + mobileNo
                + "&complaintId=" + complaintId
                + "&deviceToken=&deviceOs=external";


            HttpClient myRequest = new HttpClient("http://api.swachh.city/sbm/v1/post-voteup", "POST", req);
            string json = myRequest.GetResponse();

            swch_Basic_Result result = JsonConvert.DeserializeObject<swch_Basic_Result>(json);
            return "Http Code: " + result.httpCode + " Code: " + result.code + " Message: " + result.message;
        }

        public static string Comment_on_Complaint(string ComplaintId, string MobileNo, string Comment)
        {

            //int ComplaintId = 3586365;
            //string MobileNo = "9630455090";
            //string Comment = "this is a test Comment";

            string req = @"vendor_name=" + vendor_name
                + "&access_key=" + access_key
                + "&mobileNumber=" + MobileNo
                + "&complaintId=" + ComplaintId
                + "&commentDescription=" + Comment
                + "&deviceToken=&deviceOs=external";


            HttpClient myRequest = new HttpClient("http://api.swachh.city/sbm/v1/post-comment", "POST", req);
            string json = myRequest.GetResponse();

            swch_Basic_Result result = JsonConvert.DeserializeObject<swch_Basic_Result>(json);
            return "Http Code: " + result.httpCode + " Code: " + result.code + " Message: " + result.message;
        }

        public static async Task<string> User_Registration(string MobileNo)
        {
            swch_Basic_Result result = new swch_Basic_Result();

            User_Registration_Request user = new User_Registration_Request();
            user.vendor_name = vendor_name;
            user.access_key = access_key;
            user.mobileNumber = MobileNo;
            user.macAddress = MACAddressGenerator.GenerateMACAddress();//"74:E5:0B:31:AE:2A" 
            user.deviceToken = "";
            user.deviceOs = "external";
            user.apiKey = apiKey;
            user.lang = "en";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.swachh.city/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("sbm/v1/user", user);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                    //var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_Basic_Result>(responseData);

                }
                else
                {
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                    //var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_Basic_Result>(responseData);
                }
            }
            catch (Exception ex)
            {
                result.httpCode = 500;
                result.code = 5000;
                result.message = "Invalid Responce!" + ex.Message;
            }
            string errors = "";
            if (result.errors != null)
            {
                errors = "<br/> ERRORS : <br/> ";
                foreach (var item in result.errors)
                {
                    errors += item.field + " | " + item.code + " | " + item.message + "<br/>";
                }

            }

            return "Http Code: " + result.httpCode + " Mobile: " + MobileNo + " Message: " + result.message + "" + errors;
        }

        public static async Task<string> Change_Complaint_Status(int ComplaintId, string Comment, int StatusId)
        {
            swch_chng_status_Result result = new swch_chng_status_Result();

            //int ComplaintId = 3594455;
            //string Comment = "Complaint is Resolved!";
            //int StatusId = 3;

            Complaint_Change_Status_Request obj = new Complaint_Change_Status_Request();
            obj.statusId = 3; //StatusId;
            obj.complaintId = ComplaintId;
            obj.deviceOs = "external";
            obj.vendor_name = vendor_name;
            obj.access_key = access_key;
            obj.apiKey = apiKey;
            obj.commentDescription = Comment;
            obj.engineer_id = "1635871";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.swachh.city/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync("engineer/v1/complaint-status-update", obj);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_chng_status_Result>(responseData);

                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    result = JsonConvert.DeserializeObject<swch_chng_status_Result>(responseData);
                }
            }
            catch (Exception ex)
            {
                result.httpCode = 500;
                result.code = 5000;
                result.message = "Invalid Responce!" + ex.Message;
            }
            string errors = "";
            if (result.errors != null)
            {
                errors = "<br/> ERRORS : <br/> ";
                foreach (var item in result.errors)
                {
                    errors += item.field + " | " + item.code + " | " + item.message + "<br/>";
                }

            }

            return "Http Code: " + result.httpCode + " Code: " + result.code + " Message: " + result.message + "" + errors;
        }

        public static async Task<swch_Get_feedback_Result> Get_Feedback_Options(int complaintId)
        {
            swch_Get_feedback_Result result = new swch_Get_feedback_Result();
            //string ComplaintNo = "3594455";

            string req = @"vendor_name=" + vendor_name
            + "&access_key=" + access_key
            + "&complaint_id=" + complaintId;


            string url = "http://api.swachh.city/feedback/options?" + req;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.swachh.city/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<swch_Get_feedback_Result>(responseData);
            }
            else
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                // result = JsonConvert.DeserializeObject<swch_Get_feedback_Result>(responseData);
            }

            return result;

        }

        public static async Task<string> Update_Feedback(string mobile, int ComplaintId, int feedbackID)
        {
            string result = "";

            FeedbackRequest obj = new FeedbackRequest();
            obj.access_key = access_key;
            obj.vendor_name = vendor_name;
            obj.feedback_option_id = feedbackID.ToString();
            obj.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            obj.user_mobile_number = mobile;
            obj.comment = "Satisfied";
            //  obj.complaint_status_id = "4";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.swachh.city/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("complaint/" + ComplaintId + "/feedbacks", obj);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    dynamic jobj = JObject.Parse(responseData);
                    result = "Http_Code: " + jobj.http_code;
                    result += ", Message: " + jobj.message;

                    //if (jobj["data"] != null)
                    //{
                    //    result += ", Id: " + jobj.data[0].id;
                    //}


                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    dynamic jobj = JObject.Parse(responseData);
                    result = "Http_Code: " + jobj.http_code;
                    result += ", Message: " + jobj.message;


                    if (jobj["errors"] != null)
                    {
                        int Err_Count = jobj.errors.Count;
                        foreach (var item in jobj.errors)
                        {
                            result += "--- Error in Field Id: " + item.field_id + ", Message: " + item.message + " ---";
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                result = "Http_Code: " + 500;
                result += ", Message: " + "Invalid Responce! " + ex.Message;
            }


            return result;
        }




        #endregion
    }
}
