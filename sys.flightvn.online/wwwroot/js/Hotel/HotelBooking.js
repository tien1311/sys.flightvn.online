let pageSize = 10;


//#region Phân trang
function loadPage(page) {
    let BookingCode = document.getElementById('BookingCode').value;
    let FullName = document.getElementById('FullName').value;
    let Email = document.getElementById('Email').value;
    let Phone = document.getElementById('Phone').value;
    let StatusID = document.getElementById('StatusID').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(BookingCode, FullName, Email, Phone, StatusID, fromDate, toDate, pageSize, page)
}


function loadOrders(BookingCode, FullName, Email, Phone, StatusID, fromDate, toDate, pageSize, page = 1) {
    $.post(
        '../Hotel/GetAllHotelBookingByFilter',
        {
            BookingCode: BookingCode,
            FullName: FullName,
            Email: Email,
            Phone: Phone,
            StatusID: StatusID,
            fromDate: fromDate,
            toDate: toDate,
            pageSize: pageSize,
            page: page
        },
        (data) => {
            $("#table_Pagination").html(data);
            $("html, body").animate({ scrollTop: 0 }, "slow"); // Kéo page lên đầu
        }
    );
}

// Chọn số lượng sản phẩm hiển thị
$(document).on('change', '#Page_Size', function (e) {
    let BookingCode = document.getElementById('BookingCode').value;
    let FullName = document.getElementById('FullName').value;
    let Email = document.getElementById('Email').value;
    let Phone = document.getElementById('Phone').value;
    let StatusID = document.getElementById('StatusID').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    pageSize = e.target.value;
    loadOrders(BookingCode, FullName, Email, Phone, StatusID, fromDate, toDate, pageSize)
});

$(document).on('submit', '#searchForm', function (e) {
    e.preventDefault();
    let BookingCode = document.getElementById('BookingCode').value;
    let FullName = document.getElementById('FullName').value;
    let Email = document.getElementById('Email').value;
    let Phone = document.getElementById('Phone').value;
    let StatusID = document.getElementById('StatusID').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(BookingCode, FullName, Email, Phone, StatusID, fromDate, toDate, pageSize)
})


$(document).ready(function () {
    $(document).on('click', '.Detail', function () {
        $('#loadingOverlay').css('display', 'flex');
        var $this = $(this);
        var bookingCode = $this.data('id');
        var statusId = $this.data('status-id') || 9999; 
        $.ajax({
            type: 'POST',
            url: '../Hotel/DetailHotelBooking',
            data: {
                bookingCode: bookingCode,
                statusId: statusId
            },
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
                if (statusId == 1) {
                    var newStatusHtml = '<span style="padding: 5px 10px; border-radius: 15px;" class="btn-info">Đã nhận Booking</span>';
                    $('#status-column_' + bookingCode).html(newStatusHtml);
                    $this.removeData('status-id');
                }

                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });
            },
            failure: function (response) {
                $('#loadingOverlay').css('display', 'none');
                alert(response.responseText);
            },
            error: function (response) {
                $('#loadingOverlay').css('display', 'none');
                alert(response.responseText);
            }
        });
    });
})

