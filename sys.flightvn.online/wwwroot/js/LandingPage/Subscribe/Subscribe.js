let pageSize = 10;


//#region Phân trang
function loadPage(page) {
    let Email = document.getElementById('Email').value;
    loadOrders(Email, pageSize, page)
}


function loadOrders(Email, pageSize, page = 1) {
    $.post(
        '../LandingPage/GetAllSubscribeByFilter',
        {
            Email: Email,
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
    let Email = document.getElementById('Email').value;
    pageSize = e.target.value;
    loadOrders(Email, pageSize)
});

$(document).on('submit', '#searchForm', function (e) {
    e.preventDefault();
    let Email = document.getElementById('Email').value;
    loadOrders(Email, pageSize)
})


$(document).ready(function () {

    $(document).on('click', '#BtnCreate', function () {
        debugger
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../LandingPage/CreateNotification",
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
})
