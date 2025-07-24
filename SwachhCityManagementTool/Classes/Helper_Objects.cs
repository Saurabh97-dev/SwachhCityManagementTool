using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public class Helper_Objects
    {
    }

    public class ChangeStatus
    {
        public string complaintId { get; set; }
        public string comment { get; set; }
        public string status { get; set; }
    }
    public class AssignEngineer
    {
        public string complaint_id { get; set; }
        public string engineer_id { get; set; }
    }
    public class Filters
    {
        public string city { get; set; }
        public string status { get; set; }
        public string category { get; set; }
        public string ward { get; set; }
        public string term { get; set; }
        public string priority { get; set; }
        public string filterType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class FiltersObject
    {
        public Filters filters { get; set; }
    }

    public class WebComplaints
    {
        public string complaint_id { get; set; }
        public string generic_id { get; set; }
        public string location { get; set; }
        public string landmark { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string priority { get; set; }
        public string ward_id { get; set; }
        public string civic_agency_id { get; set; }
        public string agency_name { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public string user_id { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public string source_id { get; set; }
        public string source_name { get; set; }
        public string complaint_status_id { get; set; }
        public string statuses { get; set; }
        public string status_name { get; set; }
        public string created_at { get; set; }
    }
    public class User
    {
        public string password { get; set; }
        public string email { get; set; }
    }

    public class UserObject
    {
        public User user { get; set; }
    }
    public class Get_City_Object
    {
        public string id { get; set; }
        public string title { get; set; }
        public object slug { get; set; }
        public string district_id { get; set; }
        public string district_title { get; set; }
        public string state_id { get; set; }
        public string state_title { get; set; }
    }
    public class CheckSession
    {
        public string id { get; set; }
        public string city_id { get; set; }
        public string role_id { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public string civic_agency_id { get; set; }
        public string slug { get; set; }
    }
}
