
function sendDataBtn() {
    Swal.fire({
        title: 'Xác nhận thao tác',
        text: "Bạn có chắc chắn muốn gửi booking không?",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Bỏ qua',
    }).then((result) => {
        if (result.isConfirmed) {
            // Hiển thị hiệu ứng loading
            $('#custom-loading-overlay').show();
            var TourCode = document.getElementById("TourCodeInputValue").value;
            var codetrienkhai = document.getElementById("codetrienkhai").value;
            $.ajax({
                type: "POST",
                url: "../DuLich/Guimailbooking",
                data: {
                    TourCode: TourCode,
                    codetrienkhai: codetrienkhai
                },
                success: function (response) {
                    // Ẩn hiệu ứng loading sau khi nhận phản hồi từ máy chủ
                    $('#custom-loading-overlay').hide();
                    if (response.success) {
                        debugger;
                        var element = document.getElementById("IconTrangThai(" + TourCode + ")");
                        var tinhTrangElement = document.getElementById("tinhtrang");
                        if (element) {
                            switch (response.idStatus) {
                                case "3":
                                    element.innerHTML = `<span class="btn-primary rounded-text"><span>Đã giữ chỗ</span><input type="hidden" id="tinhtrang" value="${response.idStatus}" /></span>`;
                                    tinhTrangElement.innerText = `Đã giữ chỗ`
                                    break;
                                default:
                                    element.innerHTML = '<span class="btn-danger rounded-text"><span>Chưa có giá trị IDStatus</span></span>'; //  Đoạn này dùng để nhận biết lỗi trường hợp chưa có IDStatus
                                    break;
                            }
                        }
                        // Đổi select list
                        var SelectElement = document.getElementById("TinhTrang" + "(" + TourCode + ")");
                        SelectElement.innerHTML = ""; // Xoá
                        var OptionsElement1 = document.createElement("option");
                        OptionsElement1.setAttribute("value", "3");
                        OptionsElement1.textContent = "Đã giữ chỗ";

                        var OptionsElement2 = document.createElement("option");
                        OptionsElement2.setAttribute("value", "2");
                        OptionsElement2.textContent = "Đã tiếp nhận";

                        var OptionsElement3 = document.createElement("option");
                        OptionsElement3.setAttribute("value", "4");
                        OptionsElement3.textContent = "Đã đặt cọc";

                        var OptionsElement4 = document.createElement("option");
                        OptionsElement4.setAttribute("value", "5");
                        OptionsElement4.textContent = "Hoàn tất thanh toán";

                        SelectElement.appendChild(OptionsElement1);
                        SelectElement.appendChild(OptionsElement2);
                        SelectElement.appendChild(OptionsElement3);
                        SelectElement.appendChild(OptionsElement4);


                        Swal.fire({
                            imageUrl: "/images/success.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: 'Bạn đã giữ chỗ thành công',
                            confirmButtonText: 'Đóng',
                        });


                    } else {
                        Swal.fire({
                            imageUrl: "/images/fail.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: 'Giữ chỗ thất bại',
                            text: response.message, 
                            confirmButtonText: 'Đóng',
                        });
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    // Ẩn hiệu ứng loading nếu có lỗi xảy ra
                    $('#custom-loading-overlay').hide();

                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: response.message,
                        confirmButtonText: 'Đóng',
                    });
                }
            });
        }
    })
}




