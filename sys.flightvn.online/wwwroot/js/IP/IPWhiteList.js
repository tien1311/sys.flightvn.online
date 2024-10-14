let pageSize = 10;
let typingTimer; // Biến để lưu timer
const doneTypingInterval = 3000; // Thời gian chờ 3 giây

//#region Phân trang
function loadPage(page) {
    let companyName = document.getElementById('companyName').value;
    loadOrders(companyName, pageSize, page)
}

function loadOrders(companyName, pageSize, page = 1) {
    $.post(
        '../KyThuat/GetAllIPWhiteList',
        {
            companyName: companyName,
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
    let companyName = document.getElementById('companyName').value;
    pageSize = e.target.value;
    loadOrders(companyName, pageSize)
});

$(document).on('input', '#companyName', function (e) {
    e.preventDefault();
    clearTimeout(typingTimer); // Xóa timer trước đó
    typingTimer = setTimeout(function () {
        let companyName = document.getElementById('companyName').value;
        // Gọi hàm loadOrders sau 3 giây nếu không có input mới
        loadOrders(companyName, pageSize);
    }, doneTypingInterval);
});

$(document).ready(function () {
    $('body').on('click', '#BtnCreate', function () {
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../KyThuat/CreateIPWhiteList",
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
        })
    })
    $('body').on('click', '.Detail', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../KyThuat/DetailEndPointIPWhiteList",
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

    $('body').on('click', '.EditAPIPartner', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../KyThuat/EditAPIPartner",
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
    })


    $('body').on('click', '.EditIPAddress', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../KyThuat/EditIPAddress",
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
    })


});
