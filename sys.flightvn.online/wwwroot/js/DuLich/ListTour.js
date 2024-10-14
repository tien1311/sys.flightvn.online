function formatNumber(number) {
    number = number.toFixed(0) + '';
    var x = number.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


var priceInputs = document.getElementsByClassName('priceInput');
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


function CreateRowTable() {
    const tableBody = document.querySelector('.table-price tbody');
   
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
             <td>
                <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
            </td>
            <td>
                <select class="form-control" name="gia_loai">
                  
                    <option value="1s">KS 1s</option>
                    <option value="2s">KS 2s</option>
                    <option value="3s">KS 3s</option>
                    <option value="4s">KS 4s</option>
                    <option value="5s">KS 5s</option>
                    <option value="rs3s">KS 3s</option>
                    <option value="rs4s">RS 4s</option>
                    <option value="rs5s">RS 5s</option>
                </select>
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="gia_nguoi_lon[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="gia_tre_em[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="gia_em_be[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="phu_thu_don[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="phu_thu_quoctich[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="hh_gia_nguoi_lon[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="hh_gia_tre_em[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="hh_gia_em_be[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="km_gia_nguoi_lon[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="km_gia_tre_em[]" />
            </td>
            <td>
                <input type="text" class="form-control priceInput" name="km_gia_em_be[]" />
            </td>
         
        `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);

    // Gắn sự kiện format cho các ô input số mới
    const priceInputs = newRow.querySelectorAll('.priceInput');
    priceInputs.forEach(input => {
        input.addEventListener('input', function () {
            var inputValue = this.value.replace(/[^\d]/g, '');
            if (inputValue === '') {
                // If empty, set input value to an empty string
                this.value = '';
                return; // Exit the function
            }
            var number = parseFloat(inputValue);
            var formattedNumber = formatNumber(number);
            this.value = formattedNumber;
        });
    });
}

function DeleteRowTable(button) {
    const row = button.closest('tr');
    row.remove();
}

$(document).ready(function () {
    $('#createTourForm').submit(function (e) {
        e.preventDefault();

        // Hiển thị overlay loading
        $('#custom-loading-overlay').show();

        var formData = new FormData(this);
        var mainImageId = $('input[name="mainImage"]:checked').val();

        for (var i = 1; i <= countImg; i++) {
            var images = {};
            var detailImage = "file" + i;
            images.ImageURL = document.getElementById(detailImage).files[0];
            images.MainImage = (i == mainImageId);

            formData.append('mainImages', images.MainImage);
            formData.append('imageFiles', images.ImageURL);
        }

        $.ajax({
            url: '../DuLich/CreateTour',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                // Ẩn overlay loading
                $('#custom-loading-overlay').hide();

                if (response.success) {
                    Swal.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        text: response.message,
                        confirmButtonText: 'Đóng',
                    });
                    location.reload();
                } else {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        text: response.message,
                        confirmButtonText: 'Đóng',
                    });
                }
            },
            error: function (error) {
                // Ẩn overlay loading
                $('#custom-loading-overlay').hide();
                console.error('Lỗi:', error);
            }
        });
    });
});

function chuyenBooking() {
    // Hiển thị hiệu ứng loading
    $('#custom-loading-overlay').show();

    var TourCode = document.getElementById("TourCodeInputValue").value;
    var NguoiNhan = document.getElementById("NguoiNhan").value;

    $.ajax({
        type: "POST",
        url: "../DuLich/ChuyenBookingTourHot",
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
          
            $.ajax({
                type: "POST",
                url: "../DuLich/GuimailbookingTourHot",
                data: {
                    TourCode: TourCode,                 
                },
                success: function (response) {
                    $('#custom-loading-overlay').hide();

                    if (response.success) {
                        debugger;
                        var element = document.getElementById("IconTrangThai(" + TourCode + ")");
                        var tinhTrangElement = document.getElementById("tinhtrang");
                        if (element) {
                            switch (response.idStatus) {
                                case "3":
                                    element.innerHTML = `<span class="btn-primary rounded-text"><span>Đã giữ chỗ</span><input type="hidden" id="tinhtrang" value="${response.idStatus}" /></span>`;
                                    if (tinhTrangElement) {
                                        tinhTrangElement.innerText = `Đã giữ chỗ`;
                                    }
                                    break;
                                default:
                                    element.innerHTML = '<span class="btn-danger rounded-text"><span>Chưa có giá trị IDStatus</span></span>';
                                    break;
                            }
                        }
                        // Đổi select list
                        var SelectElement = document.getElementById("TinhTrang" + "(" + TourCode + ")");
                        if (SelectElement) {
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
                        }

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
            url: "../DuLich/ChangeBookingStatusTourHot",
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
        url: "../DuLich/CancelBookingTourHot",
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

