using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Newtonsoft.Json;

namespace BizzManWebErp
{
    public partial class wfMmMaterialPurchaseGateInword : System.Web.UI.Page
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
        public static string PurchaseOrderList()
        {
            clsMain objMain = new clsMain();
            DataTable dtPurchaseOrderList = new DataTable();

            try
            {

                dtPurchaseOrderList = objMain.dtFetchData("select Id FROM tblMmMaterialPurchaseOrderEntryMaster where Active='Y'");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtPurchaseOrderList);
        }

        [WebMethod]
        public static string WarehouseList()
        {
            clsMain objMain = new clsMain();
            DataTable dtWarehouseList = new DataTable();

            try
            {

                dtWarehouseList = objMain.dtFetchData("select Id,Name FROM tblFaWarehouseMaster");
            }
            catch (Exception ex)
            {
                return "";
            }

            return JsonConvert.SerializeObject(dtWarehouseList);
        }



        [WebMethod]
        public static string FetchMaterialPurchaseGateInwordList()
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialPurchaseGateInwordList = new DataTable();

            try
            {

                dtMaterialPurchaseGateInwordList = objMain.dtFetchData(@"select GI.Id,v.VendorName,CONVERT(nvarchar,GI.InwardEntryDate,106) as InwardEntryDate,GI.OrderId,
                                              GI.[PONo],GI.[Transporter],GI.[VehicleNo],GI.[DeliveryTerms],CONVERT(nvarchar,GI.Deadline,106) as DeadlineDate,GI.[ChallanNo]
                                             ,CONVERT(varchar(15),GI.[GateInTime],100) as GateInTime,CONVERT(varchar(15),GI.[GaeOutTime],100) as GaeOutTime,GI.[PaymentTerms],
                                              b.BranchName,w.Name as WarehouseName
                                              from tblMmMaterialPurchaseGateInwardMaster GI
                                              join tblMmVendorMaster v on GI.VendorId=v.Id
                                              join tblHrBranchMaster b on b.BranchCode=GI.BranchCode 
                                              join tblFaWarehouseMaster w on w.Id=GI.WarehouseId
                                              where GI.Active='Y'");
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
            return JsonConvert.SerializeObject(dtMaterialPurchaseGateInwordList, settings);
        }



        [WebMethod]
        public static string FetchPurchaseGateInwordDetails(string PurchaseGateInwordId)
        {
            clsMain objMain = new clsMain();
            DataTable dtPurchaseQuotationMasterDetailList = new DataTable();

            try
            {

                dtPurchaseQuotationMasterDetailList = objMain.dtFetchData(@"select GI.Id,m.MaterialName,GI.QtyOrder,GI.QtyReceive,GI.UnitPrice,GI.TotalAmt,GI.TotalAmt
                                                  from tblMmMaterialPurchaseGateInwardDetail GI
                                                  join tblMmMaterialMaster m on GI.MaterialMasterId=m.Id
                                                  where GI.Active='Y' and GI.PurchaseGateInwardMasterId='" + PurchaseGateInwordId + "'");
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
            return JsonConvert.SerializeObject(dtPurchaseQuotationMasterDetailList, settings);
        }




        [WebMethod]
        public static string FetchMaterialPurchaseGateInwordListDownload(string GateInwordid = "")
        {
            clsMain objMain = new clsMain();
            DataTable dtMaterialPurchaseOrderList = new DataTable();

            try
            {

                dtMaterialPurchaseOrderList = objMain.dtFetchData(@"select GI.Id,v.VendorName,CONVERT(nvarchar,GI.InwardEntryDate,106) as InwardEntryDate,GI.OrderId,
                                              GI.[PONo],GI.[Transporter],GI.[VehicleNo],GI.[DeliveryTerms],CONVERT(nvarchar,GI.Deadline,106) as DeadlineDate,GI.[ChallanNo]
                                             ,CONVERT(varchar(15),GI.[GateInTime],100) as GateInTime,CONVERT(varchar(15),GI.[GaeOutTime],100) as GaeOutTime,GI.[PaymentTerms],
                                              b.BranchName,w.Name as WarehouseName
                                              from tblMmMaterialPurchaseGateInwardMaster GI
                                              join tblMmVendorMaster v on GI.VendorId=v.Id
                                              join tblHrBranchMaster b on b.BranchCode=GI.BranchCode 
                                              join tblFaWarehouseMaster w on w.Id=GI.WarehouseId
                                              where GI.Active='Y'" + (GateInwordid != "" ? " and GI.Id in(SELECT Item FROM [dbo].[SplitString] ('" + GateInwordid + "',','))" : ""));
            }
            catch (Exception ex)
            {
                // return "";
            }
            dtMaterialPurchaseOrderList.TableName = "MaterialPurchaseGateInwordList";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dtMaterialPurchaseOrderList);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    byte[] bytes = stream.ToArray();

                    //Convert File to Base64 string and send to Client.
                    return Convert.ToBase64String(bytes, 0, bytes.Length);
                }
            }

        }




        [WebMethod]
        public static string FetchPurchaseOrderDetails(string OrderId)
        {
            clsMain objMain = new clsMain();
            DataTable dtPurchaseOrderDetails = new DataTable();

            try
            {

                dtPurchaseOrderDetails = objMain.dtFetchData(@"select OE.Id,M.MaterialName,OE.QtyOrder,OE.UnitPrice,OE.QtyReceive
                                                                          from tblMmMaterialPurchaseOrderEntryDetail OE
                                                                          join tblMmMaterialMaster M on M.Id=OE.MaterialMasterId
                                                                          where OE.Active='Y' and OE.PurchaseOrderMasterId='"+ OrderId + "'");
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
            return JsonConvert.SerializeObject(dtPurchaseOrderDetails, settings);
        }


        [WebMethod]
        public static string FetchPurchaseOrderMasterDetails(string OrderId)
        {
            clsMain objMain = new clsMain();
            DataTable dtPurchaseOrderMasterDetails = new DataTable();

            try
            {

                dtPurchaseOrderMasterDetails = objMain.dtFetchData(@"select V.VendorName,B.BranchName
                                                              from tblMmMaterialPurchaseOrderEntryMaster OE
                                                              join tblMmVendorMaster V on V.Id=OE.VendoreId
                                                              join tblHrBranchMaster B on B.BranchCode=OE.BranchCode
                                                              where OE.Id='" + OrderId + "'");
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
            return JsonConvert.SerializeObject(dtPurchaseOrderMasterDetails, settings);
        }



        [WebMethod]
        public static string AddMaterialPurchaseGateInword(string OrderID = "", string EntryDate = "", string Warehouse = "", string order_details = "", string LoginUser = "",
                                                           string DeadlineDate="",string PONo="",string Transporter="",string ChallanNo="",
                                                           string VehicleNo="",string GateInTime="",string GateOutTime="",string DeliveryTerm="",
                                                           string PaymentTerm="")
        {

            clsMain objMain = new clsMain();
            SqlParameter[] objParam = new SqlParameter[14];


            objParam[0] = new SqlParameter("@OrderID", SqlDbType.NVarChar);
            objParam[0].Direction = ParameterDirection.Input;
            objParam[0].Value = OrderID;


            objParam[1] = new SqlParameter("@EntryDate", SqlDbType.DateTime);
            objParam[1].Direction = ParameterDirection.Input;
            objParam[1].Value = Convert.ToDateTime(EntryDate);


            objParam[2] = new SqlParameter("@WarehouseId", SqlDbType.Int);
            objParam[2].Direction = ParameterDirection.Input;
            objParam[2].Value = Convert.ToInt32(Warehouse);


            objParam[3] = new SqlParameter("@order_details", SqlDbType.NVarChar);
            objParam[3].Direction = ParameterDirection.Input;
            objParam[3].Value = order_details;

            objParam[4] = new SqlParameter("@CreateUser", SqlDbType.NVarChar);
            objParam[4].Direction = ParameterDirection.Input;
            objParam[4].Value = LoginUser;

            objParam[5] = new SqlParameter("@PONo", SqlDbType.NVarChar);
            objParam[5].Direction = ParameterDirection.Input;
            objParam[5].Value = PONo;

            objParam[6] = new SqlParameter("@Transporter", SqlDbType.NVarChar);
            objParam[6].Direction = ParameterDirection.Input;
            objParam[6].Value = Transporter;

            objParam[7] = new SqlParameter("@VehicleNo", SqlDbType.NVarChar);
            objParam[7].Direction = ParameterDirection.Input;
            objParam[7].Value = VehicleNo;

            objParam[8] = new SqlParameter("@DeliveryTerms", SqlDbType.NVarChar);
            objParam[8].Direction = ParameterDirection.Input;
            objParam[8].Value = DeliveryTerm;

            objParam[9] = new SqlParameter("@DeadlineDate", SqlDbType.DateTime);
            objParam[9].Direction = ParameterDirection.Input;
            objParam[9].Value = Convert.ToDateTime(DeadlineDate);

            objParam[10] = new SqlParameter("@ChallanNo", SqlDbType.NVarChar);
            objParam[10].Direction = ParameterDirection.Input;
            objParam[10].Value = ChallanNo;

            objParam[11] = new SqlParameter("@GateInTime", SqlDbType.Time);
            objParam[11].Direction = ParameterDirection.Input;
            objParam[11].Value = DateTime.Parse(GateInTime).TimeOfDay;

            objParam[12] = new SqlParameter("@GateOutTime", SqlDbType.Time);
            objParam[12].Direction = ParameterDirection.Input;
            objParam[12].Value = DateTime.Parse(GateOutTime).TimeOfDay;

            objParam[13] = new SqlParameter("@PaymentTerms", SqlDbType.NVarChar);
            objParam[13].Direction = ParameterDirection.Input;
            objParam[13].Value = PaymentTerm;

            var result = objMain.ExecuteStoreProcedure("procMaterialPurchaseGateInwordInsert", objParam);

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



    }
}