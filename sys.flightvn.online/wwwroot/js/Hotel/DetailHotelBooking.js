$('#getLinkGateway').on('click', function (e) {
    e.preventDefault();
    let orderId = $(this).data('orderid');
    let inputLink = document.getElementById("inputLinkGateway");
    let areaString = document.getElementById("SettingAreaString").value;
    $.ajax({
        type: 'POST',
        url: `../../${areaString}/PaymentGateway/GetLinkPayment`,
        data: { orderId: orderId },
        success: function (response) {
            inputLink.value = response.link
        }
    })
})

$('#copyToClipboard').on('click', function (e) {
    e.preventDefault();
    let inputLink = document.getElementById("inputLinkGateway");
    inputLink.select();
    inputLink.setSelectionRange(0, 99999);
    document.execCommand('copy');
});

//$('#changeStatusBooking').on('click', function (e) {
//    e.preventDefault();
//    $('#loadingOverlay').css('display', 'flex');
//    var bookingCode = document.getElementById('bookingCode').value;
//    var statusID = document.getElementById('tinhTrang').value;
//    $.ajax({
//        type: 'POST',
//        url: '/Hotel/ChangeStatus',
//        data: {
//            bookingCode: bookingCode,
//            statusID: statusID
//        },
//        success: function (response) {
//            if (response.success) {
//                var statusClasses = {
//                    1: 'btn-danger',
//                    2: 'btn-info',
//                    3: 'btn-primary',
//                    4: 'btn-warning',
//                    5: 'btn-success',
//                    6: 'btn-success',
//                    7: 'btn-dark'
//                };

//                // Tạo nội dung HTML mới dựa trên statusID
//                var newStatusClass = statusClasses[statusID] || '';
//                var newStatusHtml = '<span style="padding: 5px 10px; border-radius: 15px;" class="' + newStatusClass + '">' + response.statusText + '</span>';

//                // Cập nhật nội dung cột trạng thái
//                $('#status-column_' + bookingCode).html(newStatusHtml);
//                $('#statusText_' + bookingCode).html(response.statusText);
//                $('#tinhTrang').val(statusID).trigger('change');
//                if (statusID >= 3) {
//                    $('#openOtherFeeModal').attr('disabled', 'disabled');
//                }
//                $('#loadingOverlay').css('display', 'none');

//                Swal.fire({
//                    imageUrl: "/images/success.png",
//                    imageWidth: 100,
//                    imageHeight: 100,
//                    title: response.message,
//                    confirmButtonText: 'Đóng',
//                });
//            }
//            else {
//                $('#loadingOverlay').css('display', 'none');
//                Swal.fire({
//                    imageUrl: "/images/fail.png",
//                    imageWidth: 100,
//                    imageHeight: 100,
//                    title: response.message,
//                    confirmButtonText: 'Đóng',
//                });
//            }
//        }
//    })
//})

$('#changeStatusBookingForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = $(this).serialize();
    $.ajax({
        type: 'POST',
        url: '../Hotel/ChangeStatus',
        data: formData,
        success: function (response) {
            if (response.success) {
                debugger;
                var statusClasses = {
                    1: 'btn-danger',
                    2: 'btn-info',
                    3: 'btn-primary',
                    4: 'btn-warning',
                    5: 'btn-success',
                    6: 'btn-success',
                    7: 'btn-dark'
                };

                // Tạo nội dung HTML mới dựa trên statusID
                var newStatusClass = statusClasses[response.statusId] || '';
                var newStatusHtml = '<span style="padding: 5px 10px; border-radius: 15px;" class="' + newStatusClass + '">' + response.statusText + '</span>';

                // Cập nhật nội dung cột trạng thái
                $('#status-column_' + response.bookingCode).html(newStatusHtml);
                $('#statusText_' + response.bookingCode).html(response.statusText);
                $('#tinhTrang').val(response.statusId).trigger('change');
                if (response.statusId >= 3) {
                    $('#openOtherFeeModal').attr('disabled', 'disabled');
                }

                if (response.statusId == 7) {
                    // Cập nhật nội dung của phần tử #cancelSection
                    $('#cancelSection').html(
                        '<div class="title-detail-BK">' +
                        '<div class="" style="line-height: 34px; padding: 0;">' +
                        'THÔNG TIN HUỶ' +
                        '</div>' +
                        '</div>' +
                        '<div class="row" style="padding: 0 10px;">' +
                        '<div class="col-sm-12 borber-table">' +
                        '<div class="row" style="">' +
                        '<label class="col-xs-2 control-label bg">Người huỷ</label>' +
                        '<div class="col-xs-10"><span id="hotelBooking_Canceller">' + response.canceller + '</span></div>' +
                        '</div>' +
                        '<div class="row" style="">' +
                        '<label class="col-xs-2 control-label bg">Lý do</label>' +
                        '<div class="col-xs-10"><span id="hotelBooking_CancelReason">' + response.cancelReason + '</span></div>' +
                        '</div>' +
                        '</div>' +
                        '</div>'
                    );
                }

                if (response.statusId == 7 || response.statusId == 6) {
                    $('#openModalChangeStatusBookingHotel').attr('disabled', 'disabled');
                }

                // Tạo dòng mới cho bảng
                var newRowHtml = '<tr>' +
                    '<td>1</td>' + 
                    '<td>' + response.statusText + '</td>' +
                    '<td>' + response.statusDescription + '</td>' +
                    '<td>' + response.createdDate + '</td>' +
                    '<td>' + response.createdBy + '</td>' +
                    '</tr>';

                // Chèn dòng mới vào đầu bảng
                var $tableBody = $('#customModal-statusHistory table tbody');
                $tableBody.prepend(newRowHtml);
                // Cập nhật lại STT cho tất cả các dòng
                $tableBody.find('tr').each(function (index) {
                    $(this).find('td:first').text(index + 1);
                });

                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
                $('#closeModalBtn-changeStatus').click();

            }
            else {
                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            }
        }
    })
})

