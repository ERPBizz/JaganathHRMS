$(document).ready(function () {
    $("button").click(function (event) {
        event.preventDefault();
    });

    BindAccountGroupDropDown();
    BindFaLedgerMasterDetails();

    $('#ddlAccountGroup').change(GetBankAccountDetails);   
});

function GetBankAccountDetails() {
    var accountGroupId = $('#ddlAccountGroup').val();
    //alert('Account Group Id : ' + accountGroupId);
    FetchFaPrimeGroupDetails(accountGroupId);    
}

function BindFaLedgerMasterDetails() {
    $.ajax({
        type: "POST",
        url: 'wfFaLedgerMaster.aspx/FetchLedgerDetails',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            $('#tblFaLedgerMasterList').DataTable().clear();
            $('#tblFaLedgerMasterList').DataTable().destroy();
            $('#tbody_FaLedgerMaster_List').html('');
            var html = '';
            for (var i = 0; i < JSON.parse(response.d).length; i++) {
                html = html + '<tr onclick="FetchFaLedgerDetails(\'' + data[i].Id + '\' ,\'' + data[i].LedgerName + '\')">'
                    + '<td> ' + data[i].Id + '</td > '
                    + '<td> ' + data[i].LedgerName + '</td > '
                    + '<td> ' + data[i].GroupName + '</td > '
                    + '<td> ' + data[i].OpBalance + '</td > '
                    + '<td> ' + data[i].FinancialYear + '</td > '
                    + '<td> ' + data[i].CreateUser + '</td > '
                    + '<td>'  + data[i].Active + '</td>'
            }
            $('#tbody_FaLedgerMaster_List').html(html);
            $('#tblFaLedgerMasterList').DataTable();

        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function BindAccountGroupDropDown() {
    $.ajax({
        type: "POST",
        url: 'wfFaLedgerMaster.aspx/AccountGroupList',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            var department = "";
            for (var i = 0; i < JSON.parse(response.d).length; i++) {
                department = department + "<option value='" + JSON.parse(response.d)[i].Id + "'>" + JSON.parse(response.d)[i].GroupName + "</option>";
            }
            $('#ddlAccountGroup').append(department);
        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });
}

function FetchFaLedgerDetails(id, ledgerName) {
    $.ajax({
        type: "POST",
        url: 'wfFaLedgerMaster.aspx/FetchFaLedgerMasterDetails',
        data: JSON.stringify({
            "id": id,
            'ledgerName': ledgerName
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            ClearAll();
            $('#divFaLedgerMasterList').hide();
            $('#divFaLedgerMasterEntry').show();
            $("#btnSave").html('Update');
            $('#txtLedgerName,#txtOpeningBalance,#txtIfcsCode,#txtBankName,#txtBankBranchName,#txtBankAcNum').attr("readonly", "readonly");
            $("#btnSave").show();


            $('#txtLedgerId').val(data[0].Id);
            $('#txtLedgerName').val(data[0].LedgerName);
            $('#txtOpeningBalance').val(data[0].OpBalance);

            $('#txtIfcsCode').val(data[0].IfcsCode);
            $('#txtBankName').val(data[0].BankName);
            $('#txtBankBranchName').val(data[0].BankBranchName);
            $('#txtBankAcNum').val(data[0].BankAcNo);

            $('#ddlAccountGroup').val(data[0].GroupMasterId);
            $('#ddlAccountGroup').attr("disabled", true);
            GetBankAccountDetails();
            $('#chkActive').prop('checked', data[0].Active === "Y");

        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function FetchFaPrimeGroupDetails(id) {
    $.ajax({
        type: "POST",
        url: 'wfFaLedgerMaster.aspx/FetchFaPrimeGroupDetails',
        data: JSON.stringify({
            "id": id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            var primeGroupName = data[0].PrimeGroupName;

            if (primeGroupName == 'Bank account')
            {
                $('#tblAccountDetails').show();
                //$('#tblAccountDetails').removeAttr("hidden");
            }
            else {
                $('#tblAccountDetails').hide();
                //$('#tblAccountDetails').attr("hidden", "hidden");
            }
        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}


function ViewFaLedgerMasterList() {
    $('#divFaLedgerMasterList').show();
    $('#divFaLedgerMasterEntry').hide();
    $('#btnSave').hide();
    BindFaLedgerMasterDetails();
}


function ClearAll() {
  
    $('#txtLedgerName,#txtIfcsCode,#txtBankName,#txtBankBranchName,#txtBankAcNum,#ddlAccountGroup').val('');
    $('#chkActive').prop('checked', true);
    $('#txtOpeningBalance').val('0');
}

function CreateFaLedgerMaster() {
    $('#divFaLedgerMasterList').hide();
    $('#divFaLedgerMasterEntry').show();
    
    $('#txtLedgerName,#txtIfcsCode,#txtBankName,#txtBankBranchName,#txtBankAcNum,#txtOpeningBalance').removeAttr("readonly");
    $("#btnSave").html('Save');
    $('#btnSave').show();
    ClearAll();
    $('#ddlAccountGroup').attr("disabled", false);
    $('#tblAccountDetails').hide();
}

function AddDetails()
{

    var isUpdate = 0;
    var isValid = true;
    if ($('#btnSave').html() == 'Update') {
        isUpdate = 1;
    }

    if ($('#txtLedgerName').val() == '') {
        alertify.error("Please Enter Ledger Name");
        isValid = false;
    }
    else if ($('#ddlAccountGroup').val() == '') {
        alertify.error("Please Enter Group Name"); 
        isValid = false;
    }
    if ($('#txtOpeningBalance').val() == '') {
        alertify.error("Please Enter Opening Balance");
        isValid = false;
    }
    else if ($('#tblAccountDetails').is(':visible'))
    {
        if ($('#txtIfcsCode').val() == '') {
            alertify.error("Please Enter Ifcs Code");
            isValid = false;
        }
        else if ($('#txtBankName').val() == '') {
            alertify.error("Please Enter Bank Name");
            isValid = false;
        }
        else if ($('#txtBankBranchName').val() == '') {
            alertify.error("Please Enter Branch Name");
            isValid = false;
        }
        else if ($('#txtBankAcNum').val() == '') {
            alertify.error("Please Enter Bank Account Number");
            isValid = false;
        }
    } 
        
    if (isValid)
    {
        if (isUpdate == 0) {
            $.ajax(
                {
                    type: "POST",
                    url: 'wfFaLedgerMaster.aspx/CheckLedgerAvailability',
                    data: JSON.stringify({
                        'ledgerName': $('#txtLedgerName').val(),
                        "isUpdate": isUpdate
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {

                    },
                    success: function (response) {
                        var data = JSON.parse(response.d);

                        if (data == 'False') {
                            $.ajax({
                                type: "POST",
                                url: 'wfFaLedgerMaster.aspx/AddDetails',
                                data: JSON.stringify({
                                    "groupMasterId": $('#ddlAccountGroup').val().trim(),
                                    "ledgerName": $('#txtLedgerName').val().trim(),
                                    "ifcsCode": $('#txtIfcsCode').val().trim(),
                                    "bankName": $('#txtBankName').val().trim(),
                                    "bankBranchName": $('#txtBankBranchName').val().trim(),
                                    "bankAcNo": $('#txtBankAcNum').val().trim(),
                                    "opBalance": $('#txtOpeningBalance').val().trim(),
                                    "clBalance": $('#txtOpeningBalance').val().trim(),
                                    "active": $('#chkActive').prop('checked') === true ? "Y" : "N",
                                    "loginUser": $('#ContentPlaceHolder1_loginuser').val()
                                }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () {

                                },
                                success: function (response) {
                                   
                                   alertify.success("Ledger added successfully");
                                   ClearAll();
                                   $('#tblAccountDetails').hide();
                                },
                                complete: function () {

                                },
                                failure: function (jqXHR, textStatus, errorThrown) {

                                }
                            });
                        }
                        else {
                            alertify.error("Current Ledger Details already available");
                        }
                    },
                    complete: function () {

                    },
                    failure: function (jqXHR, textStatus, errorThrown) {

                    }
                });
        }
        else if (isUpdate == 1)
        {
            $.ajax({
                type: "POST",
                url: 'wfFaLedgerMaster.aspx/UpdateLedgerDetails',
                data: JSON.stringify({
                    "ledgerId": $('#txtLedgerId').val().trim(),
                    "groupMasterId": $('#ddlAccountGroup').val().trim(),
                    "ledgerName": $('#txtLedgerName').val().trim(),
                    "ifcsCode": $('#txtIfcsCode').val().trim(),
                    "bankName": $('#txtBankName').val().trim(),
                    "bankBranchName": $('#txtBankBranchName').val().trim(),
                    "bankAcNo": $('#txtBankAcNum').val().trim(),
                    "opBalance": $('#txtOpeningBalance').val().trim(),
                    "clBalance": $('#txtOpeningBalance').val().trim(),
                    "active": $('#chkActive').prop('checked') === true ? "Y" : "N",
                    "loginUser": $('#ContentPlaceHolder1_loginuser').val()
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {

                },
                success: function (response) {
                    if ($('#btnSave').html() == 'Update') {
                       alertify.success("Ledger updated successfully");
                    }
                },
                complete: function () {

                },
                failure: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
    }
}
