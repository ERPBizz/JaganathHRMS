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
    public partial class wfHrEmpCtcMaster : System.Web.UI.Page
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
        public static string EmployeeSalaryGradeList()
        {
            clsMain objMain = new clsMain();
            DataTable dtSalaryGradeList = new DataTable();

            try
            {

                dtSalaryGradeList = objMain.dtFetchData("select ID,SalaryGradeName from tblHrSalaryGradeMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtSalaryGradeList);
        }


        [WebMethod]
        public static string EmployeeMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData("select EmpId,EmpName+' ('+EmpId+')' as EmpName from tblHrEmpMaster where EmpId not in(select EmpId from tblHrPayrollEmpCtcTransection)");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtEmpList);
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
        public static string FetchSalaryGradeDetails(string GradeId)
        {
            clsMain objMain = new clsMain();
            DataTable dtGradeDetails = new DataTable();
            SqlParameter[] objParam = new SqlParameter[1];


            objParam[0] = new SqlParameter("@SalaryGradeId", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(GradeId);


            dtGradeDetails = objMain.ExecuteStoreProcedure("procHrEmpSalaryGradeDetails", objParam);


            string json = JsonConvert.SerializeObject(dtGradeDetails, Formatting.None);
            return json;
        }


        [WebMethod]
        public static string FetchPTaxDetails(string amnt)
        {
            clsMain objMain = new clsMain();
            DataTable dtPTax = new DataTable();

            try
            {

                dtPTax = objMain.dtFetchData(@"select AmtValue from tblHrP_Tax where AmtFrom<="+ amnt + " and AmtTo>="+ amnt + "");
            }
            catch (Exception ex)
            {
                // return "";
            }

            string json = JsonConvert.SerializeObject(dtPTax, Formatting.None);
            return json;
        }

        [WebMethod]
        public static string AddEmployeeCTC(string EmpId, string SalaryGradeId, string SalaryType, string BasicRate,string DAPercent,string DAAmnt,
                                   string HRAPercent, string HRAAmnt, string SPLALPercent,string SPLALAmnt,string EDUALPercent,string EDUALAmnt,
                                   string MobileAllowancePercent,string MobileAllowanceAmnt,string LTAPercent,string LTAAmnt,string STIPPercent,
                                   string STIPAmnt,string BonusPercent,string BonusAmnt,string GratuityPercent,string GratuityAmnt,string TotalEarn,
                                   string PFEmployerPercent,string PFEmployerAmnt,string PFEmployeePercent,string PFEmployeeAmnt,string ESIEmployerPercent,
                                   string ESIEmployerAmnt,string ESIEmployeePercent,string ESIEmployeeAmnt,string CTCAmnt,string PTAmnt,string TDSPercent,
                                   string TDSAmnt,string TotalDeduct,string NetPayAmnt,string LoginUser, string TA_Perday, string MedicalAllowPerDay, 
                                   string UniformAllowPerMonth, string OtherAllownce)
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[42];


            objParam[0] = new SqlParameter("@Empid", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = EmpId;


            objParam[1] = new SqlParameter("@SalaryGradeId", SqlDbType.Int);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Convert.ToInt32(SalaryGradeId);


            objParam[2] = new SqlParameter("@SalaryType", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = SalaryType;


            objParam[3] = new SqlParameter("@BasicRate", SqlDbType.Decimal);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = Convert.ToDecimal(BasicRate);


            objParam[4] = new SqlParameter("@DAPercent", SqlDbType.Decimal);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = Convert.ToDecimal(DAPercent);

            objParam[5] = new SqlParameter("@DAAmnt", SqlDbType.Decimal);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = Convert.ToDecimal(DAAmnt);

            objParam[6] = new SqlParameter("@HRAPercent", SqlDbType.Decimal);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = Convert.ToDecimal(HRAPercent);


            objParam[7] = new SqlParameter("@HRAAmnt", SqlDbType.Decimal);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = Convert.ToDecimal(HRAAmnt);

            objParam[8] = new SqlParameter("@SPLALPercent", SqlDbType.Decimal);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = Convert.ToDecimal(SPLALPercent);

            objParam[9] = new SqlParameter("@SPLALAmnt", SqlDbType.Decimal);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = Convert.ToDecimal(SPLALAmnt);

            objParam[10] = new SqlParameter("@EDUALPercent", SqlDbType.Decimal);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = Convert.ToDecimal(EDUALPercent);

            objParam[11] = new SqlParameter("@EDUALAmnt", SqlDbType.Decimal);
            objParam[11].Direction = ParameterDirection.Input;
            objParam[11].Value = Convert.ToDecimal(EDUALAmnt);

            objParam[12] = new SqlParameter("@MobileAllowancePercent", SqlDbType.Decimal);
            objParam[12].Direction = ParameterDirection.Input;
            objParam[12].Value = Convert.ToDecimal(MobileAllowancePercent);

            objParam[13] = new SqlParameter("@MobileAllowanceAmnt", SqlDbType.Decimal);
            objParam[13].Direction = ParameterDirection.Input;
            objParam[13].Value = Convert.ToDecimal(MobileAllowanceAmnt);

            objParam[14] = new SqlParameter("@LTAPercent", SqlDbType.Decimal);
            objParam[14].Direction = ParameterDirection.Input;
            objParam[14].Value = Convert.ToDecimal(LTAPercent);

            objParam[15] = new SqlParameter("@LTAAmnt", SqlDbType.Decimal);
            objParam[15].Direction = ParameterDirection.Input;
            objParam[15].Value = Convert.ToDecimal(LTAAmnt);

            objParam[16] = new SqlParameter("@STIPPercent", SqlDbType.Decimal);
            objParam[16].Direction = ParameterDirection.Input;
            objParam[16].Value = Convert.ToDecimal(STIPPercent);

            objParam[17] = new SqlParameter("@STIPAmnt", SqlDbType.Decimal);
            objParam[17].Direction = ParameterDirection.Input;
            objParam[17].Value = Convert.ToDecimal(STIPAmnt);

            objParam[18] = new SqlParameter("@BonusPercent", SqlDbType.Decimal);
            objParam[18].Direction = ParameterDirection.Input;
            objParam[18].Value = Convert.ToDecimal(BonusPercent);

            objParam[19] = new SqlParameter("@BonusAmnt", SqlDbType.Decimal);
            objParam[19].Direction = ParameterDirection.Input;
            objParam[19].Value = Convert.ToDecimal(BonusAmnt);

            objParam[20] = new SqlParameter("@GratuityPercent", SqlDbType.Decimal);
            objParam[20].Direction = ParameterDirection.Input;
            objParam[20].Value = Convert.ToDecimal(GratuityPercent);

            objParam[21] = new SqlParameter("@GratuityAmnt", SqlDbType.Decimal);
            objParam[21].Direction = ParameterDirection.Input;
            objParam[21].Value = Convert.ToDecimal(GratuityAmnt);

            objParam[22] = new SqlParameter("@TotalEarn", SqlDbType.Decimal);
            objParam[22].Direction = ParameterDirection.Input;
            objParam[22].Value = Convert.ToDecimal(TotalEarn);

            objParam[23] = new SqlParameter("@PFEmployerPercent", SqlDbType.Decimal);
            objParam[23].Direction = ParameterDirection.Input;
            objParam[23].Value = Convert.ToDecimal(PFEmployerPercent);

            objParam[24] = new SqlParameter("@PFEmployerAmnt", SqlDbType.Decimal);
            objParam[24].Direction = ParameterDirection.Input;
            objParam[24].Value = Convert.ToDecimal(PFEmployerAmnt);

            objParam[25] = new SqlParameter("@PFEmployeePercent", SqlDbType.Decimal);
            objParam[25].Direction = ParameterDirection.Input;
            objParam[25].Value = Convert.ToDecimal(PFEmployeePercent);

            objParam[26] = new SqlParameter("@PFEmployeeAmnt", SqlDbType.Decimal);
            objParam[26].Direction = ParameterDirection.Input;
            objParam[26].Value = Convert.ToDecimal(PFEmployeeAmnt);

            objParam[27] = new SqlParameter("@ESIEmployerPercent", SqlDbType.Decimal);
            objParam[27].Direction = ParameterDirection.Input;
            objParam[27].Value = Convert.ToDecimal(ESIEmployerPercent);

            objParam[28] = new SqlParameter("@ESIEmployerAmnt", SqlDbType.Decimal);
            objParam[28].Direction = ParameterDirection.Input;
            objParam[28].Value = Convert.ToDecimal(ESIEmployerAmnt);

            objParam[29] = new SqlParameter("@ESIEmployeePercent", SqlDbType.Decimal);
            objParam[29].Direction = ParameterDirection.Input;
            objParam[29].Value = Convert.ToDecimal(ESIEmployeePercent);

            objParam[30] = new SqlParameter("@ESIEmployeeAmnt", SqlDbType.Decimal);
            objParam[30].Direction = ParameterDirection.Input;
            objParam[30].Value = Convert.ToDecimal(ESIEmployeeAmnt);

            objParam[31] = new SqlParameter("@CTCAmnt", SqlDbType.Decimal);
            objParam[31].Direction = ParameterDirection.Input;
            objParam[31].Value = Convert.ToDecimal(CTCAmnt);

            objParam[32] = new SqlParameter("@PTAmnt", SqlDbType.Decimal);
            objParam[32].Direction = ParameterDirection.Input;
            objParam[32].Value = Convert.ToDecimal(PTAmnt);

            objParam[33] = new SqlParameter("@TDSPercent", SqlDbType.Decimal);
            objParam[33].Direction = ParameterDirection.Input;
            objParam[33].Value = Convert.ToDecimal(TDSPercent);

            objParam[34] = new SqlParameter("@TDSAmnt", SqlDbType.Decimal);
            objParam[34].Direction = ParameterDirection.Input;
            objParam[34].Value = Convert.ToDecimal(TDSAmnt);

            objParam[35] = new SqlParameter("@TotalDeduct", SqlDbType.Decimal);
            objParam[35].Direction = ParameterDirection.Input;
            objParam[35].Value = Convert.ToDecimal(TotalDeduct);

            objParam[36] = new SqlParameter("@NetPayAmnt", SqlDbType.Decimal);
            objParam[36].Direction = ParameterDirection.Input;
            objParam[36].Value = Convert.ToDecimal(NetPayAmnt);

            objParam[37] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[37].Direction = ParameterDirection.Input;
            objParam[37].Value = LoginUser;

            objParam[38] = new SqlParameter("@TA_Perday", SqlDbType.Decimal);
            objParam[38].Direction = ParameterDirection.Input;
            objParam[38].Value = TA_Perday;

            objParam[39] = new SqlParameter("@MedicalAllowPerDay", SqlDbType.Decimal);
            objParam[39].Direction = ParameterDirection.Input;
            objParam[39].Value = MedicalAllowPerDay;

            objParam[40] = new SqlParameter("@UniformAllowPerMonth", SqlDbType.Decimal);
            objParam[40].Direction = ParameterDirection.Input;
            objParam[40].Value = UniformAllowPerMonth;

            objParam[41] = new SqlParameter("@OtherAllownce", SqlDbType.Decimal);
            objParam[41].Direction = ParameterDirection.Input;
            objParam[41].Value = OtherAllownce;

            var result = objMain.ExecuteProcedure("procHrEmpCTCInsert", objParam);


            return "";
        }



        [WebMethod]
        public static string FetchEmployeCTCList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpCTCList = new DataTable();

            try
            {

                dtEmpCTCList = objMain.dtFetchData(@"select c.*,s.SalaryGradeName,e.EmpName from tblHrPayrollEmpCtcTransection c
                                                            join tblHrSalaryGradeMaster s on s.ID=c.SalaryGradeId
                                                            join tblHrEmpMaster e on c.EmpId=e.EmpId
                                                            where c.Active='Y' order by c.Id desc
                                                            ");
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
            return JsonConvert.SerializeObject(dtEmpCTCList, settings);
        }

        [WebMethod]
        public static string FetchEmployeeCTCDetails(string id = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpCTCDetails = new DataTable();

            try
            {

                dtEmpCTCDetails = objMain.dtFetchData(@"select c.*,s.SalaryGradeName,e.EmpName,isnull(s.PF,'N') as AllowPF,
                                                    isnull(s.TDS,'N') as AllowTDS,cast(c.SalaryGradeId as int) as Salary_Grade_Id
                                                    from tblHrPayrollEmpCtcTransection c
                                                    join tblHrSalaryGradeMaster s on s.ID=c.SalaryGradeId
                                                    join tblHrEmpMaster e on c.EmpId=e.EmpId
                                                    where c.Id=" + id + "");
            }
            catch (Exception ex)
            {
                // return "";
            }


            string json = JsonConvert.SerializeObject(dtEmpCTCDetails, Formatting.None);
            return json;
        }



    }
}