function chuyenBooking() {
    // Hiển thị hiệu ứng loading
    $('#custom-loading-overlay').show();

    var TourCode = document.getElementById("TourCodeInputValue").value;
    var NguoiNhan = document.getElementById("NguoiNhan").value;

    $.ajax({
        type: "POST",
        url: "../DuLich/ChuyenBooking",
        data: {
            TourCode: TourCode,
            NguoiNhan: NguoiNhan
        },
        success: function (response) {
            // Ẩn hiệu ứng loading sau khi nhận phản hồi từ máy chủ
            $('#custom-loading-overlay').hide();

            if (response.success) {
                swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            } else {
                // Thông báo Sweet Alert lỗi
                swal.fire({
                    title: "Error!",
                    text: response.message,
                    icon: "error",
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            // Ẩn hiệu ứng loading nếu có lỗi xảy ra
            $('#custom-loading-overlay').hide();

            // Thông báo Sweet Alert lỗi
            swal.fire({
                title: "Error!",
                text: "Error: " + textStatus,
                icon: "error",
            });
        }
    });
}




function changeStatus() {
    Swal.fire({
        title: 'Xác nhận thao tác',
        text: "Bạn có chắc chắn muốn đổi trạng thái không?",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Bỏ qua',
    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel) {
           
            Swal.close();
            return;
        }
        // Hiển thị hiệu ứng loading
        $('#custom-loading-overlay').show();

        var TourCode = document.getElementById("TourCodeInputValue").value;
        var IDStatus = document.getElementById("TinhTrang" + "(" + TourCode + ")").value;

        $.ajax({
            type: "POST",
            url: "../DuLich/ChangeBookingStatus",
            data: {
                IDStatus: IDStatus,
                TourCode: TourCode
            },
            success: function (response) {
                // Ẩn hiệu ứng loading sau khi nhận phản hồi từ máy chủ
                $('#custom-loading-overlay').hide();

                if (response.success) {
                    document.getElementById("tinhtrang").innerHTML = IDStatus;
                    var element = document.getElementById("IconTrangThai(" + TourCode + ")");
                    var actionButtons = document.getElementsByClassName("actionBookingBtn" + "(" + TourCode + ")");
                    var selectElement = document.getElementById("TinhTrang" + "(" + TourCode + ")");
                    var tinhTrangElement = document.getElementById("tinhtrang");

                    if (element) {
                        switch (IDStatus) {
                            case "3":
                                element.innerHTML = `<span class="btn-primary rounded-text"><span>Đã giữ chỗ</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                tinhTrangElement.innerText = `Đã giữ chỗ`
                                break;
                            case "2":
                                element.innerHTML = `<span class="btn-info rounded-text"><span>Đã tiếp nhận</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                tinhTrangElement.innerText = `Đã tiếp nhận`
                                break;
                            case "4":
                                element.innerHTML = `<span class="btn-warning rounded-text"><span>Đã đặt cọc</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                tinhTrangElement.innerText = `Đã đặt cọc`
                                break;
                            case "5":
                                element.innerHTML = `<span class="btn-success rounded-text"><span>Hoàn thành</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                tinhTrangElement.innerText = `Hoàn thành`
                                break;
                            case "1":
                                element.innerHTML = `<span class="btn-danger rounded-text"><span>Mới</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                tinhTrangElement.innerText = `Mới`
                                break;
                            case "7":
                                element.innerHTML = `<span class="btn-success rounded-text"><span>Đã hoàn thành</span><input type="hidden" id="tinhtrang" value="${IDStatus}" /></span>`;
                                var inputElement = document.createElement("input");
                                inputElement.type = "text";
                                inputElement.value = "Đã hoàn thành";
                                inputElement.readOnly = true;
                                inputElement.className = "form-control"
                                selectElement.parentNode.replaceChild(inputElement, selectElement);
                                tinhTrangElement.innerText = `Đã hoàn thành `
                                for (var i = 0; i <= actionButtons.length; i++) {
                                    actionButtons[i].disabled = true;
                                }

                                break;
                            default:
                                element.innerHTML = '<span class="btn-danger rounded-text"><span>Chưa có giá trị IDStatus</span></span>'; //  Đoạn này dùng để nhận biết lỗi trường hợp chưa có IDStatus
                                break;
                        }
                    }
                    swal.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: response.message,
                        confirmButtonText: 'Đóng',
                    });
                } else {
                    swal.fire({
                        title: "Error!",
                        text: response.message,
                        icon: "error",
                    });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                // Ẩn hiệu ứng loading nếu có lỗi xảy ra
                $('#custom-loading-overlay').hide();

                swal.fire({
                    title: "Error!",
                    text: "Error: " + textStatus,
                    icon: "error",
                });
            }
        });
    })
}





function showCancelBookingModal() {

    Swal.fire({
        title: 'Xác nhận thao tác',
        text: "Bạn có chắc chắn muốn huỷ booking không?",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Bỏ qua',
    }).then((result) => {
        if (result.isConfirmed) {
            $('#cancel_booking_detail').modal('show');
        }
    });
}


document.getElementById("closeModal1Button").onclick = function () {
  
    $('#cancel_booking_detail').modal('hide');
};







function saveCancellationReason() {
   
    $('#custom-loading-overlay').show();

    var cancellationReason = $("#cancellation_reason").val();
    var TourCode = $("#TourCodeInputValue").val();

   
    if (!cancellationReason) {
        $('#custom-loading-overlay').hide();
        swal.fire({
            imageUrl: "/images/fail.png",
            imageWidth: 100,
            imageHeight: 100,
            title: 'Vui lòng nhập lí do hủy',
            confirmButtonText: 'Đóng',
        });
        return; 
    }

    $.ajax({
        type: "POST",
        url: "../DuLich/CancelBooking",
        data: {
            cancellationReason: cancellationReason,
            TourCode: TourCode
        },
        success: function (response) {
            $('#custom-loading-overlay').hide();

            if (response.success) {

                var element = document.getElementById("IconTrangThai(" + TourCode + ")");
                var actionButtons = document.getElementsByClassName("actionBookingBtn" + "(" + TourCode + ")");
                var selectElement = document.getElementById("TinhTrang" + "(" + TourCode + ")");
                var tinhTrangElement = document.getElementById("tinhtrang");


                if (element) {
                    switch (response.idStatus) {
                        case "6":
                            element.innerHTML = `<span class="btn-dark rounded-text"><span>Đã huỷ</span><input type="hidden" id="tinhtrang" value="${response.idStatus}" /></span>`;
                            tinhTrangElement.innerText = `Huỷ booking`;
                            break;
                        default:
                            element.innerHTML = '<span class="btn-danger rounded-text"><span>Chưa có giá trị IDStatus</span></span>'; //  Đoạn này dùng để nhận biết lỗi trường hợp chưa có IDStatus
                            break;
                    }
                }

                swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });

                $('#cancel_booking_detail').modal('hide');

                var inputElement = document.createElement("input");
                inputElement.type = "text";
                inputElement.value = "Huỷ booking";
                inputElement.readOnly = true;
                inputElement.className = "form-control"
                selectElement.parentNode.replaceChild(inputElement, selectElement);

                for (var i = 0; i <= actionButtons.length; i++) {
                    actionButtons[i].disabled = true;
                }



            } else {
                swal.fire({
                    title: "Error!",
                    text: response.message,
                    icon: "error",
                });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            $('#custom-loading-overlay').hide();
            swal.fire({
                title: "Error!",
                text: "Error: " + textStatus,
                icon: "error",
            });
        }
    });
}








