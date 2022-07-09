using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace BizzManWebErp
{
    public partial class wfFaLedgerMaster : System.Web.UI.Page
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
        public static string AccountGroupList()
        {
            clsMain objMain = new clsMain();
            DataTable dtAccountGroupList = new DataTable();

            try
            {
                dtAccountGroupList = objMain.dtFetchData("select Id,GroupName from dbo.tblFaGroupMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtAccountGroupList);
        }

        [WebMethod]
        public static string FetchLedgerDetails()
        {
            clsMain objMain = new clsMain();
            DataTable dtLedgerList = new DataTable();

            try
            {

                dtLedgerList = objMain.dtFetchData(@"select flm.Id,LedgerName,fgm.GroupName, OpBalance, flm.CreateUser, FinancialYear, Active
                                                 from dbo.tblFaLedgerMaster flm
                                                 inner join dbo.tblFaGroupMaster fgm on fgm.Id = flm.GroupMasterId
												 inner join dbo.tblFaSetup fs on fs.CurrentFinancialId = flm.FinancialYearId");
            }
            catch (Exception ex)
            {
            }
            
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays
            };
            return JsonConvert.SerializeObject(dtLedgerList, settings);
        }
                
        [WebMethod]
        public static string FetchFaPrimeGroupDetails(string id)
        {
            clsMain objMain = new clsMain();
            DataTable dtPrimeGroupList = new DataTable();

            try
            {

                dtPrimeGroupList = objMain.dtFetchData(string.Format(@"select LTRIM(RTRIM(pgm.PrimeGroupName)) as 'PrimeGroupName' from dbo.tblFaGroupMaster fgm 
                                                     inner join dbo.tblFaPrimeGroupMaster pgm on pgm.Id = fgm.PrimeGroupId 
                                                     where fgm.Id='{0}'", id));

                foreach (DataRow dr in dtPrimeGroupList.Rows)
                {
                    dr.SetField<string>("PrimeGroupName", Convert.ToString(dr["PrimeGroupName"]).Replace("\r\n", ""));
                }
            }
            catch (Exception ex)
            {
            }

            return JsonConvert.SerializeObject(dtPrimeGroupList, Formatting.None);
        }

        [WebMethod]
        public static string FetchFaLedgerMasterDetails(string id, string ledgerName)
        {
            clsMain objMain = new clsMain();
            DataTable dtLedgerDetails = new DataTable();

            try
            {
                dtLedgerDetails = objMain.dtFetchData(string.Format("Select Id, GroupMasterId, LedgerName, IfcsCode, BankName, BankBrnachName as 'BankBranchName', BankAcNo, OpBalance,	Active from dbo.tblFaLedgerMaster where Id='{0}' or LedgerName='{1}'", id, ledgerName));
            }
            catch (Exception ex)
            {
            }

            return JsonConvert.SerializeObject(dtLedgerDetails, Formatting.None);
        }

        [WebMethod]
        public static string CheckLedgerAvailability(string ledgerName, string isUpdate)
        {
            clsMain objMain = new clsMain();
            bool checkId = new bool();

            try
            {

                if (isUpdate == "0")
                {
                    checkId = objMain.blSearchDataHO(string.Format("select 1 from dbo.tblFaLedgerMaster where  LedgerName like  '%{0}%'", ledgerName));
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
        public static string AddDetails(string groupMasterId, string ledgerName, string ifcsCode, string bankName, string bankBranchName, string bankAcNo, string opBalance, string clBalance, string active, string loginUser)
        {
            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[11];

            objParam[0] = new SqlParameter("@Id", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = null;

            objParam[1] = new SqlParameter("@GroupMasterId", SqlDbType.Int);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Convert.ToInt32(groupMasterId);

            objParam[2] = new SqlParameter("@LedgerName", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = ledgerName;

            objParam[3] = new SqlParameter("@IfcsCode", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(ifcsCode))
            {
                objParam[3].Value = ifcsCode;
            }

            objParam[4] = new SqlParameter("@BankName", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankName))
            {
                objParam[4].Value = bankName;
            }

            objParam[5] = new SqlParameter("@BankBranchName", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankBranchName))
            {
                objParam[5].Value = bankBranchName;
            }

            objParam[6] = new SqlParameter("@BankAcNo", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankAcNo))
            {
                objParam[6].Value = bankAcNo;
            }

            objParam[7] = new SqlParameter("@OpBalance", SqlDbType.Decimal);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = Convert.ToInt32(opBalance);

            objParam[8] = new SqlParameter("@ClBalance", SqlDbType.Decimal);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = Convert.ToInt32(clBalance);

            objParam[9] = new SqlParameter("@Active", SqlDbType.NVarChar);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = active;

            objParam[10] = new SqlParameter("@CurrentUser", SqlDbType.NVarChar);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = loginUser;

            var result = objMain.ExecuteProcedure("procFaLedgerMaster", objParam);

            return "";
        }

        [WebMethod]
        public static string UpdateLedgerDetails(string ledgerId, string groupMasterId, string ledgerName, string ifcsCode, string bankName, string bankBranchName, string bankAcNo, string opBalance, string clBalance, string active, string loginUser)
        {
            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[11];

            objParam[0] = new SqlParameter("@Id", SqlDbType.Int);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = Convert.ToInt32(ledgerId);

            objParam[1] = new SqlParameter("@GroupMasterId", SqlDbType.Int);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Convert.ToInt32(groupMasterId);

            objParam[2] = new SqlParameter("@LedgerName", SqlDbType.NVarChar);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = ledgerName;

            objParam[3] = new SqlParameter("@IfcsCode", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(ifcsCode))
            {
                objParam[3].Value = ifcsCode;
            }

            objParam[4] = new SqlParameter("@BankName", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankName))
            {
                objParam[4].Value = bankName;
            }

            objParam[5] = new SqlParameter("@BankBranchName", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankBranchName))
            {
                objParam[5].Value = bankBranchName;
            }

            objParam[6] = new SqlParameter("@BankAcNo", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;

            if (!string.IsNullOrEmpty(bankAcNo))
            {
                objParam[6].Value = bankAcNo;
            }

            objParam[7] = new SqlParameter("@OpBalance", SqlDbType.Decimal);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = Convert.ToInt32(opBalance);

            objParam[8] = new SqlParameter("@ClBalance", SqlDbType.Decimal);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = Convert.ToInt32(clBalance);

            objParam[9] = new SqlParameter("@Active", SqlDbType.NVarChar);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = active;

            objParam[10] = new SqlParameter("@CurrentUser", SqlDbType.NVarChar);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = loginUser;

            var result = objMain.ExecuteProcedure("procFaLedgerMaster", objParam);

            return "";
        }
     }
}