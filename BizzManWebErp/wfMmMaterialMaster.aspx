<%@ Page Title="" Language="C#" MasterPageFile="~/MmMainMenu.Master" AutoEventWireup="true" CodeBehind="wfMmMaterialMaster.aspx.cs" Inherits="BizzManWebErp.wfMmMaterialMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/style.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/moment.min.js"></script>
    <script src="Scripts/MmMaterialMaster.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="loginuser" runat="server" />
    <button onclick="CreateMaterial();">Create</button>
    <button onclick="ViewMaterialList();">View</button>
    <button onclick="AddDetails();" style="display: none;" id="btnSave">Save</button>

        <div class="container" id="divMaterialList" style="margin-top: 10px; overflow: auto;">
        <table id="tblMaterialList" class="display">
            <thead>
                <tr>
                    <th style="width: 5%;">Id</th>
                    <th style="width: 10%;">Material Category Name</th>
                    <th style="width: 30%;">Material Name</th>
                    <th style="width: 15%;">Unit Mesure</th>
                    <th style="width: 20%;">Description</th>
                </tr>
            </thead>
            <tbody id="tbody_Material_List">
            </tbody>
        </table>
    </div>

    <div class="container" id="divMaterialEntry" style="display: none; margin-top: 10px;">
        <div class="card">
            <div class="card-header">
                <b>Add Material</b>
            </div>
            <div class="card-body">
                <div class="panel panel-default">   
                    <div class="panel-body">
                        <table class="tbl">
                            <tr>
                                <td>
                                    <label class="control-label">Material Category Name *</label>
                                </td>
                                <td>
                                    <select style="width: 31%;" id="ddlMaterialCategoryName" name="ddlMaterialCategoryName" class="form-control rounded border-dark">
                                        <option value="">-Select Material Category Name-</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%;">Material Name *</td>
                                <td>
                                   <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtMaterialName" name="txtMaterialName" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="control-label">Unit Mesure *</label>
                                </td>
                                <td>
                                    <select style="width: 31%;" id="ddlUnitMesure" name="ddlUnitMesure" class="form-control rounded border-dark">
                                        <option value="">-Select Unit Mesure-</option>
                                    </select>
                                </td>
                            </tr>
                              <tr>
                                <td style="width: 15%;">Description</td>
                                <td>
                                   <input type="text" style="width: 31%;" class="form-control rounded border-dark" id="txtDescription" name="txtDescription" maxlength="100" />
                                </td>
                            </tr>
                           
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
