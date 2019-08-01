//clean the Item Options so that the old items will not be in select box
function clearOption() {
    $("#item")
        .children()
        .not(':first-child').remove();  
}

//Load Items base on Category
function loadItems(CategoryID) {
    $.ajax({
        url: "/StoreClerk/GetItemByCategory/" + CategoryID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            clearOption();

            $.each(result, function (key, item) {
                $("#item").append("<option value='" + item.Id + "'>" + item.Description +"</option >");
            });            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//functions for data table manipulation
function addItem() {

    //if (isNaN(quantity.value) || quantity.value == '' || item.value == 0 || reason.value == '') {

    //    alert("Please input valid data!");

    //    return false;
    //}

    let row = `
                <tr>
                    <td><input type="hidden" value="${$('#item option:selected').val()}">${$('#item option:selected').text()}</td>
                    <td><input type="text" value="${$('#quantity').val()}"></td>
                    <td>${$('#reason').val()}</td>
                    <td><button class="btn btn-outline-danger btn-sm" onclick="">Remove</button></td>
                </tr>`;

    let btnSubmit = `<div class="col-6">
                            <div class="row">
                              <div class="col-6"></div>
                              <div class="col-3">
                                <button class="btn btn-outline-success">Submit</button>
                              </div>
                            </div>
                          </div>
                        </div>`;


    $(row).insertAfter("#append-node");

    $("#item").val("0");
    $("#quantity").val("");
    $("#reason").val("");

    let rowCount = $("#data-body tbody tr").length -1 ;

    if (rowCount > 0) {
        //let something = document.getElementById('submit-placeholder');

        //something.innerHTML = btnSubmit;
        //alert(something);
        //$("#submit-placeholder").append(btnSubmit);

        $("#submit-placeholder").html(btnSubmit);
    }
    
}



//const addItem = () => {

//    counter += 1;

//    let trId = 'tr' + counter;

//    let item = document.getElementById('item');
//    let quantity = document.getElementById('quantity');
//    let reason = document.getElementById('reason');

//    if (isNaN(quantity.value) || quantity.value == '' || item.value == 0 || reason.value == '') {

//        alert("Please input valid data!");

//        return false;
//    }

//    let row = `
//           <tr id="${trId}">
//                <td><input type="hidden" value="${item.options[item.selectedIndex].value}">${item.options[item.selectedIndex].text}</td>
//                <td><input type="text" value="${quantity.value}"></td>
//                <td>${reason.value}</td>
//                <td><button class="btn btn-outline-danger btn-sm" onclick="removeItem(${trId})">Remove</button></td>
//           </tr>`;

//    let btnSubmit = `<div class="col-6">
//                            <div class="row">
//                              <div class="col-6"></div>
//                              <div class="col-3">
//                                <button class="btn btn-outline-success">Submit</button>
//                              </div>
//                            </div>
//                          </div>
//                        </div>`;

//    let itemData = document.getElementById('item-data');

//    itemData.innerHTML += row;

//    item.options[0].selected = true;
//    quantity.value = null;
//    reason.value = null;

//    if (checkTableRow()) {

//        let placeholder = document.getElementById('submit-placeholder');
//        placeholder.innerHTML = btnSubmit;
//    }
//};