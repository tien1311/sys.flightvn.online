function CustomError(message) {
    Swal.fire({
        imageUrl: "/images/fail.png",
        imageWidth: 100,
        imageHeight: 100,
        title: message,
        confirmButtonText: 'Đóng',
    });
}

function CustomSuccess(message) {
    Swal.fire({
        imageUrl: "/images/success.png",
        imageWidth: 100,
        imageHeight: 100,
        title: message,
        confirmButtonText: 'Đóng',
    }).then((result) => {
        location.reload();
    });
}
$(document).on('click', '#btnCreate', function () {
    $.ajax({
        type: "POST",
        url: "../Daily/CreatePost",
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

$('body').on('click', '.deleteBtn', function (e) {
    e.preventDefault();
    var id = $(this).data('id');

    Swal.fire({
        title: 'Bạn có muốn xoá?',
        text: "Hành động này không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xoá',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: '../Daily/DeletePost',
                data: { subjectId: id },
                success: function (response) {
                    if (response.success) {
                        CustomSuccess(response.message);
                    } else {
                        CustomError(response.message);
                    }
                }
            });
        }
    });
});

$('body').on('click', '.isShowBtn', function () {
    var id = $(this).data('id');
    $.ajax({
        type: 'POST',
        url: '../Daily/IsActive',
        data: { subjectId: id },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                })
            } else {
                CustomError(response.message);
            }
        }
    });
});

$('body').on('click', '.editBtn', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: "../Daily/DetailPost",
        data: {
            subjectId: id
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

$('body').on('click', '.mailBtn', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    Swal.fire({
        title: 'Bạn có muốn gửi mail?',
        text: "Hành động này không thể hoàn tác!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Gửi mail',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: '../Daily/SendMail',
                data: { subjectId: id },
                success: function (response) {
                    if (response.success) {
                        CustomSuccess(response.message);
                    } else {
                        CustomError(response.message);
                    }
                }
            });
        }
    });
})

// Filter List Order
document.getElementById('Filter_TinhTrang').onchange = (e) => {
    categoryId = e.target.value;
    pageSize = document.getElementById('Page_Size').value;
    loadOrders(categoryId, pageSize)
};

// Chọn số lượng sản phẩm hiển thị
$(document).on('change', '#Page_Size', function (e) {
    categoryId = document.getElementById('Filter_TinhTrang').value;
    pageSize = e.target.value;
    loadOrders(categoryId, pageSize)
});

//#region Phân trang
function loadPage(page) {
    categoryId = document.getElementById('Filter_TinhTrang').value;
    loadOrders(categoryId, pageSize, page)
}

function loadOrders(categoryId, pageSize, page = 1) {
    $.post(
        '../Daily/GetAllOrderByFilter',
        {
            categoryId: categoryId,
            pageSize: pageSize,
            page: page
        },
        (data) => {
            $("#table_Pagination").html(data);
            $("html, body").animate({ scrollTop: 0 }, "slow"); // Kéo page lên đầu
        }
    );
}