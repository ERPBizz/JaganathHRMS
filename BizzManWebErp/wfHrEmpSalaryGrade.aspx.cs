using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace BizzManWebErp
{
    public partial class wfHrEmpSalaryGrade : System.Web.UI.Page
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
        public static string FetchEmployeSalaryGradeList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpSalaryGradeList = new DataTable();

            try
            {

                dtEmpSalaryGradeList = objMain.dtFetchData(@"select * from tblHrSalaryGradeMaster order by id desc");
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
            return JsonConvert.SerializeObject(dtEmpSalaryGradeList, settings);
        }




        [WebMethod]
        public static string FetchEmployeeSalaryGradeDetails(string id = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpSalaryGradeList = new DataTable();

            try
            {

                dtEmpSalaryGradeList = objMain.dtFetchData(@"select * from tblHrSalaryGradeMaster where ID=" + id + "");
            }
            catch (Exception ex)
            {
                // return "";
            }

            
            string json = JsonConvert.SerializeObject(dtEmpSalaryGradeList, Formatting.None);
            return json;
        }

        [WebMethod]
        public static string CheckSalaryGradeNameAvailability(string id, string gradename)
        {
            clsMain objMain = new clsMain();
            bool CheckSalaryGrade = new bool();

            try
            {

                if (id.Trim() == "")
                {
                    CheckSalaryGrade = objMain.blSearchDataHO("select ID FROM tblHrSalaryGradeMaster where SalaryGradeName='" + gradename.Trim() + "'");
                }
                else
                {
                    CheckSalaryGrade = false;
                }
            }
            catch (Exception ex)
            {
                return "False";
            }

            return JsonConvert.SerializeObject(CheckSalaryGrade.ToString());
        }


        [WebMethod]
        public static string AddEmployeeSalaryGrade(string id = "", string gradename = "", string basic = "", string DA = "", string HRA = "",
                                   string Conveyance = "", string Medical = "", string PF = "", string ESI = "", string PTax = "", string TDS = "",
                                   string CL = "", string EL = "", string ML = "",string Description="", string LoginUser = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[16];


            objParam[0] = new SqlParameter("@id", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = (!string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0);


            objParam[1] = new SqlParameter("@gradename", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = gradename;


            objParam[2] = new SqlParameter("@basic", SqlDbType.Decimal);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = Convert.ToDecimal(basic);


            objParam[3] = new SqlParameter("@DA", SqlDbType.Decimal);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = (!string.IsNullOrEmpty(DA) ?Convert.ToDecimal(DA) :0);


            objParam[4] = new SqlParameter("@HRA", SqlDbType.Decimal);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = (!string.IsNullOrEmpty(HRA) ? Convert.ToDecimal(HRA) : 0);

            objParam[5] = new SqlParameter("@Conveyance", SqlDbType.Decimal);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = (!string.IsNullOrEmpty(Conveyance) ? Convert.ToDecimal(Conveyance) : 0);

            objParam[6] = new SqlParameter("@Medical", SqlDbType.Decimal);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = (!string.IsNullOrEmpty(Medical) ? Convert.ToDecimal(Medical) : 0);

            objParam[7] = new SqlParameter("@PF", SqlDbType.NVarChar);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = PF;

            objParam[8] = new SqlParameter("@ESI", SqlDbType.NVarChar);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = ESI;

            objParam[9] = new SqlParameter("@PTax", SqlDbType.NVarChar);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = PTax;

            objParam[10] = new SqlParameter("@TDS", SqlDbType.NVarChar);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = TDS;

            objParam[11] = new SqlParameter("@CL", SqlDbType.NVarChar);
            objParam[11].Direction = ParameterDirection.Input;
            objParam[11].Value = CL;

            objParam[12] = new SqlParameter("@EL", SqlDbType.NVarChar);
            objParam[12].Direction = ParameterDirection.Input;
            objParam[12].Value = EL;

            objParam[13] = new SqlParameter("@ML", SqlDbType.NVarChar);
            objParam[13].Direction = ParameterDirection.Input;
            objParam[13].Value = ML;

            objParam[14] = new SqlParameter("@Description", SqlDbType.NVarChar);
            objParam[14].Direction = ParameterDirection.Input;
            objParam[14].Value = Description;

            objParam[15] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[15].Direction = ParameterDirection.Input;
            objParam[15].Value = LoginUser;


            var result = objMain.ExecuteStoreProcedure("procHrEmpSalaryGradeInsertUpdate", objParam);

            string json = "";
            if (result != null)
            {
                if (result.Rows.Count > 0)
                {
                    json = result.Rows[0][0].ToString();
                }
            }


            return json;
        }


    }
}