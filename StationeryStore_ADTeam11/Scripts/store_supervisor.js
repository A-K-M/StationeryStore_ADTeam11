/*function loadItems(CategoryID) {

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
}*/

/**
 * 
 *                     <tr>
                        <td>@voucher.EmployeeId</td>
                        <td>Stock Adjustment  @voucher.Id</td>
                        <td>@voucher.Date.ToString("dd/MM/yyyy")</td>
                        <td>@voucher.Status</td>
                        <td><a href="#" class="btn btn-outline-info">View</a></td>
                    </tr>
 */

function filterVouchers(status) {
    $.ajax({
        url: "/StoreSupervisor/FilterAdjustmentVouchers/" + status,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            $("#vouchers").remove();

            let tableData = '<tbody id="vouchers">';
            let issuedDate;

            $.each(result, function (key, item) {

                issuedDate = convertDate(new Date(parseInt(item.Date.substr(6))));

                tableData += `<tr>
                                    <td>${item.Name}</td>
                                    <td>Stock Adjustment ${item.Id}</td>
                                    <td>${issuedDate}</td>
                                    <td>${item.TotalQuantity}</td>
                                    <td>${item.Status}</td>
                                    <td><a href="#" class="btn btn-outline-info">View</a></td>
                                </tr>`;
            });

            tableData += '</tbody>'

            $("#voucher-table").append(tableData);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function convertDate(date) {
    function pad(s) { return (s < 10) ? '0' + s : s; }

    return [pad(date.getDate()), pad(date.getMonth() + 1), date.getFullYear()].join('/');
}