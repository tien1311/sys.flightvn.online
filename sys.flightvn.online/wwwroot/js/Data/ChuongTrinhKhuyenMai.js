$("#BtnCreate").click(function () {

    $.ajax({
        type: "POST",
        url: "../Data/CreateCTKM",
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

$("#gridCTKM .Edit").click(function () {
    var id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../Data/EditCTKM",
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
function UpdateStatusCTKM(status, id) {
    $.ajax({
        type: "POST",
        url: "../Data/StatusCTKM",
        data: {
            ID: id,
            Status: status
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã lưu danh mục thành công");
            }
            else {
                alert("Bạn đã lưu danh mục không thành công");
            }
            window.location.href = "/Data/ChuongTrinhKhuyenMai?i=9";
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};