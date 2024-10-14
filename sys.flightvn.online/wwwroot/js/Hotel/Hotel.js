let pageSize = 10;
//#region Phân trang
function loadPage(page) {
    let hotelCodes = document.getElementById('hotelCodes').value;
    let hotelNames = document.getElementById('hotelNames').value;
    let province = document.getElementById('province').value;
    let isActived = document.getElementById('isActived').value;
    loadOrders(hotelCodes, hotelNames, province, isActived, pageSize, page)
}


function loadOrders(hotelCodes, hotelNames, province, isActived, pageSize, page = 1) {
    $.post(
        '../Hotel/GetAllHotelByFilter',
        {
            hotelCodes: hotelCodes,
            hotelNames: hotelNames,
            province: province,
            isActived: isActived,
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
    let hotelCodes = document.getElementById('hotelCodes').value;
    let hotelNames = document.getElementById('hotelNames').value;
    let province = document.getElementById('province').value;
    let isActived = document.getElementById('isActived').value;
    pageSize = e.target.value;
    loadOrders(hotelCodes, hotelNames, province, isActived, pageSize)
});

$(document).on('submit', '#searchForm', function (e) {
    e.preventDefault();
    let hotelCodes = document.getElementById('hotelCodes').value;
    let hotelNames = document.getElementById('hotelNames').value;
    let province = document.getElementById('province').value;
    let isActived = document.getElementById('isActived').value;
    loadOrders(hotelCodes, hotelNames, province, isActived, pageSize)

})


$(document).ready(function () {
    $('body').on('click', '.isActived', function () {
        var code = $(this).data("id");

        $.ajax({
            url: '../Hotel/IsActived',
            type: 'POST',
            data: { code: code },
            success: function (rs) {
                if (rs.success) {
                    Swal.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thành công",
                        text: rs.message,
                        button: "Đóng",
                    })
                } else {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thất bại",
                        text: rs.message,
                        button: "Đóng",
                    });
                }
            },
            error: function () {
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: "Thao tác thất bại",
                    text: rs.message,
                    button: "Đóng",
                });
            }
        });
    });


    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data("id");
        Swal.fire({
            title: 'Bạn thực sự muốn xoá?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý',
            cancelButtonText: 'Huỷ bỏ',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '../Hotel/delete',
                    type: 'POST',
                    data: { id: id },
                    success: function (rs) {
                        if (rs.success) {
                            Swal.fire({
                                imageUrl: "/images/success.png",
                                imageWidth: 100,
                                imageHeight: 100,
                                title: "Thao tác thành công",
                                text: rs.message,
                                button: "Đóng",
                            }).then(() => {
                                location.reload();
                            });
                        } else {
                            Swal.fire({
                                imageUrl: "/images/fail.png",
                                imageWidth: 100,
                                imageHeight: 100,
                                title: "Thao tác thất bại",
                                text: rs.message,
                                button: "Đóng",
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            imageUrl: "/images/fail.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: "Thao tác thất bại",
                            text: rs.message,
                            button: "Đóng",
                        });
                    }
                });
            }
        });
    });

    $("#BtnCreate").click(function () {
        $.ajax({
            type: "POST",
            url: "../Hotel/CreateHotel",
            success: function (response) {
                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });
                $('#provinceSelect').change(function () {
                    debugger
                    var provinceCode = $(this).val();
                    $.get('../Hotel/GetDistricts', { provinceCode: provinceCode }, function (data) {
                        $('#districtSelect').empty();
                        $('#districtSelect').append('<option value="">Chọn Quận/Huyện</option>');
                        $.each(data, function (index, value) {
                            $('#districtSelect').append('<option value="' + value.code + '">' + value.full_Name + '</option>');
                        });
                    });
                });

                $('#districtSelect').change(function () {
                    var districtCode = $(this).val();
                    $.get('../Hotel/GetWards', { districtCode: districtCode }, function (data) {
                        $('#wardSelect').empty();
                        $('#wardSelect').append('<option value="">Chọn Phường/Xã</option>');
                        $.each(data, function (index, value) {
                            $('#wardSelect').append('<option value="' + value.code + '">' + value.full_Name + '</option>');
                        });
                    });
                });

                CKEDITOR.replace('LongDescription', {
                    height: 200,
                    filebrowserUploadUrl: '/Data/UploadCKEditorDuLich'
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

    $('body').on('click', '.Edit', function () {
        var id = $(this).data("id");

        $.ajax({
            type: "POST",
            url: "../Hotel/EditHotel",
            data: {
                id: id
            },
            success: function (response) {
                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });

                // Cập nhật các select boxes với dữ liệu đã nhúng
                var initialProvince = initialData.Province;
                var initialDistrict = initialData.District;
                var initialWard = initialData.Ward;

                // Cập nhật Tỉnh/Thành
                $('#provinceSelect').val(initialProvince).trigger('change');

                // Cập nhật Quận/Huyện và Phường/Xã dựa trên Tỉnh/Thành đã chọn
                if (initialProvince) {
                    $.get('../Hotel/GetDistricts', { provinceCode: initialProvince }, function (data) {
                        $('#districtSelect').empty();
                        $('#districtSelect').append('<option value="">Chọn Quận/Huyện</option>');
                        $.each(data, function (index, value) {
                            $('#districtSelect').append('<option value="' + value.code + '">' + value.full_Name + '</option>');
                        });

                        // Set giá trị Quận/Huyện đã chọn
                        $('#districtSelect').val(initialDistrict).trigger('change');
                    });
                }

                // Cập nhật Quận/Huyện khi Tỉnh/Thành thay đổi
                $('#provinceSelect').change(function () {
                    var provinceCode = $(this).val();
                    $.get('../Hotel/GetDistricts', { provinceCode: provinceCode }, function (data) {
                        $('#districtSelect').empty();
                        $('#districtSelect').append('<option value="">Chọn Quận/Huyện</option>');
                        $.each(data, function (index, value) {
                            $('#districtSelect').append('<option value="' + value.code + '">' + value.full_Name + '</option>');
                        });

                        // Xóa và cập nhật Phường/Xã
                        $('#wardSelect').empty();
                        $('#wardSelect').append('<option value="">Chọn Phường/Xã</option>');
                    });
                });

                // Cập nhật Phường/Xã khi Quận/Huyện thay đổi
                $('#districtSelect').change(function () {
                    var districtCode = $(this).val();
                    $.get('../Hotel/GetWards', { districtCode: districtCode }, function (data) {
                        debugger;
                        $('#wardSelect').empty();
                        $('#wardSelect').append('<option value="">Chọn Phường/Xã</option>');

                        var isCodeExists = false;

                        $.each(data, function (index, value) {
                            if (value.code == initialWard) {
                                isCodeExists = true;
                            }
                            $('#wardSelect').append('<option value="' + value.code + '">' + value.full_Name + '</option>');
                        });

                        // Set giá trị Phường/Xã đã chọn
                        if (isCodeExists == true) {
                            $('#wardSelect').val(initialWard);
                        }
                        else {
                            $('#wardSelect').val("");
                        }
                    });
                });

                CKEDITOR.replace('LongDescription', {
                    height: 200,
                    filebrowserUploadUrl: '/Data/UploadCKEditorDuLich'
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


    $('body').on('click', '.EditRoomTypeHotel', function () {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../Hotel/EditRoomTypeHotel",
            data: {
                ID: id
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

    $('body').on('click', '.EditProductHotelService', function () {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../Hotel/EditProductHotelService",
            data: {
                ID: id
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

    $('body').on('click', '#btnDeleteSelected', function (e) {
        e.preventDefault();
        debugger;
        var str = "";
        var checkbox = $('#gridHotel').find('input:checkbox.cbkItem:checked'); // Lấy tất cả các checkbox đã được chọn trong bảng với id là 'gridHotel'
        var i = 0;
        var deletedIds = []; 
        checkbox.each(function () {
            debugger;
            var _id = $(this).val(); // Lấy giá trị của checkbox (ID của hàng)
            deletedIds.push(_id); // Thêm ID vào mảng
            if (i === 0) {
                str += _id;
            } else {
                str += "," + _id;
            }
            i++;
        });
        if (str.length > 0) {
            // Sử dụng SweetAlert 2 để hiển thị cửa sổ xác nhận thay vì confirm
            Swal.fire({
                title: 'Xác nhận xoá',
                text: 'Bạn có chắc là muốn xoá các nội dung này?',
                icon: 'warning', // Loại biểu tượng (có thể là 'success', 'error', 'warning', 'info', v.v.)
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Xoá',
                cancelButtonText: 'Hủy'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '../hotel/DeleteSelected',
                        type: 'POST',
                        data: { ids: str },
                        success: function (rs) {
                            if (rs.success) {
                                location.reload();
                            }
                        }
                    });
                }
            });
        }
        else {
            // Sử dụng SweetAlert 2 thay thế cho alert
            Swal.fire({
                icon: 'warning', // Loại biểu tượng (có thể là 'success', 'error', 'warning', 'info', v.v.)
                title: 'Lỗi',
                text: 'Vui lòng chọn ít nhất 1 dòng',
                confirmButtonText: 'OK'
            });
        }
    });


    $('body').on('click', '#btnUndo', function (e) {
        e.preventDefault();
        $.ajax({
            url: '../Hotel/UndoDelete', // Địa chỉ URL của action Undo trong Controller
            type: 'POST',
            success: function (rs) {
                if (rs.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: 'Thao tác đã được hoàn tác, nhấn OK để tiếp tục',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then(function () {
                        // Sau khi bấm OK, làm mới trang
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        title: 'Lỗi!',
                        text: 'Không thể hoàn tác thao tác.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            }
        });
    });

})