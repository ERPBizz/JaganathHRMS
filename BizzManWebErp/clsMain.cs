using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;


namespace BizzManWebErp
{
 

    public class clsMain
    {

        public System.Data.SqlClient.SqlConnection con;


        public System.Data.SqlClient.SqlConnection conHo;
        public System.Data.SqlClient.SqlCommand comm;
        public System.Data.SqlClient.SqlDataAdapter DA;
        public System.Data.DataSet DS;
        public System.Data.DataTable DT;
        public bool gblConStatus, gblConHoStatus;
        //  public string gstrFinancialYear;
        // '" + System.DateTime.Today.ToString("yyyyMMdd") + "'

      

        public string gstrFinancialYear;
        public DateTime gdtStartFianceDate, gdtEndFinanceDate;

        public clsMain()
        {
            gblConStatus = false;  // for Bijli Database
            gblConHoStatus = false;
            try
            {              
              //  for Local server,  my Laptop
              //  conHo = new System.Data.SqlClient.SqlConnection("Data Source=HP;Initial Catalog=BizzzManERP;User ID=sa;Password=aRindom@2022;Trusted_Connection=False;Persist Security Info=True;");
              // conHo = new System.Data.SqlClient.SqlConnection("Data Source=Subhojit-Dey;Initial Catalog=BizzzManERP;Integrated Security=True");
            
                // MY LAPTOP
                conHo = new System.Data.SqlClient.SqlConnection("Data Source=DESKTOP-NUIV2H8;Initial Catalog=BizzzManERP;User ID=sa;Password=123;Persist Security Info=True");

              

                // go dady            
               //  conHo = new System.Data.SqlClient.SqlConnection("Data Source=A2NWPLSK14SQL-v06.shr.prod.iad2.secureserver.net;Initial Catalog=BizmanTest1;User ID=BizmanTest1_user;Password=Pa$$w0rd123");
                //conHo = new System.Data.SqlClient.SqlConnection("Data Source=A2NWPLSK14SQL-v06.shr.prod.iad2.secureserver.net;Initial Catalog=BizmanTest2;User ID=BizmanTest1_user;Password=Pa$$w0rd123;Persist Security Info=True");



                conHo.Open();
                gblConHoStatus = true;
            }
            catch (Exception eConHo)
            {
                gblConHoStatus = false;
            }
        }

        public bool blSearchDataHO(string strSQL)
        {
            try
            {
                DataTable dt = new DataTable();
                DA = new System.Data.SqlClient.SqlDataAdapter(strSQL, conHo);
                DA.Fill(dt);
                if (dt.Rows.Count > 0) { return true; }
                else { return false; }
            }
            catch (Exception e)
            {
                //MessageBox.Show("internet connection error,   try later..");
                return false;
            }
        }
        public string strFetchDate(string strSQL)
        {
            try
            {
                string temp;
                DataTable dt = new DataTable();
                DA = new System.Data.SqlClient.SqlDataAdapter(strSQL, conHo);
                DA.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    temp = dt.Rows[0][0].ToString();
                }
                else
                {
                    temp = "";
                }
                return temp;
            }
            catch (Exception e)
            {
                //  MessageBox.Show("internet connection error,   try later..");
               // MessageBox.Show(e.Message.ToString());
                string temp1 = "";
                return temp1;
            }
        }

        public DataTable dtFetchData(string strSQL)
        {
            try
            {
                DataTable dt = new DataTable();
                DA = new System.Data.SqlClient.SqlDataAdapter(strSQL, conHo);
                DA.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
               // MessageBox.Show("internet connection error,   try later..");
                DataTable dt1 = new DataTable();
                return dt1;
            }
        }


        public bool Insert(string strSQL)
        {
            int result;
            try
            {
                comm = new System.Data.SqlClient.SqlCommand(strSQL, conHo);
                result = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message.ToString());
                result = 0;
            }

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }






        public dynamic ExecuteProcedure(string sqlProcName, SqlParameter[] sqlParam)
        {

            int result = 0;
            try
            {
                using (SqlConnection _sqlConnection = new SqlConnection(conHo.ConnectionString))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    using (SqlCommand _sqlCommand = new SqlCommand(sqlProcName, _sqlConnection))
                    {
                        _sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParam != null && sqlParam.Length > 0)
                        { _sqlCommand.Parameters.AddRange(sqlParam); }
                        result = _sqlCommand.ExecuteNonQuery();
                        return
                                new
                                {
                                    msg = result.ToString(),
                                    status = "success"
                                };
                    }
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    msg = ex.Message,
                    status = "failure"
                };
            }

        }



        public DataTable ExecuteStoreProcedure(string strProcName, SqlParameter[] sqlParam)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection _sqlConnection = new SqlConnection(conHo.ConnectionString))
                {
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    using (SqlCommand _sqlCommand = new SqlCommand(strProcName, _sqlConnection))
                    {
                        _sqlCommand.CommandTimeout = 20 * 60 * 1000;
                        _sqlCommand.CommandType = CommandType.StoredProcedure;
                        if (sqlParam != null && sqlParam.Length > 0)
                        { _sqlCommand.Parameters.AddRange(sqlParam); }
                        SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter();
                        _sqlDataAdapter.SelectCommand = _sqlCommand;
                        _sqlDataAdapter.Fill(dt);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }


    }
}