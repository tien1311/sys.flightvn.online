let pageSize = 10;
//#region Phân trang
function loadPage(page) {
    let bankCodes = document.getElementById('bankCodes').value;
    let bankNames = document.getElementById('bankNames').value;
    let firstSerials = document.getElementById('firstSerials').value;
    loadOrders(bankCodes, bankNames, firstSerials, pageSize, page)
}


function loadOrders(bankCodes, bankNames, firstSerials, pageSize, page = 1) {
    $.post(
        '../KeToan/GetAllBankAccountByFilter',
        {
            bankCodes: bankCodes,
            bankNames: bankNames,
            firstSerials: firstSerials,
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
    let bankCodes = document.getElementById('bankCodes').value;
    let bankNames = document.getElementById('bankNames').value;
    let firstSerials = document.getElementById('firstSerials').value;
    pageSize = e.target.value;
    loadOrders(bankCodes, bankNames, firstSerials, pageSize)
});

$(document).on('input', '#bankCodes, #bankNames, #firstSerials', function (e) {
    e.preventDefault();
    let bankCodes = document.getElementById('bankCodes').value;
    let bankNames = document.getElementById('bankNames').value;
    let firstSerials = document.getElementById('firstSerials').value;
    loadOrders(bankCodes, bankNames, firstSerials, pageSize);
});

$(document).ready(function () {
    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data("id");

        CustomSweetAlert_Confirm('Bạn thực sự muốn xoá?', 'Sau khi xoá thì chỉ có thể hoàn tác trong vòng 5s').then((result) => {
            if (result.isConfirmed) {
                $('#loadingOverlay').css('display', 'flex');
                $.ajax({
                    url: '../KeToan/DeleteBankAccount',
                    type: 'POST',
                    data: { id: id },
                    success: function (rs) {
                        $('#loadingOverlay').css('display', 'none');
                        if (rs.success) {
                            CustomSweetAlert_Success_ReloadPage(rs.message)
                        } else {
                            CustomSweetAlert_Error(rs.message)
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
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../KeToan/CreateBankAccount",
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
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

    $('body').on('click', '.Edit', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../KeToan/EditBankAccount",
            data: {
                id: id
            },
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
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

    $('body').on('click', '#btnDeleteSelected', function (e) {
        e.preventDefault();
        var str = "";
        var checkbox = $('#gridItem').find('input:checkbox.cbkItem:checked'); // Lấy tất cả các checkbox đã được chọn trong bảng với id là 'gridItem'
        var i = 0;
        var deletedIds = [];
        checkbox.each(function () {
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

            CustomSweetAlert_Confirm('Bạn có chắc là muốn xoá các nội dung này?', 'Sau khi xoá thì chỉ có thể hoàn tác trong vòng 5s').then((result) => {
                if (result.isConfirmed) {
                    $('#loadingOverlay').css('display', 'flex');
                    $.ajax({
                        url: '../KeToan/DeleteBankAccountSelected',
                        type: 'POST',
                        data: { ids: str },
                        success: function (rs) {
                            $('#loadingOverlay').css('display', 'none');
                            if (rs.success) {
                                CustomSweetAlert_Success_ReloadPage(rs.message)
                            }
                        }
                    });
                }
            })
        }
        else {
            CustomSweetAlert_Info("Vui lòng chọn ít nhất 1 dòng")
        }
    });


    $('body').on('click', '#btnUndo', function (e) {
        e.preventDefault();
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            url: '../KeToan/UndoDeleteBankAccount', // Địa chỉ URL của action Undo trong Controller
            type: 'POST',
            success: function (rs) {
                $('#loadingOverlay').css('display', 'none');
                if (rs.success) {
                    CustomSweetAlert_Success_ReloadPage(rs.message);
                } else {
                    CustomSweetAlert_Error(rs.message);
                }
            }
        });
    });

})