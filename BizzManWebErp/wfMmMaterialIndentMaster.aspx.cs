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
    public partial class wfMmMaterialIndentMaster : System.Web.UI.Page
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
        public static string RequisitionIdList()
        {
            clsMain objMain = new clsMain();
            DataTable dtRequisitionIdList = new DataTable();

            try
            {

                dtRequisitionIdList = objMain.dtFetchData("select [RequisitionId] FROM [tblMmMaterialRequisitionNote] where [Active]='Y'");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtRequisitionIdList);
        }

        [WebMethod]
        public static string MaterialNameList()
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialNameList = new DataTable();

            try
            {

                dtMaterialNameList = objMain.dtFetchData("select [Id],[MaterialName] FROM [tblMmMaterialMaster]");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtMaterialNameList);
        }

        [WebMethod]
        public static string FetchMaterialIndentMasterList()
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialIndentMasterList = new DataTable();

            try
            {

                dtMaterialIndentMasterList = objMain.dtFetchData(@"select im.Id,im.MaterialRequisitionNoteId,rn.RequisitionNote,im.Description,
                                      CONVERT(nvarchar,im.CreateDate,106) as CreateDate,d.DeptName,b.BranchName from 
                                      tblMmMaterialIndentMaster im join tblMmMaterialRequisitionNote rn on im.MaterialRequisitionNoteId=rn.RequisitionId
                                      left join tblHrDeptMaster d on d.Id=im.DepartmentId
                                      left join tblHrBranchMaster b on b.BranchCode=im.BranchCode
                                      where im.Active='Y' order by im.Id desc");
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
            return JsonConvert.SerializeObject(dtMaterialIndentMasterList, settings);
        }



        [WebMethod]
        public static string FetchRequisitionNote(string ReqId = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtRequisitionNote = new DataTable();

            try
            {

                dtRequisitionNote = objMain.dtFetchData(@"select rn.*,d.DeptName,b.BranchName from tblMmMaterialRequisitionNote rn
                                                      left join tblHrDeptMaster d on d.Id=rn.DepartmentId
                                                      left join tblHrBranchMaster b on b.BranchCode=rn.BranchCode
                                                      where RequisitionId='" + ReqId + "'");
            }
            catch (Exception ex)
            {
                // return "";
            }

            string json = JsonConvert.SerializeObject(dtRequisitionNote, Formatting.None);
            return json;
        }


        [WebMethod]
        public static string AddMaterialIndentMaster(string ReqId = "", string EntryDate = "", string Description = "", 
                                                   string Indent_details = "", string LoginUser = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[5];


            objParam[0] = new SqlParameter("@ReqId", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = ReqId;


            objParam[1] = new SqlParameter("@EntryDate", SqlDbType.DateTime);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Convert.ToDateTime(EntryDate);


            objParam[2] = new SqlParameter("@Description", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = Description;


            objParam[3] = new SqlParameter("@Indent_details", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = Indent_details;

            objParam[4] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = LoginUser;


            var result = objMain.ExecuteProcedure("procMmMaterialIndentMasterInsertUpdate", objParam);


            return "";
        }




        [WebMethod]
        public static string DeleteMaterialIndentMasterEntry(string id = "")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[1];


            objParam[0] = new SqlParameter("@id", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(id);


            var result = objMain.ExecuteProcedure("procMmMaterialIndentMasterDelete", objParam);


            return "";
        }



        [WebMethod]
        public static string FetchMaterialIndentMasterDetails(string id="")
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialIndentMasterList = new DataTable();

            try
            {

                dtMaterialIndentMasterList = objMain.dtFetchData(@"select im.Id,im.MaterialRequisitionNoteId,rn.RequisitionNote,im.Description,
                                      CONVERT(nvarchar,im.CreateDate,101) as CreateDate,im.CreateUser,d.DeptName,b.BranchName from 
                                      tblMmMaterialIndentMaster im join tblMmMaterialRequisitionNote rn on im.MaterialRequisitionNoteId=rn.RequisitionId
                                      left join tblHrDeptMaster d on d.Id=im.DepartmentId
                                      left join tblHrBranchMaster b on b.BranchCode=im.BranchCode
                                      where im.Active='Y' and im.Id=" + id+"");
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
            return JsonConvert.SerializeObject(dtMaterialIndentMasterList, settings);
        }



        [WebMethod]
        public static string FetchMaterialIndentDetails(string id = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialIndentMasterList = new DataTable();

            try
            {

                dtMaterialIndentMasterList = objMain.dtFetchData(@"select im.*,mm.MaterialName from 
                                      tblMmMaterialIndentDetail im join tblMmMaterialMaster mm on im.MaterialMasterId=mm.Id
                                      where im.MaterialIndentMasterId=" + id + "");
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
            return JsonConvert.SerializeObject(dtMaterialIndentMasterList, settings);
        }


    }
}