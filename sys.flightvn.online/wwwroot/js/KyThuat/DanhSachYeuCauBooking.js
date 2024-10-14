$("#gridTable .Chitiet").click(function () {

    /*var index = $('#gridVeHoan tr').index($(this).closest('tr'));*/
    var id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../KyThuat/Chitietyeucaubooking",
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