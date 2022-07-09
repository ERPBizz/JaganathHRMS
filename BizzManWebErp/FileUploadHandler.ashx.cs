using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace BizzManWebErp
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.QueryString["emp_id"] != null)
            {

                string imgType = context.Request.QueryString["img_type"];
                clsMain objMain = new clsMain();
                DataTable dtEmpList = new DataTable();
                byte[] rawBytes = new byte[0];
                dtEmpList = objMain.dtFetchData(@"select "+ imgType + " from tblHrEmpMaster where EmpId='" + context.Request.QueryString["emp_id"].ToString() + "'");
                if (dtEmpList != null)
                {
                    if (dtEmpList.Rows.Count > 0)
                    {
                        if (dtEmpList.Rows[0][0] != DBNull.Value)
                        {
                            context.Response.BinaryWrite((byte[])dtEmpList.Rows[0][0]);
                        }
                        else
                        {
                            context.Response.BinaryWrite(rawBytes);
                        }
                    }
                    else
                    {
                        context.Response.BinaryWrite(rawBytes);
                    }
                }
                else
                {
                    context.Response.BinaryWrite(rawBytes);
                }

            }
            else
            {
                byte[] profile_bytes = null;
                byte[] adhar_bytes = null;
                byte[] voter_bytes = null;
                byte[] pan_bytes = null;
                byte[] passport_bytes = null;
                byte[] driving_bytes = null;
                byte[] bank_bytes = null;
                string empid = "";
                int img_save = 0;
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        if (Path.GetExtension(file.FileName).ToLower() == ".xlsx")
                        {
                            using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                            {
                                //Read the first Sheet from Excel file.
                                IXLWorksheet workSheet = workBook.Worksheet(1);

                                //Create a new DataTable.
                                DataTable dt = new DataTable();

                                //Loop through the Worksheet rows.
                                bool firstRow = true;
                                foreach (IXLRow row in workSheet.Rows())
                                {
                                    //Use the first row to add columns to DataTable.
                                    if (firstRow)
                                    {
                                        foreach (IXLCell cell in row.Cells())
                                        {
                                            dt.Columns.Add(cell.Value.ToString());
                                        }
                                        firstRow = false;
                                    }
                                    else
                                    {
                                        //Add rows to DataTable.
                                        dt.Rows.Add();
                                        int j = 0;
                                        foreach (IXLCell cell in row.Cells())
                                        {
                                            dt.Rows[dt.Rows.Count - 1][j] = cell.Value.ToString();
                                            j++;
                                        }
                                    }

                                }

                                clsMain objMain = new clsMain();
                                SqlParameter[] objParam = new SqlParameter[2];


                                objParam[0] = new SqlParameter("@tblExcelData", SqlDbType.Structured);
                                objParam[0].Direction = ParameterDirection.Input;
                                objParam[0].Value = dt;

                                objParam[1] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
                                objParam[1].Direction = ParameterDirection.Input;
                                objParam[1].Value = files.Keys[i].ToString();


                                var result = objMain.ExecuteStoreProcedure("procHrEmpAttendanceUpload", objParam);

                                string msg = "";
                                if (result != null)
                                {
                                    if (result.Rows.Count > 0)
                                    {
                                        msg = result.Rows[0][0].ToString();
                                    }
                                }
                                context.Response.Write(msg);
                            }
                        }
                        else
                        {

                            img_save = 1;
                            BinaryReader br = new BinaryReader(file.InputStream);
                            if (files.Keys[i].Split('_')[0].ToString() == "profile")
                            {
                                profile_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "adhar")
                            {
                                adhar_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "voter")
                            {
                                voter_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "pan")
                            {
                                pan_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "passport")
                            {
                                passport_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "driving")
                            {
                                driving_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                            else if (files.Keys[i].Split('_')[0].ToString() == "bank")
                            {
                                bank_bytes = br.ReadBytes((Int32)file.InputStream.Length);
                                empid = files.Keys[i].Split('_')[1].ToString();
                            }
                        }

                    }
                }

                if (img_save == 1)
                {
                    clsMain objMain = new clsMain();
                    SqlParameter[] objParam = new SqlParameter[8];


                    objParam[0] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
                    objParam[0].Direction = ParameterDirection.Input;
                    objParam[0].Value = empid;


                    objParam[1] = new SqlParameter("@EmpPhotoImage", SqlDbType.VarBinary);
                    objParam[1].Direction = ParameterDirection.Input;
                    objParam[1].Value = profile_bytes;


                    objParam[2] = new SqlParameter("@AdharNoImage", SqlDbType.VarBinary);
                    objParam[2].Direction = ParameterDirection.Input;
                    objParam[2].Value = adhar_bytes;

                    objParam[3] = new SqlParameter("@VoterImage", SqlDbType.VarBinary);
                    objParam[3].Direction = ParameterDirection.Input;
                    objParam[3].Value = voter_bytes;

                    objParam[4] = new SqlParameter("@PanNoImage", SqlDbType.VarBinary);
                    objParam[4].Direction = ParameterDirection.Input;
                    objParam[4].Value = pan_bytes;

                    objParam[5] = new SqlParameter("@PassportImage", SqlDbType.VarBinary);
                    objParam[5].Direction = ParameterDirection.Input;
                    objParam[5].Value = passport_bytes;

                    objParam[6] = new SqlParameter("@DrivingNoImage", SqlDbType.VarBinary);
                    objParam[6].Direction = ParameterDirection.Input;
                    objParam[6].Value = driving_bytes;

                    objParam[7] = new SqlParameter("@BankImage", SqlDbType.VarBinary);
                    objParam[7].Direction = ParameterDirection.Input;
                    objParam[7].Value = bank_bytes;

                    var result = objMain.ExecuteProcedure("procHrEmpUpdateEmplyeeImages", objParam);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}