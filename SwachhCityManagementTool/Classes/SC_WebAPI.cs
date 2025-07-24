using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public static class SC_WebAPI
    {
        public static bool Loginstatus = false;
        public static CheckSession session_obj = new CheckSession();
        public static Get_City_Object get_city_object = new Get_City_Object();


        public static Dictionary<string, string> employees_dic = new Dictionary<string, string>();


        static Uri uri = new Uri("http://www.swachh.city");
        static CookieCollection collection = new CookieCollection();

        static SC_WebAPI()
        {

        }

        public static async Task<bool> Login(string email, string password)
        {
            bool result = false;
            User user = new User { email = email, password = password };
            UserObject user_obj = new UserObject();
            user_obj.user = user;

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"));
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpResponseMessage response = await client.GetAsync(uri);
            collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
            //handler.CookieContainer.Add(collection);
            using (var client12 = new HttpClient(handler) { BaseAddress = uri })
            {
                client12.Timeout = TimeSpan.FromMinutes(5);
                client12.DefaultRequestHeaders.Accept.Clear();
                client12.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client12.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client12.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                client12.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client12.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                // client12.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=UTF-8");
                client12.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client12.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/");
                client12.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                client12.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8,hi;q=0.7");
                HttpResponseMessage responseMessage = await client12.PostAsJsonAsync("Authenticate/ajax_login", user_obj);
                collection = handler.CookieContainer.GetCookies(uri);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    //{"status":"success","message":"Logged In Successfully","role_id":"2","redirect":"page"}
                    dynamic jobj = JObject.Parse(responseData);
                    if (jobj.status == "success")
                    {
                        Loginstatus = true;
                        result = true;
                    }

                }
                else
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    //result = JsonConvert.DeserializeObject<swch_Reg_ComplaintResult>(responseData);
                }
            }
            return result;
        }

        public async static Task<List<WebComplaints>> GetComplaints(int PageNo, string StatusId)
        {

            // 4 --> Resolved

            List<WebComplaints> complaintslist = new List<WebComplaints>();
            Filters filters = new Filters
            {
                city = "",
                status = StatusId,
                category = "",
                ward = "",
                term = "",
                priority = "",
                filterType = "",
                startDate = "2022-01-01",
                endDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now)
            };

            FiltersObject filters_object = new FiltersObject();
            filters_object.filters = filters;

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client13 = new HttpClient(handler) { BaseAddress = uri })
            {
                client13.Timeout = TimeSpan.FromMinutes(5);

                client13.DefaultRequestHeaders.Accept.Clear();
                client13.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client13.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client13.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client13.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/");
                //client12.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //client12.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client13.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client13.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = await client13.PostAsJsonAsync("complaint/lists/30/" + PageNo + "/id/DESC", filters_object);
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {

                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        dynamic jobj = JObject.Parse(responseData);

                        foreach (var item in jobj.result)
                        {
                            WebComplaints comp = new WebComplaints();
                            comp.complaint_id = item.complaint_id;
                            comp.generic_id = item.generic_id;
                            comp.location = item.location;
                            comp.landmark = item.landmark;
                            comp.latitude = item.latitude;
                            comp.longitude = item.longitude;
                            comp.priority = item.priority;
                            comp.ward_id = item.ward_id;
                            comp.civic_agency_id = item.civic_agency_id;
                            comp.agency_name = item.agency_name;
                            comp.category_id = item.category_id;
                            comp.category_name = item.category_name;
                            comp.user_id = item.user_id;
                            comp.city_id = item.city_id;
                            comp.city_name = item.city_name;
                            comp.source_id = item.source_id;
                            comp.source_name = item.source_name;
                            comp.complaint_status_id = item.complaint_status_id;
                            comp.statuses = item.statuses.ToString();
                            comp.status_name = item.status_name;
                            comp.created_at = item.created_at;
                            complaintslist.Add(comp);
                        }
                    }
                    else
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch { }
            }
            return complaintslist;
        }

        public static List<WebComplaints> GetAllComplaints(string StatusId)
        {
            List<WebComplaints> complaintslist = new List<WebComplaints>();

            for (int i = 1; i < 100; i++)
            {
                List<WebComplaints> tt = GetComplaints(i, StatusId).Result;
                if (tt.Any())
                {
                    complaintslist.AddRange(tt);
                }
            }

            return complaintslist;
        }

        public static string Get_Default_Page()
        {
            string result = "";
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/page")).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        result = "OK!";
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        result = "Error!";
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Get_City(out Get_City_Object city_obj)
        {
            city_obj = new Get_City_Object();

            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/settings/get_city")).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        dynamic jobj = JObject.Parse(result);
                        var jResult = JObject.Parse(result)["result"];

                        // Copy to a static Album instance
                        city_obj = jResult.ToObject<Get_City_Object>();
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Check_Session(out CheckSession session_obj)
        {
            session_obj = new CheckSession();

            string result = "";
            // CheckSession session_obj = new CheckSession();

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/Authenticate/check_session")).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        session_obj = JsonConvert.DeserializeObject<CheckSession>(result);
                        //dynamic jobj = JObject.Parse(result);

                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Get_City_Config(string CityID)
        {
            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/Settings/get_city_config/" + CityID)).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Categories_List(string CityID)
        {
            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/Settings/categories_list/" + CityID)).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Complaint_List_Page()
        {
            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/complaint/complaint_list")).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        result = "OK!";
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        result = "ERROR!";
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Get_Status()
        {
            string result = "";

            Filters filters = new Filters
            {
                city = "",
                status = "",
                category = "",
                ward = "",
                term = "",
                priority = "",
                filterType = "",
                startDate = "2017-01-16",
                endDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now)
            };

            FiltersObject filters_object = new FiltersObject();
            filters_object.filters = filters;

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {
                    HttpResponseMessage responseMessage = client.PostAsJsonAsync("complaint/get_status/30/1/id/DESC", filters_object).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static string Get_Engineers(string CityID, out Dictionary<string, string> employees)
        {
            employees = new Dictionary<string, string>();
            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                //clien.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {

                    HttpResponseMessage responseMessage = client.GetAsync(new Uri("http://www.swachh.city/user/get_engineers/" + CityID)).Result;
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                        dynamic jobj = JObject.Parse(result);
                        string status = jobj.status;
                        if (status == "success")
                        {
                            foreach (var item in jobj.result)
                            {

                                dynamic jobj2 = JObject.Parse("{" + item.ToString() + "}");
                                foreach (JProperty prop in jobj2.Properties())
                                {
                                    // Console.WriteLine(prop.Name);
                                    string employee_id = jobj2[prop.Name].employee_id;
                                    string employee_name = jobj2[prop.Name].employee_name;
                                    string mobile_number = jobj2[prop.Name].mobile_number;
                                    string employee_designation = jobj2[prop.Name].employee_designation;
                                    string city_name = jobj2[prop.Name].city_name;
                                    employees.Add(employee_id, employee_name + " (" + mobile_number + ") " + employee_designation + " - " + city_name);
                                }

                            }
                        }
                    }
                    else
                    {
                        result = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            return result;
        }

        public static async Task<string> ChangeStatus(ChangeStatus param)
        {
            // param.complaintId = "";

            string result = "";
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            handler.CookieContainer.Add(uri, new Cookie("_ga", "GA1.2.1650500235.1514120617"));
            handler.CookieContainer.Add(uri, new Cookie("_gid", "GA1.2.1635948102.1515769964"));
            handler.CookieContainer.Add(uri, new Cookie("ljs-lang", "en"));
            handler.CookieContainer.Add(uri, new Cookie("_pk_ref.1.779c", "%5B%22%22%2C%22%22%2C1515962905%2C%22https%3A%2F%2Fwww.google.co.in%2F%22%5D"));
            handler.CookieContainer.Add(uri, new Cookie("_pk_ses.1.779c", "*"));
            handler.CookieContainer.Add(uri, new Cookie("_pk_id.1.779c", "8f640cf9e9ed0114.1514120619.66.1515963084.1515962905."));

            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/page");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                try
                {

                    var jsonString = "{\"complaintId\":\"" + param.complaintId + "\",\"comment\":\"" + param.comment + "\",\"status\":\"" + param.status + "\"}";
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PutAsync("complaint/changestatus", httpContent);
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {

                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        dynamic jobj = JObject.Parse(responseData);
                        string msg = jobj.message;
                        string status = jobj.status;

                        result = param.complaintId + " - Status: " + status + " - Message: " + msg;
                    }
                    else
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        result = param.complaintId + "";
                    }
                }
                catch (Exception ex)
                {
                    result = param.complaintId + ex.Message;
                }
            }
            return result;
        }

        public static async Task<string> AssignToEngineer(AssignEngineer param)
        {
            string result = "";
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer.Add(collection);
            using (var client = new HttpClient(handler) { BaseAddress = uri })
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("csrf_test_name", collection["csrf_cookie_name"].Value);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.swachh.city/");
                client.DefaultRequestHeaders.Add("Origin", "http://www.swachh.city");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync("complaint/assign_engineer", param);
                    collection = handler.CookieContainer.GetCookies(uri); // Retrieving a Cookie
                    if (responseMessage.IsSuccessStatusCode)
                    {

                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        dynamic jobj = JObject.Parse(responseData);
                        string msg = jobj.message;
                        string status = jobj.status;

                        result = param.complaint_id + " - Status: " + status + " - Message: " + msg;
                    }
                    else
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {
                    result = param.complaint_id + ex.Message;
                }
            }
            return result;
        }


    }
}
