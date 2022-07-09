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
    public partial class wfHrEmpSalaryGrnerate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] != null)
            {
                loginuser.Value = Session["Id"].ToString();
                txtYear.Value = DateTime.Now.Year.ToString();

            }
            else
            {
                Response.Redirect("wfAdminLogin.aspx");
            }
        }




        [WebMethod]
        public static string AddEmployeeSalary(string inputType = "", string EmpId = "", string BranchId = "", string month = "", string year = "",
                                    string SalaryType = "", string LoginUser = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[7];


            objParam[0] = new SqlParameter("@inputType", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(inputType);


            objParam[1] = new SqlParameter("@branchid", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = BranchId;


            objParam[2] = new SqlParameter("@empid", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = EmpId;


            objParam[3] = new SqlParameter("@year", SqlDbType.Int);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = Convert.ToInt32(year);


            objParam[4] = new SqlParameter("@month", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = month;

            objParam[5] = new SqlParameter("@SalaryType", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = SalaryType;

            objParam[6] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = LoginUser;

            var result = objMain.ExecuteStoreProcedure("procHrEmpSalaryGrnerateInsert", objParam);

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



        [WebMethod]
        public static string FetchEmployeSalaryGenerateList(string branchid = "", string month = "", string year = "", string EmployeeId = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpSalaryGenerateList = new DataTable();

            try
            {
                if(year == "")
                {
                    year = DateTime.Now.Year.ToString();
                }
                dtEmpSalaryGenerateList = objMain.dtFetchData(@" select sg.*,e.EmpName
                                                        from tblHrPayrollMonthlySalaryGenerate sg 
                                                        join tblHrEmpMaster e on e.EmpId=sg.EmpId
                                                        where e.PresentStatus='Working' and e.Active='Y'
                                                        " + (branchid != "" ? " and e.Branchcode = '" + branchid + "'" : "") + "" + 
                                                         (year != "" ? " and sg.Year = " + year + "" : "") + "" +
                                                        (month != "" ? " and sg.Month = '" + month + "'" : "") + "" +
                                                        (EmployeeId != "" ? " and sg.EmpId='" + EmployeeId + "'" : "") + "" +

                                                        " order by sg.id desc");
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
            return JsonConvert.SerializeObject(dtEmpSalaryGenerateList, settings);
        }


        [WebMethod]
        public static string DeleteEmployeeSalary(string id = "")
        {
            clsMain objMain = new clsMain();

            try
            {

                objMain.dtFetchData(@"delete from tblHrPayrollMonthlySalaryGenerate where Id in ("+id+")");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject("");
        }



      

    }
}