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
    public partial class wfHrEmpSalaryApprove : System.Web.UI.Page
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
        public static string FetchEmployeSalaryApproveList(string branchid = "", string month = "", string year = "", string EmployeeId = "", string SalaryType = "", string SalaryApprove="")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpSalaryGenerateList = new DataTable();

            try
            {
                if (year == "")
                {
                    year = DateTime.Now.Year.ToString();
                }
                dtEmpSalaryGenerateList = objMain.dtFetchData(@" select sg.*,e.EmpName
                                                        from tblHrPayrollMonthlySalaryGenerate sg 
                                                        join tblHrEmpMaster e on e.EmpId=sg.EmpId
                                                        join tblHrPayrollEmpCtcTransection c on c.EmpId=sg.EmpId
                                                        where e.PresentStatus='Working' and e.Active='Y'
                                                        and sg.SalaryApprove='"+ SalaryApprove + "' and sg.SalaryPayment='N'" 
                                                        + (branchid != "" ? " and e.Branchcode = '" + branchid + "'" : "") + "" +
                                                         (year != "" ? " and sg.Year = " + year + "" : "") + "" +
                                                        (month != "" ? " and sg.Month = '" + month + "'" : "") + "" +
                                                        (EmployeeId != "" ? " and sg.EmpId='" + EmployeeId + "'" : "") + "" +
                                                        (SalaryType != "" ? " and c.SalaryType='" + SalaryType + "'" : "") + "" +
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
        public static string RejectSalaryApprove(string id = "", string loginuser="")
        {
            clsMain objMain = new clsMain();

            try
            {

                objMain.dtFetchData(@"update tblHrPayrollMonthlySalaryGenerate set SalaryApprove='N',UpdateUser='"+loginuser+ "',UpdateDate=getdate() where Id in (" + id + ")");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject("");
        }


        [WebMethod]
        public static string UpdateSalaryApprove(string id = "", string loginuser = "")
        {
            clsMain objMain = new clsMain();

            try
            {

                objMain.dtFetchData(@"update tblHrPayrollMonthlySalaryGenerate set SalaryApprove='Y',UpdateUser='" + loginuser + "',UpdateDate=getdate() where Id in (" + id + ")");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject("");
        }

        [WebMethod]
        public static string SalaryExportToPDF(string id = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpSalaryGenerateList = new DataTable();

            try
            {

                dtEmpSalaryGenerateList = objMain.dtFetchData(@" select e.EmpName,isnull(dpt.DeptName,'') as DeptName,isnull(dsg.DesignationName,'') as DesignationName,
                                                                isnull(e.CardNo,'') as CardNo,cast(sg.TotalDay as int) as TotalDay,cast(sg.EarnDay as int) as EarnDay,0 as LWP,
                                                                sg.BasicRate,sg.PF_EmployeesValue,sg.ESI_EmployeesValue,sg.PT,0 as Advance,
                                                                (sg.PF_EmployeesValue+sg.ESI_EmployeesValue+sg.PT) as TotalDeduct,sg.HraAmt,sg.SPLAL_Amt,
                                                                isnull(OtherAllownce,0) as OtherAllownce,(sg.HraAmt+sg.SPLAL_Amt+isnull(OtherAllownce,0)) as TotalAllow,
                                                                (sg.BasicRate+sg.HraAmt+sg.SPLAL_Amt+isnull(OtherAllownce,0)) as GrossSalary,
                                                                0 as Leave,sg.BonusAmt,(0+sg.BonusAmt) as AnualBenefit,
                                                                ((sg.BasicRate+sg.HraAmt+sg.SPLAL_Amt+isnull(OtherAllownce,0)+0+sg.BonusAmt)-(sg.PF_EmployeesValue+sg.ESI_EmployeesValue+sg.PT)) as NetAmnt,
                                                                sg.GROSS_TOTAL,(sg.GROSS_TOTAL * 12) as GROSS_TOTAL_PA,(sg.Month+' - '+cast(sg.year as varchar)) as MonthYear,sg.EmpId
                                                                from tblHrPayrollMonthlySalaryGenerate sg
                                                                join tblHrEmpMaster e on e.EmpId=sg.EmpId
                                                                left join tblHrDeptMaster dpt on dpt.Id=e.PresentDepartId
                                                                left join tblHrDesignationMaster dsg on dsg.Id=Cast(isnull(e.PresentDesignation,0) as int)
                                                                where sg.Id=" + id + "");
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
    }
}