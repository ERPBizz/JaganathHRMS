$(document).ready(function () {
    $("button").click(function (event) {
        event.preventDefault();
    });

    BindUnitMesureDropdown()
    BindMaterialCategoryNameDropdown();
    BindMaterialList();
});

function BindMaterialCategoryNameDropdown() {
    $.ajax({
        type: "POST",
        url: 'wfMmMaterialMaster.aspx/MaterialMasterList',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            var abranch = "";
            for (var i = 0; i < JSON.parse(response.d).length; i++) {
                abranch = abranch + "<option value='" + JSON.parse(response.d)[i].Id + "'>" + JSON.parse(response.d)[i].Name + "</option>";
            }
            $('#ddlMaterialCategoryName').append(abranch);
            // $('#ddlBankBranch').append(branch);
        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function BindUnitMesureDropdown() {
    $.ajax({
        type: "POST",
        url: 'wfMmMaterialMaster.aspx/UnitMesureList',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            var branch = "";
            for (var i = 0; i < JSON.parse(response.d).length; i++) {
                branch = branch + "<option value='" + JSON.parse(response.d)[i].Id + "'>" + JSON.parse(response.d)[i].UnitMesureName + "</option>";
            }
            $('#ddlUnitMesure').append(branch);
            // $('#ddlBankBranch').append(branch);
        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function BindMaterialList() {
    $.ajax({
        type: "POST",
        url: 'wfMmMaterialMaster.aspx/FetchMaterialList',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            var data = JSON.parse(response.d);
            $('#tblMaterialList').DataTable().clear();
            $('#tblMaterialList').DataTable().destroy();
            $('#tbody_Material_List').html('');
            var html = '';
            for (var i = 0; i < JSON.parse(response.d).length; i++) {
                html = html + '<tr onclick="FetchMaterialDetails(\'' + data[i].MaterialName + '\')"><td>' + data[i].Id + '</td>'
                    + '<td>' + data[i].Name + '</td>'
                    + '<td>' + data[i].MaterialName + '</td>'
                    + '<td>' + data[i].UnitMesureName + '</td>'
                    + '<td>' + data[i].Description + '</td>';
            }
            $('#tbody_Material_List').html(html);
            $('#tblMaterialList').DataTable();

        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function FetchMaterialDetails(name) {
    $.ajax({
        type: "POST",
        url: 'wfMmMaterialMaster.aspx/FetchMaterialDetails',
        data: JSON.stringify({
            "MaterialName": name
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

        },
        success: function (response) {
            ClearAll();
            $('#divMaterialList').hide();
            $('#divMaterialEntry').show();
            $("#btnSave").html('Update');
            $('#txtMaterialName').attr("readonly", "readonly");
            $("#btnSave").show();




            var data = JSON.parse(response.d);

            $('#txtMaterialName').val(data[0].MaterialName);
            $('#txtDescription').val(data[0].Description);

            setTimeout(function () {
                $('#ddlMaterialCategoryName').val(data[0].MaterialCategoryId);
                $('#ddlUnitMesure').val(data[0].UnitMesure);
            }, 1000);

        },
        complete: function () {

        },
        failure: function (jqXHR, textStatus, errorThrown) {

        }
    });

}
function CreateMaterial() {
    $('#divMaterialList').hide();
    $('#divMaterialEntry').show();
    $('#txtMaterialName').removeAttr("readonly");
    $("#btnSave").html('Save');
    $('#btnSave').show();
    ClearAll();
}

function ViewMaterialList() {
    $('#divMaterialList').show();
    $('#divMaterialEntry').hide();
    $('#btnSave').hide();
    BindMaterialList();
}

function ClearAll() {
    $('#ddlMaterialCategoryName,#txtMaterialName,#ddlUnitMesure,#txtDescription').val('');

}

function AddDetails() {

    var isUpdate = 0;
    var isValid = true;
    if ($('#btnSave').html() == 'Update') {
        isUpdate = 1;
    }

    if ($('#ddlMaterialCategoryName').val() == '') {
        alertify.error("Please Select Material Category");
        isValid = false;
    }
    else if ($('#txtMaterialName').val() == '') {
        alertify.error("Please Enter Material Name");
        isValid = false;
    }
    else if ($('#ddlUnitMesure').val() == '') {
        alertify.error("Please Select Unit Mesure");
        isValid = false;
    }
 
    if (isValid) {
        $.ajax({
            type: "POST",
            url: 'wfMmMaterialMaster.aspx/CheckMaterialAvailability',
            data: JSON.stringify({ "MaterialName": $('#txtMaterialName').val(), "isUpdate": isUpdate }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {

            },
            success: function (response) {
                var data = JSON.parse(response.d);
                if (data == 'False') {
                    $.ajax({
                        type: "POST",
                        url: 'wfMmMaterialMaster.aspx/AddDetails',
                        data: JSON.stringify({
                            "MaterialCategoryId": $('#ddlMaterialCategoryName').val().trim(),
                            "MaterialName": $('#txtMaterialName').val().trim(),
                            "UnitMesure": $('#ddlUnitMesure').val().trim(),
                            "Description": $('#txtDescription').val().trim(),
                            "loginUser": $('#ContentPlaceHolder1_loginuser').val()

                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function () {

                        },
                        success: function (response) {
                            UploadFiles();

                        },
                        complete: function () {

                        },
                        failure: function (jqXHR, textStatus, errorThrown) {

                        }
                    });
                }
                else {
                    alertify.error("Current Material Name already available");
                }
            },
            complete: function () {

            },
            failure: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
}

function UploadFiles() {
    var data = new FormData();

    $.ajax({
        url: "FileUploadHandler.ashx",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            if ($('#btnSave').html() == 'Update') {
                alertify.success("Material Master updated successfully");
            }
            else {
                alertify.success("Material Master added successfully");
                ClearAll();
            }

        },
        error: function (err) {
            alert(err.statusText)
        }
    });
}