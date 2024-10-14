$("#saveBtn").click(function () {

    $.ajax({
        type: "GET",
        url: "../KyThuat/CreateFlight",
        success: function (response) {
            $('#openPopup1').html(response);

            $('#openPopup1').modal({
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

$("#gridTable .DeleteFlight").click(function () {

    /*var index = $('#gridVeHoan tr').index($(this).closest('tr'));*/
    var id = String($(this).closest('tr').attr('id'));
    let text = "Bạn có chắc muốn xóa hành trình này.";
    if (confirm(text) == true) {
        $.ajax({
            type: "POST",
            url: "../KyThuat/DeleteFlightData",
            data: { khoachinh: id },
            success: function (response) {
                if (response == "false") {
                    alert("Xóa thất bại");
                }
                else {
                    alert("Xóa thành công");
                    location.reload();
                }

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }

});

$("#gridTable .Flight").click(function () {

    /*var index = $('#gridVeHoan tr').index($(this).closest('tr'));*/
    var id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../KyThuat/UpdateFlight",
        data: { khoachinh: id },
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