using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace BizzManWebErp
{
    public partial class wfMmRequisitionNote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] != null)
            {
                loginuser.Value =Convert.ToString(Session["Id"]);

            }
            else
            {
                Response.Redirect("wfAdminLogin.aspx");
            }

        }

        [WebMethod]
        public static string FetchRequisitionNoteDetails(string requisitionId = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtCategoryList = new DataTable();

            try
            {

                dtCategoryList = objMain.dtFetchData(string.Format(@"select RequisitionId,RequisitionDate,BranchCode,DepartmentId,RequisitionNote,Active, CreateUser
                                                    from tblMmMaterialRequisitionNote where RequisitionId='{0}'",requisitionId));
            }
            catch (Exception)
            { 
            }

            return JsonConvert.SerializeObject(dtCategoryList, Formatting.None);
           
        }

        [WebMethod]
        public static string FetchRequisitionNoteList()
        {
            clsMain objMain = new clsMain();
            DataTable dtEmpList = new DataTable();

            try
            {

                dtEmpList = objMain.dtFetchData(@"select RequisitionId, CONVERT(nvarchar,RequisitionDate,106) as RequisitionDate, RequisitionNote,hm.DeptName,bm.BranchName, rn.Active, rn.CreateUser
                                                from tblMmMaterialRequisitionNote rn
                                                inner join tblHrDeptMaster hm on hm.Id=rn.DepartmentId
                                                inner join tblHrBranchMaster bm on bm.BranchCode=rn.BranchCode");
            }
            catch (Exception)
            { 
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
        public static string CheckRequisitionNoteAvailability(string id, string isUpdate)
        {
            clsMain objMain = new clsMain();
            bool checkId = new bool();

            try
            {

                if (isUpdate == "0")
                {
                    checkId = objMain.blSearchDataHO("select RequisitionId FROM [tblMmMaterialRequisitionNote] where RequisitionId='" + id + "'");
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
        public static string AddRequisitionNote(string requisitionId, string requisitionDate, string branch, int dept, string requisitionNote = "", string active = "", string LoginUser = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[8];

            objParam[0] = new SqlParameter("@RequisitionId", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = requisitionId;

            DateTime dt_reqDate = new DateTime();
            if (!string.IsNullOrEmpty(requisitionDate))
            {
                dt_reqDate = Convert.ToDateTime(requisitionDate);
            }

            objParam[1] = new SqlParameter("@RequisitionDate", SqlDbType.DateTime);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = dt_reqDate;

            objParam[2] = new SqlParameter("@RequisitionNote", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = requisitionNote;

            objParam[3] = new SqlParameter("@Active", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = active;

            objParam[4] = new SqlParameter("@Branch", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = branch;

            objParam[5] = new SqlParameter("@Dept", SqlDbType.BigInt);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = dept;

            objParam[6] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = LoginUser;

            objParam[7] = new SqlParameter("@UpdateUser", SqlDbType.NVarChar);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = LoginUser;

            var result = objMain.ExecuteProcedure("procMmMaterialRequisitionNote", objParam);


            return "";
        }

        [WebMethod]
        public static string GetBranchDetails()
        {
            clsMain objMain = new clsMain();
            DataTable dtList = new DataTable();

            try
            {

                dtList = objMain.dtFetchData("select BranchName, BranchCode from tblHrBranchMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtList);
        }

        [WebMethod]
        public static string GetDeptDetails()
        {
            clsMain objMain = new clsMain();
            DataTable dtList = new DataTable();

            try
            {

                dtList = objMain.dtFetchData("select DeptName, Id from tblHrDeptMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtList);
        }
    }
}