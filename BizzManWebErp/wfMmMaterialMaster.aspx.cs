using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace BizzManWebErp
{
    public partial class wfMmMaterialMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] != null)
            {
                loginuser.Value = Convert.ToString(Session["Id"]);

            }
            else
            {
                Response.Redirect("wfAdminLogin.aspx");
            }
        }

        [WebMethod]
        public static string MaterialMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtBranchList = new DataTable();

            try
            {

                dtBranchList = objMain.dtFetchData("select Id,Name FROM tblMmCategoryMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtBranchList);
        }

        [WebMethod]
        public static string UnitMesureList()
        {
            clsMain objMain = new clsMain();
            DataTable dtUnitList = new DataTable();

            try
            {

                dtUnitList = objMain.dtFetchData("select Id,UnitMesureName FROM tblFaUnitMesureMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtUnitList);
        }

        [WebMethod]
        public static string FetchMaterialDetails(string MaterialName = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialList = new DataTable();

            try
            {

                dtMaterialList = objMain.dtFetchData(@"select MaterialCategoryId,MaterialName,UnitMesure,Description from tblMmMaterialMaster where MaterialName='" + MaterialName + "'");
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

            string json = JsonConvert.SerializeObject(dtMaterialList, Formatting.None);
            return json;
        }

        [WebMethod]
        public static string CheckMaterialAvailability(string MaterialName, string isUpdate)
        {
            clsMain objMain = new clsMain();
            bool checkId = new bool();

            try
            {

                if (isUpdate == "0")
                {
                    checkId = objMain.blSearchDataHO(string.Format("select 1 from tblMmMaterialMaster where MaterialName='{0}'", MaterialName));
                }
                else
                {
                    checkId = false;
                }
            }
            catch (Exception ex)
            {
                return "False";
            }

            return JsonConvert.SerializeObject(checkId.ToString());
        }

        [WebMethod]
        public static string FetchMaterialList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData(@"select e.Id,e.MaterialCategoryId,e.MaterialName,e.Description,a.UnitMesureName,br.Name
                                                from tblMmMaterialMaster e
                                                inner join tblFaUnitMesureMaster a on e.UnitMesure=a.Id
                                                inner join tblMmCategoryMaster br on e.MaterialCategoryId=br.Id");

                // dtEmpList = objMain.dtFetchData(@"select Id,MaterialCategoryId,MaterialName,UnitMesure,Description from tblMmMaterialMaster");
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
        public static string AddDetails(string MaterialCategoryId, string MaterialName, string UnitMesure, string Description, string loginUser)
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[6];

            objParam[0] = new SqlParameter("@MaterialCategoryId", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(MaterialCategoryId);

            objParam[1] = new SqlParameter("@MaterialName", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = MaterialName;

            objParam[2] = new SqlParameter("@UnitMesure", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = UnitMesure;


            objParam[3] = new SqlParameter("@Description", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = Description;

            objParam[4] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = loginUser;

            objParam[5] = new SqlParameter("@UpdateUser", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = loginUser;

            var result = objMain.ExecuteProcedure("procMmMaterialMaster", objParam);

            return "";
        }

    }
}