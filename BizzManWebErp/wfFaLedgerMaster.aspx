<%@ Page Title="" Language="C#" MasterPageFile="~/FaMainMenu.Master" AutoEventWireup="true" CodeBehind="wfFaLedgerMaster.aspx.cs" Inherits="BizzManWebErp.wfFaLedgerMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/alertify.css" rel="stylesheet" />
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/alertify.js"></script>
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.dataTables.min.js"></script>
    <link href="Scripts/buttons.dataTables.min.css" rel="stylesheet" />
    <script src="Scripts/dataTables.buttons.min.js"></script>
    <script src="Scripts/jszip.min.js"></script>
    <script src="Scripts/buttons.html5.min.js"></script>
    <link href="css/select2.min.css" rel="stylesheet" />
    <script src="Scripts/select2.min.js"></script>
    
    <link href="css/style.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/moment.min.js"></script>
    <script src="Scripts/FaLedgerMaster.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="loginuser" runat="server" />
    <button onclick="CreateFaLedgerMaster();">Create</button>
    <button onclick="ViewFaLedgerMasterList();">View</button>
    <button onclick="AddDetails();" style="display: none;" id="btnSave">Save</button>

     <div class="container" id="divFaLedgerMasterList" style="margin-top: 10px; overflow: auto;">
        <table id="tblFaLedgerMasterList" class="display">
            <thead>
                <tr>
                    <th style="width: 5%;">Ledger Id</th>
                    <th style="width: 10%;">Ledger Name</th>
                    <th style="width: 30%;">Group Name</th>
                    <th style="width: 15%;">Opening Balance</th>
                    <th style="width: 20%;">Financial Year</th>
                    <th style="width: 15%;">Created By</th>
                    <th style="width: 5%;">Active</th>
                </tr>
            </thead>
            <tbody id="tbody_FaLedgerMaster_List">
            </tbody>
        </table>
    </div>

    <div class="container" id="divFaLedgerMasterEntry" style="display: none; margin-top: 10px;">
        <div class="card">
            <div class="card-header">
                <b>Add Ledger Master New</b>
            </div>
            <div class="card-body">
                <div class="panel panel-default">   
                    <div class="panel-body">
                        <table class="tbl">
                            <tr>
                                <td style="width: 18%;">Ledger Name *</td>
                                <td>
                                   <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtLedgerName" name="txtLedgerName" maxlength="100" />
                                   <input type="text" style="display:none;" class="form-control rounded border-dark" id="txtLedgerId" name="txtLedgerId" />
                                </td>
                            </tr>
                              <tr>
                                <td style="width: 18%;">Group Name *</td>
                                <td>
                                      <select style="width: 31%;" id="ddlAccountGroup" name="ddlAccountGroup" class="form-control rounded border-dark">
                                        <option value="">-Select Group-</option>
                                    </select>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 18%;">Opening Balance *</td>
                                <td>
                                   <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtOpeningBalance" name="txtOpeningBalance" value="0" maxlength="18" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table id="tblAccountDetails" name="tblAccountDetails" class="tbl" style="display:none;">
                                        <tr>
                                            <td style="width: 18%;">Ifcs Code *</td>
                                            <td>
                                               <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtIfcsCode" name="txtIfcsCode" maxlength="100" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td style="width: 18%;">Bank Name *</td>
                                            <td>
                                               <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtBankName" name="txtBankName" maxlength="100" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td style="width: 18%;">Bank Branch Name *</td> 
                                            <td>
                                                <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtBankBranchName" name="txtBankBranchName" maxlength="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 18%;">Bank Account Number *</td>
                                            <td>
                                                <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtBankAcNum" name="txtBankAcNum" maxlength="100" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 11%;">Active</td>
                                <td colspan="3">
                                    <input type="checkbox" placeholder="Enter Description" id="chkActive" name="chkActive" checked/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