$('#cancelBookingForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = $(this).serialize();
    $.ajax({
        type: 'POST',
        url: '../Hotel/CancelBooking',
        data: formData,
        success: function (response) {
            if (response.success) {

                $('#changeStatusBooking').attr('disabled', 'disabled');
                $('#sendBookingHotel').attr('disabled', 'disabled');
                $('#openModalCancelBookingHotel').attr('disabled', 'disabled');
                $('#openOtherFeeModal').attr('disabled', 'disabled');


                // Cập nhật nội dung của phần tử #cancelSection
                $('#cancelSection').html(
                    '<div class="title-detail-BK">' +
                    '<div class="" style="line-height: 34px; padding: 0;">' +
                    'THÔNG TIN HUỶ' +
                    '</div>' +
                    '</div>' +
                    '<div class="row" style="padding: 0 10px;">' +
                    '<div class="col-sm-12 borber-table">' +
                    '<div class="row" style="">' +
                    '<label class="col-xs-2 control-label bg">Người huỷ</label>' +
                    '<div class="col-xs-10"><span id="hotelBooking_Canceller">' + response.canceller + '</span></div>' +
                    '</div>' +
                    '<div class="row" style="">' +
                    '<label class="col-xs-2 control-label bg">Lý do</label>' +
                    '<div class="col-xs-10"><span id="hotelBooking_CancelReason">' + response.cancelReason + '</span></div>' +
                    '</div>' +
                    '</div>' +
                    '</div>'
                );



                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
                $('#closeModalBtn-cancel').click();
            }
            else {
                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            }
        }
    })
})

$('#otherFeeForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = $(this).serialize();
    $.ajax({
        type: 'POST',
        url: '../Hotel/UpdateOtherFee',
        data: formData,
        success: function (response) {
            if (response.success) {

                $('#otherFeeAmount').html(formatNumber(response.otherFee));
                $('#otherFeeReason').html(formatNumber(response.otherFeeReason));
                $('#totalPriceAmount').html(formatNumber(response.totalPrice));
                $('#remainingAmount').html(formatNumber(response.totalPrice));
                $('#totalPrice-column_' + response.bookingCode).html(formatNumber(response.totalPrice) + " VNĐ") ;
                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
                $('#closeModalBtn-otherFee').click();
            }
            else {
                $('#loadingOverlay').css('display', 'none');
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            }
        }
    })
})

var priceInputs = document.getElementsByClassName('price-input');
for (var i = 0; i < priceInputs.length; i++) {
    // Add input event listener to format input value
    priceInputs[i].addEventListener('input', function () {
        // Get input value and remove non-digit characters
        var inputValue = this.value.replace(/[^\d]/g, '');
        // Check if the input value is empty
        if (inputValue === '') {
            // If empty, set input value to an empty string
            this.value = '';
            return; // Exit the function
        }
        // Parse input value as float
        var number = parseFloat(inputValue);
        // Format number
        var formattedNumber = formatNumber(number);
        // Update input value with formatted number
        this.value = formattedNumber;
    });
}

function formatNumber(number) {
    return number.toLocaleString();
}

/// Mở modal OtherFee
var customModalOtherFee = document.getElementById('customModal-otherFee');
var openModalBtnOtherFee = document.getElementById('openOtherFeeModal');
var closeModalBtnOtherFee = document.getElementById('closeModalBtn-otherFee');
openModalBtnOtherFee.onclick = function () {
    customModalOtherFee.style.display = 'block';
}
closeModalBtnOtherFee.onclick = function () {
    customModalOtherFee.style.display = 'none';
}
window.onclick = function (event) {
    if (event.target === modal) {
        customModalOtherFee.style.display = 'none';
    }
}



/// Mở modal ChangeStatusBooking
var customModalChangeStatus = document.getElementById('customModal-changeStatus');
var openModalBtnChangeStatus = document.getElementById('openModalChangeStatusBookingHotel');
var closeModalBtnChangeStatus = document.getElementById('closeModalBtn-changeStatus');
openModalBtnChangeStatus.onclick = function () {
    customModalChangeStatus.style.display = 'block';
}
closeModalBtnChangeStatus.onclick = function () {
    customModalChangeStatus.style.display = 'none';
}
window.onclick = function (event) {
    if (event.target === modal) {
        customModalChangeStatus.style.display = 'none';
    }
}

/// Mở modal StatusHistory
var customModalStatusHistory = document.getElementById('customModal-statusHistory');
var openModalBtnStatusHistory = document.getElementById('openStatusHistoryModal');
var closeModalBtnStatusHistory = document.getElementById('closeModalBtn-statusHistory');
openModalBtnStatusHistory.onclick = function () {
    customModalStatusHistory.style.display = 'block';
}
closeModalBtnStatusHistory.onclick = function () {
    customModalStatusHistory.style.display = 'none';
}
window.onclick = function (event) {
    if (event.target === modal) {
        customModalStatusHistory.style.display = 'none';
    }
}