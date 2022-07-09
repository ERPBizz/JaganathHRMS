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
    public partial class wfHrEmpAttendance : System.Web.UI.Page
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
        public static string EmployeeMasterList(string branchid="")
        {
            clsMain objMain = new clsMain();
            DataTable dtDepartmentList = new DataTable();

            try
            {

                dtDepartmentList = objMain.dtFetchData(@"select EmpId,EmpName+' ('+EmpId+')' as EmpName from tblHrEmpMaster where PresentStatus='Working' and Active='Y'
                    " + (branchid != "" ? " and Branchcode='" + branchid + "'" : ""));
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtDepartmentList);
        }



        [WebMethod]
        public static string BranchMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtBranchList = new DataTable();

            try
            {

                dtBranchList = objMain.dtFetchData("select [BranchCode],[BranchName] FROM [tblHrBranchMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtBranchList);
        }


        [WebMethod]
        public static string FetchEmployeeDetails(string EmpId = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData(@"select EmpId,EmpName,Branchcode,DOB,DOJ,PresentDesignation,PresentDepartId,Area,
                                              FatherName,MotherName,SpouseName,Division,Grade,PresentResNo,PresentResName,
                                              PresentRoadStreet,PresentPinNo,PresentPost,PresentState,PresentDistrict,
                                              PermanentResNo,PermanentResName,PermanentRoadStreet,PermanentPinNo,PermanentPost,
                                              PermanentState,PermanentDistrict,AdharNo,VoterNo,PanNo,Passport,DrivingNo,
                                              IfscCode,BankBranchName,BankName,AcNumber,PfNo,EsiNo,Sex,MaritalStatus,
                                              MobileNo,EmailAddress,Religion,Caste from tblHrEmpMaster where EmpId='" + EmpId + "'");
            }
            catch (Exception ex)
            {
                // return "";
            }

            string json = JsonConvert.SerializeObject(dtEmpList, Formatting.None);
            return json;
        }


        [WebMethod]
        public static string AddEmployeeAttendance(string AttendanceType="", string EmpId = "", string BranchId = "", string AttendanceDate = "", string AttendanceTime = "",
                                    string Attendance = "", string Reason = "", string LoginUser = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[8];


            objParam[0] = new SqlParameter("@AttendanceType", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = AttendanceType;


            objParam[1] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = EmpId;


            objParam[2] = new SqlParameter("@BranchId", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = BranchId;


            objParam[3] = new SqlParameter("@AttendanceDate", SqlDbType.DateTime);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = Convert.ToDateTime(AttendanceDate);


            objParam[4] = new SqlParameter("@AttendanceTime", SqlDbType.Time);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = DateTime.Parse(AttendanceTime).TimeOfDay;

            objParam[5] = new SqlParameter("@Attendance", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = Attendance;

            objParam[6] = new SqlParameter("@Reason", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = Reason;

            objParam[7] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = LoginUser;


            var result = objMain.ExecuteStoreProcedure("procHrEmpAttendanceInsertUpdate", objParam);

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
        public static string FetchEmployeAttendanceList(string AttendanceDate = "",string day="", string month="", string year="",string EmployeeId="")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpAttendanceList = new DataTable();

            try
            {
                if (AttendanceDate != "")
                {
                    day = Convert.ToDateTime(AttendanceDate).Day.ToString();
                    month = Convert.ToDateTime(AttendanceDate).ToString("MMMM").Substring(0, 3);
                    year = Convert.ToDateTime(AttendanceDate).Year.ToString();
                }
                dtEmpAttendanceList = objMain.dtFetchData(@" select ea.EmpId,e.EmpName,ea.AttYear,ea.AttMonth,ea.AttDay,
                                                        CONVERT(varchar(15),ea.AttTime,100) as AttTime,CONVERT(nvarchar,ea.AttEntryDate,106) as AttEntryDate,ea.Attendance,ea.Reason
                                                        from tblHrEmpAttendance ea 
                                                        join tblHrEmpMaster e on e.EmpId=ea.EmpId
                                                        where e.PresentStatus='Working' and e.Active='Y'
                                                        " + (EmployeeId != "" ? " and ea.EmpId='" + EmployeeId + "'" : "") + "" +
                                                         (year != "" ? " and ea.AttYear = " + year + "" : "") + "" +
                                                        (month != "" ? " and ea.AttMonth = '" + month + "'" : "") + "" +
                                                        (day != "" ? " and ea.AttDay = " + day + "" : "") + "" +
                                                        " order by ea.id desc");
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
            return JsonConvert.SerializeObject(dtEmpAttendanceList, settings);
        }

    }
}