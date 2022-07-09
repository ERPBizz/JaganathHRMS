using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BizzManWebErp
{
    public partial class wfFaAccountsGroupMaster : System.Web.UI.Page
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
        public static string GetPrimeGroupList()
        {
            clsMain objMain = new clsMain();
            DataTable dtPrimeGroupList = new DataTable();

            try
            {
               dtPrimeGroupList = objMain.dtFetchData("select Id, PrimeGroupName from dbo.tblFaPrimeGroupMaster");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return JsonConvert.SerializeObject(dtPrimeGroupList);
        }

        [WebMethod]
        public static string FetchAccountsGroupList()
        {
            clsMain objMain = new clsMain();
            DataTable dtAccountsGroupList = new DataTable();

            try
            {
                dtAccountsGroupList = objMain.dtFetchData(@"select agm.Id, RTRIM(LTRIM(pgm.PrimeGroupName)) as 'PrimeGroupName', RTRIM(LTRIM(agm.GroupName)) as 'GroupName', RTRIM(LTRIM(agm.GroupCategory)) as 'GroupCategory'
                                                            ,ISNULL(um.UserName, '-') as 'CreatedBy'
                                                            from tblFaGroupMaster agm 
                                                            left join tblFaPrimeGroupMaster pgm on agm.PrimeGroupId = pgm.Id
                                                            left join tblUserMaster um on agm.CreateUser = um.UserName
                                                            order by agm.Id asc");
                foreach (DataRow dr in dtAccountsGroupList.Rows)
                {
                    dr.SetField<string>("GroupName", Convert.ToString(dr["GroupName"]).Replace("\r\n", ""));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays
            };

            return JsonConvert.SerializeObject(dtAccountsGroupList, settings);
        }

        [WebMethod]
        public static string AddNewAccountGroup(string PrimeGroupId, string GroupName, string GroupCategory, string loginUser)
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[4];

            objParam[0] = new SqlParameter("@PrimeGroupId", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(PrimeGroupId);

            objParam[1] = new SqlParameter("@GroupName", SqlDbType.NVarChar);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = GroupName;

            objParam[2] = new SqlParameter("@GroupCategory", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = GroupCategory;

            objParam[3] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = loginUser;

            var result = objMain.ExecuteProcedure("procFaAccountsGroupMaster", objParam);

            return "";
        }

        [WebMethod]
        public static string FetchFaAccountsGroupDetails(string groupName)
        {
            clsMain objMain = new clsMain();
            DataTable dtCategoryList = new DataTable();

            try
            {
                string strSql = string.Format(@"select Id, PrimeGroupId, RTRIM(LTRIM(GroupName)) as 'GroupName', GroupCategory from tblFaGroupMaster where GroupName like '%{0}%'", groupName);
                dtCategoryList = objMain.dtFetchData(strSql);
                                 
            }
            catch (Exception ex)
            {
            }

            return JsonConvert.SerializeObject(dtCategoryList, Formatting.None);

        }
    }
}