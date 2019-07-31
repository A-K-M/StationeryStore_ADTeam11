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
                $("#item").append("<option value='" + item.ID + "'>" + item.Description +"</option >");
            });            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}