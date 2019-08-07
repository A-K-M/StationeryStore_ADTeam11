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
                                    <td><a href="/StoreSupervisor/VoucherItems/${item.Id}" class="btn btn-outline-info">View</a></td>
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