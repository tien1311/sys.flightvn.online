let pageSize = 10;


//#region Phân trang
function loadPage(page) {
    debugger
    let Email = document.getElementById('Email').value;
    let IsResolved = document.getElementById('IsResolvedValue').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(Email, IsResolved, fromDate, toDate, pageSize, page)
}


function loadOrders(Email, IsResolvedValue, fromDate, toDate, pageSize, page = 1) {
    $.post(
        '../LandingPage/GetAllCustomerRequestByFilter',
        {
            Email: Email,
            IsResolvedValue: IsResolvedValue,
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
    let Email = document.getElementById('Email').value;
    let IsResolved = document.getElementById('IsResolvedValue').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    pageSize = e.target.value;
    loadOrders(Email, IsResolved, fromDate, toDate, pageSize)
});

$(document).on('submit', '#searchForm', function (e) {
    e.preventDefault();
    let Email = document.getElementById('Email').value;
    let IsResolved = document.getElementById('IsResolvedValue').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(Email, IsResolved, fromDate, toDate, pageSize)
})


$(document).ready(function () {
    $(document).on('click', '.Detail', function () {
        $('#loadingOverlay').css('display', 'flex');
        let id = $(this).data("id");
        $.ajax({
            type: 'POST',
            url: '../LandingPage/DetailCustomerRequest',
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
})

