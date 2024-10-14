let pageSize = 10;


//#region Phân trang
function loadPage(page) {
    let CreatedBy = document.getElementById('CreatedBy').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(CreatedBy, fromDate, toDate, pageSize, page)
}


function loadOrders(CreatedBy, fromDate, toDate, pageSize, page = 1) {
    $.post(
        '../LandingPage/GetAllNewsByFilter',
        {
            CreatedBy: CreatedBy,
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
    let CreatedBy = document.getElementById('CreatedBy').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    pageSize = e.target.value;
    loadOrders(CreatedBy, fromDate, toDate, pageSize)
});

$(document).on('submit', '#searchForm', function (e) {
    e.preventDefault();
    let CreatedBy = document.getElementById('CreatedBy').value;
    let fromDate = document.getElementById('fromDate').value;
    let toDate = document.getElementById('toDate').value;
    loadOrders(CreatedBy, fromDate, toDate, pageSize)
})


$(document).ready(function () {

    $(document).on('click', '#BtnCreate', function () {
        debugger
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../LandingPage/CreateNews",
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

    $(document).on('click', '.Detail', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../LandingPage/EditNews",
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

    $(document).on('click', '.isActived', function () {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../LandingPage/IsActivedNews",
            data: {
                id: id
            },
            success: function (response) {
                if (response.success) {
                    CustomSweetAlert_Success(response.message);
                }
                else {
                    CustomSweetAlert_Error(response.message);
                }
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

