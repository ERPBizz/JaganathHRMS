using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BizzManWebErp.Model;
using Newtonsoft.Json;

namespace BizzManWebErp
{
    public partial class wfHrEmpMaster : System.Web.UI.Page
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
        public static string SalaryGradeMasterList()
        {


            clsMain objMain = new clsMain();
            DataTable dtSalaryGradeList = new DataTable();

            try
            {
                
                dtSalaryGradeList = objMain.dtFetchData("select [ID],[SalaryGradeName] FROM [tblHrSalaryGradeMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtSalaryGradeList);
        }

        [WebMethod]
        public static string DepartmentMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtDepartmentList = new DataTable();

            try
            {

                dtDepartmentList = objMain.dtFetchData("select [Id],[DeptName] FROM [tblHrDeptMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtDepartmentList);
        }

        [WebMethod]
        public static string DivisionMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtDivisionList = new DataTable();

            try
            {

                dtDivisionList = objMain.dtFetchData("select [ID],[DivisionName] FROM [tblHrDivisionMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtDivisionList);
        }

        [WebMethod]
        public static string DesignationMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtDesignationList = new DataTable();

            try
            {

                dtDesignationList = objMain.dtFetchData("select [ID],[DesignationName] FROM [tblHrDesignationMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtDesignationList);
        }

        [WebMethod]
        public static string GradeMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtGradeList = new DataTable();

            try
            {

                dtGradeList = objMain.dtFetchData("select [ID],[GradeName] FROM [tblHrGradeMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtGradeList);
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
        public static string AreaMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtAreaList = new DataTable();

            try
            {

                dtAreaList = objMain.dtFetchData("select [Id],[AreaName] FROM [tblHrAreaMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtAreaList);
        }

        [WebMethod]
        public static string CheckEmployeeIdAvailability(string EmpId, string IsUpdate)
        {
            clsMain objMain = new clsMain();
            bool CheckEmpId = new bool();

            try
            {

                if (IsUpdate == "0")
                {
                    CheckEmpId = objMain.blSearchDataHO("select EmpId FROM [tblHrEmpMaster] where EmpId='" + EmpId + "'");
                }
                else
                {
                    CheckEmpId = false;
                }
            }
            catch (Exception ex)
            {
                return "False";
            }

            return JsonConvert.SerializeObject(CheckEmpId.ToString());
        }


        [WebMethod]
        public static List<string> FetchBankBranchAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtBranchList = new DataTable();
            List<string> branch = new List<string>();

            try
            {

                dtBranchList = objMain.dtFetchData("select [BranchName] as Name FROM [tblHrBranchMaster] where BranchName like '"+prefixText+"' + '%'");
                if(dtBranchList != null)
                {
                    if (dtBranchList.Rows.Count > 0)
                    {
                        for(int i=0;i< dtBranchList.Rows.Count; i++)
                        {
                            branch.Add(dtBranchList.Rows[i][0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return branch;
            }

            return branch;
        }


        [WebMethod]
        public static List<string> FetchAddressStateAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtStateList = new DataTable();
            List<string> state = new List<string>();

            try
            {

                dtStateList = objMain.dtFetchData("select [StateName] as Name FROM [tblHrStateMaster] where StateName like '" + prefixText + "' + '%'");
                if (dtStateList != null)
                {
                    if (dtStateList.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtStateList.Rows.Count; i++)
                        {
                            state.Add(dtStateList.Rows[i][0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return state;
            }

            return state;
        }

        [WebMethod]
        public static List<string> FetchAddressPinAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtPinList = new DataTable();
            List<string> pin = new List<string>();

            try
            {

            }
            catch (Exception ex)
            {
                return pin;
            }

            return pin;
        }

        [WebMethod]
        public static List<string> FetchAddressPostAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtPostList = new DataTable();
            List<string> post = new List<string>();

            try
            {

            }
            catch (Exception ex)
            {
                return post;
            }

            return post;
        }

        [WebMethod]
        public static List<string> FetchAddressDistrictAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtDistrictList = new DataTable();
            List<string> district = new List<string>();

            try
            {

            }
            catch (Exception ex)
            {
                return district;
            }

            return district;
        }

        [WebMethod]
        public static List<string> FetchBankNameAutocomplete(string prefixText)
        {
            clsMain objMain = new clsMain();
            DataTable dtBankList = new DataTable();
            List<string> bank = new List<string>();

            try
            {

            }
            catch (Exception ex)
            {
                return bank;
            }

            return bank;
        }

        [WebMethod]
        public static string SaveUpdateEmployeeDetails(string EmpId = "", string Sex = "", string Name = "", string FatherName = "",
                                    string MotherName = "", string SpouseName = "", string MobileNo = "", string Email = "", string DOB = "",
                                    string DOJ = "", string SalaryGrade = "", string Religion = "", string Caste = "", string Department = "",
                                    string Division = "", string Designation = "", string Grade = "", string Area = "", string Branch = "",
                                    string MaritalStatus = "", string PresentAddrResNo = "", string PresentAddrResName = "",
                                    string PresentAddrRoad = "", string PresentAddrPin = "", string PresentAddrPost = "", string PresentAddrState = "",
                                    string PresentAddrDistrict = "", string PermAddrResNo = "", string PermAddrResName = "", string PermAddrRoad = "",
                                    string PermAddrPin = "", string PermAddrPost = "", string PermAddrState = "", string PermAddrDistrict = "",
                                    string ProfileImage = "", string AdharNo = "", string AdharImage = "", string VoterNo = "", string VoterImage = "",
                                    string PANNo = "", string PANImage = "", string PassportNo = "", string PassportImage = "", string DrivingLicense = "",
                                    string DrivingLicenseImage = "", string IFSC = "", string BankBranch = "", string BankName = "", string AcNumber = "",
                                    string AccountImage = "", string PFNo="", string ESINo="", string LoginUser="", string CardNo = "")
        {
            
            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[53];


            objParam[0] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = EmpId;


            objParam[1] = new SqlParameter("@EmpName", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Name;


            objParam[2] = new SqlParameter("@Branchcode", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = Branch;

            DateTime dt_DOB = new DateTime();
            if (!string.IsNullOrEmpty(DOB))
            {
                dt_DOB = Convert.ToDateTime(DOB);
            }

            objParam[3] = new SqlParameter("@DOB", SqlDbType.DateTime);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = dt_DOB;


            if (!string.IsNullOrEmpty(DOJ))
            {
                objParam[4] = new SqlParameter("@DOJ", SqlDbType.DateTime);
                objParam[4].Direction = ParameterDirection.Input;
                objParam[4].Value = Convert.ToDateTime(DOJ);
            }
            else
            {
                objParam[4] = new SqlParameter("@DOJ", SqlDbType.DateTime);
                objParam[4].Direction = ParameterDirection.Input;
                objParam[4].Value = DBNull.Value;
            }

            objParam[5] = new SqlParameter("@PresentDesignation", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = Designation;

            objParam[6] = new SqlParameter("@PresentDepartId", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = Department;

            objParam[7] = new SqlParameter("@EmpPhotoImage", SqlDbType.VarBinary);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = DBNull.Value;

            objParam[8] = new SqlParameter("@Area", SqlDbType.NVarChar);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = Area;

            objParam[9] = new SqlParameter("@FatherName", SqlDbType.NVarChar);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = FatherName;

            objParam[10] = new SqlParameter("@MotherName", SqlDbType.NVarChar);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = MotherName;

            objParam[11] = new SqlParameter("@SpouseName", SqlDbType.NVarChar);
            objParam[11].Direction = ParameterDirection.Input;
            objParam[11].Value = SpouseName;

            objParam[12] = new SqlParameter("@Division", SqlDbType.NVarChar);
            objParam[12].Direction = ParameterDirection.Input;
            objParam[12].Value = Division;

            objParam[13] = new SqlParameter("@Grade", SqlDbType.NVarChar);
            objParam[13].Direction = ParameterDirection.Input;
            objParam[13].Value = Grade;

            objParam[14] = new SqlParameter("@PresentResNo", SqlDbType.NVarChar);
            objParam[14].Direction = ParameterDirection.Input;
            objParam[14].Value = PresentAddrResNo;

            objParam[15] = new SqlParameter("@PresentResName", SqlDbType.NVarChar);
            objParam[15].Direction = ParameterDirection.Input;
            objParam[15].Value = PresentAddrResName;

            objParam[16] = new SqlParameter("@PresentRoadStreet", SqlDbType.NVarChar);
            objParam[16].Direction = ParameterDirection.Input;
            objParam[16].Value = PresentAddrRoad;

            objParam[17] = new SqlParameter("@PresentPinNo", SqlDbType.NVarChar);
            objParam[17].Direction = ParameterDirection.Input;
            objParam[17].Value = PresentAddrPin;

            objParam[18] = new SqlParameter("@PresentPost", SqlDbType.NVarChar);
            objParam[18].Direction = ParameterDirection.Input;
            objParam[18].Value = PresentAddrPost;

            objParam[19] = new SqlParameter("@PresentState", SqlDbType.NVarChar);
            objParam[19].Direction = ParameterDirection.Input;
            objParam[19].Value = PresentAddrState;

            objParam[20] = new SqlParameter("@PresentDistrict", SqlDbType.NVarChar);
            objParam[20].Direction = ParameterDirection.Input;
            objParam[20].Value = PresentAddrDistrict;

            objParam[21] = new SqlParameter("@PermanentResNo", SqlDbType.NVarChar);
            objParam[21].Direction = ParameterDirection.Input;
            objParam[21].Value = PermAddrResNo;

            objParam[22] = new SqlParameter("@PermanentResName", SqlDbType.NVarChar);
            objParam[22].Direction = ParameterDirection.Input;
            objParam[22].Value = PermAddrResName;

            objParam[23] = new SqlParameter("@PermanentRoadStreet", SqlDbType.NVarChar);
            objParam[23].Direction = ParameterDirection.Input;
            objParam[23].Value = PermAddrRoad;

            objParam[24] = new SqlParameter("@PermanentPinNo", SqlDbType.NVarChar);
            objParam[24].Direction = ParameterDirection.Input;
            objParam[24].Value = PermAddrPin;

            objParam[25] = new SqlParameter("@PermanentPost", SqlDbType.NVarChar);
            objParam[25].Direction = ParameterDirection.Input;
            objParam[25].Value = PermAddrPost;

            objParam[26] = new SqlParameter("@PermanentState", SqlDbType.NVarChar);
            objParam[26].Direction = ParameterDirection.Input;
            objParam[26].Value = PermAddrState;

            objParam[27] = new SqlParameter("@PermanentDistrict", SqlDbType.NVarChar);
            objParam[27].Direction = ParameterDirection.Input;
            objParam[27].Value = PermAddrDistrict;

            objParam[28] = new SqlParameter("@AdharNo", SqlDbType.NVarChar);
            objParam[28].Direction = ParameterDirection.Input;
            objParam[28].Value = AdharNo;

            objParam[29] = new SqlParameter("@AdharNoImage", SqlDbType.VarBinary);
            objParam[29].Direction = ParameterDirection.Input;
            objParam[29].Value = DBNull.Value;

            objParam[30] = new SqlParameter("@VoterNo", SqlDbType.NVarChar);
            objParam[30].Direction = ParameterDirection.Input;
            objParam[30].Value = VoterNo;

            objParam[31] = new SqlParameter("@VoterImage", SqlDbType.VarBinary);
            objParam[31].Direction = ParameterDirection.Input;
            objParam[31].Value = DBNull.Value;

            objParam[32] = new SqlParameter("@PanNo", SqlDbType.NVarChar);
            objParam[32].Direction = ParameterDirection.Input;
            objParam[32].Value = PANNo;

            objParam[33] = new SqlParameter("@PanNoImage", SqlDbType.VarBinary);
            objParam[33].Direction = ParameterDirection.Input;
            objParam[33].Value = DBNull.Value;

            objParam[34] = new SqlParameter("@Passport", SqlDbType.NVarChar);
            objParam[34].Direction = ParameterDirection.Input;
            objParam[34].Value = PassportNo;

            objParam[35] = new SqlParameter("@PassportImage", SqlDbType.VarBinary);
            objParam[35].Direction = ParameterDirection.Input;
            objParam[35].Value = DBNull.Value;

            objParam[36] = new SqlParameter("@DrivingNo", SqlDbType.NVarChar);
            objParam[36].Direction = ParameterDirection.Input;
            objParam[36].Value = DrivingLicense;

            objParam[37] = new SqlParameter("@DrivingNoImage", SqlDbType.VarBinary);
            objParam[37].Direction = ParameterDirection.Input;
            objParam[37].Value = DBNull.Value;

            objParam[38] = new SqlParameter("@IfscCode", SqlDbType.NVarChar);
            objParam[38].Direction = ParameterDirection.Input;
            objParam[38].Value = IFSC;

            objParam[39] = new SqlParameter("@BankBranchName", SqlDbType.NVarChar);
            objParam[39].Direction = ParameterDirection.Input;
            objParam[39].Value = BankBranch;

            objParam[40] = new SqlParameter("@BankName", SqlDbType.NVarChar);
            objParam[40].Direction = ParameterDirection.Input;
            objParam[40].Value = BankName;

            objParam[41] = new SqlParameter("@AcNumber", SqlDbType.NVarChar);
            objParam[41].Direction = ParameterDirection.Input;
            objParam[41].Value = AcNumber;

            objParam[42] = new SqlParameter("@BankImage", SqlDbType.VarBinary);
            objParam[42].Direction = ParameterDirection.Input;
            objParam[42].Value = DBNull.Value;

            objParam[43] = new SqlParameter("@PfNo", SqlDbType.NVarChar);
            objParam[43].Direction = ParameterDirection.Input;
            objParam[43].Value = PFNo;

            objParam[44] = new SqlParameter("@EsiNo", SqlDbType.NVarChar);
            objParam[44].Direction = ParameterDirection.Input;
            objParam[44].Value = ESINo;

            objParam[45] = new SqlParameter("@Sex", SqlDbType.NVarChar);
            objParam[45].Direction = ParameterDirection.Input;
            objParam[45].Value = Sex;

            objParam[46] = new SqlParameter("@MaritalStatus", SqlDbType.NVarChar);
            objParam[46].Direction = ParameterDirection.Input;
            objParam[46].Value = MaritalStatus;

            objParam[47] = new SqlParameter("@MobileNo", SqlDbType.NVarChar);
            objParam[47].Direction = ParameterDirection.Input;
            objParam[47].Value = MobileNo;

            objParam[48] = new SqlParameter("@EmailAddress", SqlDbType.NVarChar);
            objParam[48].Direction = ParameterDirection.Input;
            objParam[48].Value = Email;

            objParam[49] = new SqlParameter("@Religion", SqlDbType.NVarChar);
            objParam[49].Direction = ParameterDirection.Input;
            objParam[49].Value = Religion;

            objParam[50] = new SqlParameter("@Caste", SqlDbType.NVarChar);
            objParam[50].Direction = ParameterDirection.Input;
            objParam[50].Value = Caste;

            objParam[51] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[51].Direction = ParameterDirection.Input;
            objParam[51].Value = LoginUser;

            objParam[52] = new SqlParameter("@CardNo", SqlDbType.NVarChar);
            objParam[52].Direction = ParameterDirection.Input;
            objParam[52].Value = CardNo;

            var result= objMain.ExecuteProcedure("procHrEmpCreateUpdateEmploye", objParam);


            return "";
        }

        [WebMethod]
        public static string FetchEmployeList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData(@"select e.EmpId,e.EmpName,e.Sex,e.FatherName,e.MotherName,e.SpouseName,e.MobileNo,
                                                e.EmailAddress,CONVERT(nvarchar,e.DOB,106) as DOB,CONVERT(nvarchar,e.DOJ,106) as DOJ,e.Religion,e.Caste,e.MaritalStatus,d.DeptName,dv.DivisionName,
                                                dm.DesignationName,g.GradeName,a.AreaName,br.BranchName
                                                from tblHrEmpMaster e left join tblHrDeptMaster d on e.PresentDepartId=d.Id
                                                left join tblHrDivisionMaster dv on e.Division=dv.ID
                                                left join tblHrDesignationMaster dm on e.PresentDesignation=dm.ID
                                                left join tblHrGradeMaster g on e.Grade=g.ID
                                                left join tblHrAreaMaster a on e.Area=a.Id
                                                join tblHrBranchMaster br on e.Branchcode=br.BranchCode");
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
            return JsonConvert.SerializeObject(dtEmpList,settings);
        }




        [WebMethod]
        public static string FetchEmployeeDetails(string EmpId="")
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
                                              MobileNo,EmailAddress,Religion,Caste,CardNo from tblHrEmpMaster where EmpId='" + EmpId+"'");
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

            string json = JsonConvert.SerializeObject(dtEmpList, Formatting.None);
            return json;
        }

        [WebMethod]
        public static string CheckEmployeeCardNoAvailability(string EmpId, string CardNo)
        {
            clsMain objMain = new clsMain();
            bool CheckEmpCardNo = new bool();

            try
            {

                if (CardNo.Trim() != "")
                {
                    CheckEmpCardNo = objMain.blSearchDataHO("select EmpId FROM [tblHrEmpMaster] where EmpId !='" + EmpId + "' and CardNo='"+CardNo+"'");
                }
                else
                {
                    CheckEmpCardNo = false;
                }
            }
            catch (Exception ex)
            {
                return "False";
            }

            return JsonConvert.SerializeObject(CheckEmpCardNo.ToString());
        }
    }
}