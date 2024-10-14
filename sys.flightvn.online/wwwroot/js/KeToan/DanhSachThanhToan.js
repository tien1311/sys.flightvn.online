document.getElementById('fromDate').addEventListener('change', function () {
    var fromDate = this.value;
    var toDateInput = document.getElementById('toDate');

    // Đặt thuộc tính min cho ô toDate
    toDateInput.min = fromDate;

    // Nếu ngày trong ô toDate trước ngày fromDate, xóa giá trị của nó
    if (toDateInput.value && toDateInput.value < fromDate) {
        toDateInput.value = '';
        Swal.fire({
            imageUrl: "/images/fail.png",
            imageWidth: 100,
            imageHeight: 100,
            title: "Thao tác thất bại",
            text: "Vui lòng chọn ngày/tháng hợp lệ",
            button: "Đóng",
        });
    }
});

var currentPage = 1;
var pageSize = 10;

function updatePagination(totalPages) {


    var paginationHtml = '<li class="btn " id="prevPage"' + (currentPage == 1 ? ' disabled style ="cursor:not-allowed"' : '') + '> <i class="fa fa-angle-left" aria-hidden="true"></i> </li>';

    for (var i = 1; i <= totalPages; i++) {
        if (i == 1 || i == totalPages || (i >= currentPage - 2 && i <= currentPage + 2)) {
            paginationHtml += '<li class="btn ' + (i == currentPage ? 'active' : '') + ' page-btn">' + i + '</li>';
        } else if (i == currentPage - 3 || i == currentPage + 3) {
            paginationHtml += '<li> ... </li>';
        }
    }

    paginationHtml += '<li class="btn"' + (currentPage == totalPages ? ' disabled style ="cursor:not-allowed"' : 'id="nextPage"') + '> <i class="fa fa-angle-right" aria-hidden="true"></i> </li>';

    $('#pagination').html(paginationHtml);
}
$(document).ready(function () {
    $("#fromDate, #toDate, #orderId, #paymentType, #resultCode").on("change", function () {
        // Đặt currentPage về 1
        currentPage = 1;
    });
    $("#searchForm").on("submit", function (event) {
        event.preventDefault();
        var formData = $(this).serialize() + "&pageNumber=" + currentPage + "&pageSize=" + pageSize;

        $.ajax({
            type: 'POST',
            data: formData,
            url: '../Ketoan/SearchThanhToan',
            success: function (data) {
                $('#userpayData').html(data);
                var totalPage = parseInt(document.getElementById('totalPagesInput').value);
                $('#currentPage').text(currentPage);
                $('#totalPages').text(totalPage);
                updatePagination(totalPage);
            }
        })
    });

    $("#pagination").on("click", ".page-btn", function () {
        currentPage = parseInt($(this).text());
        $("#searchForm").submit();
    });
    $("#pagination").on("click", "#prevPage", function () {
        if (currentPage > 1) {
            currentPage--;
            $("#searchForm").submit();
        }
    });
    $("#pagination").on("click", "#nextPage", function () {
        currentPage++;
        $("#searchForm").submit();
    });

    $("#btnExportExcel").on("click", function () {
        var formData = $("#searchForm").serialize();
        $.ajax({
            type: 'POST',
            data: formData,
            url: '../Ketoan/ExportThanhToan',
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = 'ThanhToanOnline_KeToan.xlsx';
                link.click();
            },
            error: function () {
                alert("Có lỗi khi xuất Excel, vui lòng liên hệ IT");
            }
        });
    });
});
