let dateFrom = null;
let dateTo = null;
let pageSize = null;
let statusID = null;


$('body').on('click', '.Detail', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: "../KyThuat/ChiTietYeuCauDoan",
        data: {
            code: id
        },
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

// Chọn khoảng ngày
$(document).ready(function () {
    $('.datepicker.date-range').daterangepicker({
        locale: {
            format: 'DD/MM/YYYY',
            separator: " - ",
            applyLabel: "Áp dụng",
            cancelLabel: "Hủy",
            customRangeLabel: "Tùy chỉnh",
            daysOfWeek: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
            monthNames: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
        },
        ranges: {
            '30 ngày trước': [moment().subtract(29, 'days'), moment()],
            '7 ngày trước': [moment().subtract(6, 'days'), moment()],
            'Hôm nay': [moment()],
        },
        opens: 'left',
        singleDatePicker: false,
        showDropdowns: true,
        forceUpdate: true
    }, function (start, end, label) {
        pageSize = document.getElementById('Page_Size').value;
        dateFrom = start.format('YYYY-MM-DD');
        dateTo = end.format('YYYY-MM-DD');

        // Gọi AJAX lấy dữ liệu Search
        loadOrders(pageSize, dateFrom, dateTo)
    });

    // Apply button
    $('.datepicker.date-range').on('apply.daterangepicker', function (ev, picker) {
        // Cập nhật biến
        pageSize = document.getElementById('Page_Size').value;
        dateFrom = picker.startDate.format('YYYY-MM-DD');
        dateTo = picker.endDate.format('YYYY-MM-DD');
        loadOrders(pageSize, dateFrom, dateTo)

    });
    // Cancel Button
    $('.datepicker.date-range').on('cancel.daterangepicker', function (ev, picker) {
        $('.datepicker.date-range').val('');
        pageSize = document.getElementById('Page_Size').value;
        dateFrom = null;
        dateTo = null;
        loadOrders(pageSize, dateFrom, dateTo)

    });
});

// Chọn số lượng sản phẩm hiển thị
$(document).on('change', '#Page_Size', function (e) {
    pageSize = e.target.value;
    loadOrders(pageSize, dateFrom, dateTo)
});


//#region Phân trang
function loadPage(page) {
    loadOrders(pageSize, dateFrom, dateTo, page)
}
function loadOrders(pageSize, dateFrom, dateTo, page = 1) {
    $.post(
        '../KyThuat/GetAllRequestGroupBooking',
        {
            pageSize: pageSize,
            dateFrom: dateFrom,
            dateTo: dateTo,
            page: page
        },
        (data) => {
            $("#table_Pagination").html(data);
            $("html, body").animate({ scrollTop: 0 }, "slow"); // Kéo page lên đầu
        }
    );
}