﻿function clearOption() {
    $("#item")
        .children()
        .not(':first-child').remove();
}

//Load Items base on Category
function loadItems(CategoryID) {

    $.ajax({
        url: "/DepartmentEmployee/GetItemByCategory/" + CategoryID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            clearOption();

            $.each(result, function (key, item) {
                $("#item").append("<option value='" + item.Id + "'>" + item.Description + "</option >");
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Adjustment Voucher
function addVoucher() {

    var items = createJSONList();

    $.ajax({
        url: "/DepartmentEmployee/AddStationeryRequest",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: JSON.stringify(items),
        success: function (result) {
            alert(result);
            location.href = "/DepartmentEmployee/CreateStationeryRequest";
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//functions for data table manipulation
function addItem() {

    if (isNaN($("#quantity").val()) || $("#quantity").val() == '' || $("#item").val() == 0 || $("#reason").val() == '') {

        alert("Please input valid data!");

        return false;
    }

    let row = `<tr>
                    <td><input type="hidden" class="item-id" value="${$('#item option:selected').val()}">${$('#item option:selected').text()}</td>
                    <td><input type="text" class="item-qty" value="${$('#quantity').val()}"></td>
                    <td><button class="btn btn-danger" btn-sm" onclick="removeRow(this)">Remove</button></td>
                </tr>`;

    let btnSubmit = `<div class="col-6">
                            <div class="row">
                              <div class="col-6"></div>
                              <div class="col-3">
                                <button class="btn btn-success" onclick="addVoucher()">Submit</button>
                              </div>
                            </div>
                          </div>
                        </div>`;


    $(row).insertAfter("#append-node");

    $("#item").val("0");
    $("#quantity").val("");
    $("#reason").val("");

    let rowCount = $("#data-body tbody tr").length - 1;

    if (rowCount > 0) {
        $("#submit-placeholder").html(btnSubmit);
    }
}

function removeRow(element) {

    element.parentNode.parentNode.remove();

    if ($("#data-body tbody tr").length - 1 == 0) {

        $("#submit-placeholder").empty();
    }
}

function generateDynamicIds() {

    let index = $("#item-data > tr");

    for (var i = 1; i <= index.length; i++) {

        if (index[i] === undefined) {
            return false;
        }

        index[i].setAttribute("id", "tr_" + i); //set id for each tr

        index[i].cells[0].childNodes[0].setAttribute("id", "item_" + i); //set id for hidden box within tr

        index[i].cells[1].childNodes[0].setAttribute("id", "qty_" + i); //set id for qty input type
    }
}

function createJSONList() {

    generateDynamicIds();

    let total_tr = $("#item-data > tr");

    let jsonObj = [];

    for (let i = 1; i <= total_tr.length - 1; i++) {

        let item = {};

        item["itemId"] = $("#item_" + i).val();
        item["quantity"] = $("#qty_" + i).val();

        jsonObj.push(item);
    }

    return jsonObj;
}