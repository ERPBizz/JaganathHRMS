using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace BizzManWebErp
{
    public partial class wfMmCategoryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] != null)
            {
                loginuser.Value = Session["Id"].ToString();

            }
            else
            {
                Response.Redirect("wfAdminLogin.aspx");
            }

        }
        [WebMethod]
        public static string FetchCategoryList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData(@"select Id,Name,Description from tblMmCategoryMaster");
            }
            catch (Exception ex)
            {
                // return "";
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays
            };
            return JsonConvert.SerializeObject(dtEmpList, settings);
        }




        

        [WebMethod]
        public static string AddCategory(string Name = "", string Description = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[2];


            objParam[0] = new SqlParameter("@Name", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Name;

            Debug.WriteLine("======================");
             Debug.WriteLine(Name);
             Debug.WriteLine("======================");


            objParam[1] = new SqlParameter("@Description", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Description;

            Debug.WriteLine("======================");
            Debug.WriteLine(Description);
            Debug.WriteLine("======================");


            var result = objMain.ExecuteProcedure("procMmCategoryMaster", objParam);


            return "";
        }

        [WebMethod]
        public static string CheckCategoryNameAvailability(string Name, string IsUpdate)
        {
            clsMain objMain = new clsMain();
            bool CheckName = new bool();

            try
            {

                if (IsUpdate == "0")
                {
                    CheckName = objMain.blSearchDataHO("select Name FROM [tblMmCategoryMaster] where Name='" + Name + "'");
                }
                else
                {
                    CheckName = false;
                }
            }
            catch (Exception ex)
            {
                return "False";
            }

            return JsonConvert.SerializeObject(CheckName.ToString());
        }


        [WebMethod]
        public static string FetchEmployeeDetails(string Name = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtCategoryList = new DataTable();

            try
            {

                dtCategoryList = objMain.dtFetchData(@"select Name,Description from tblMmCategoryMaster where Name='" + Name + "'");
            }
            catch (Exception ex)
            {
                // return "";
            }

            //var settings = new JsonSerializerSettings
            //{
            //    Formatting = Formatting.Indented,
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    PreserveReferencesHandling = PreserveReferencesHandling.Arrays
            //};

            //JavaScriptSerializer serializer = new JavaScriptSerializer();

            //serializer.MaxJsonLength = Int32.MaxValue;
            //return serializer.Serialize(dtEmpList); //JsonConvert.SerializeObject(dtEmpList, settings);

            string json = JsonConvert.SerializeObject(dtCategoryList, Formatting.None);
            return json;
        }

    }
}