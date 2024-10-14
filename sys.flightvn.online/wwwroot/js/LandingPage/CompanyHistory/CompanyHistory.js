$(document).ready(function () {
    $('body').on('click', '#BtnCreate', function () {
        debugger
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../LandingPage/CreateCompanyHistory",
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
            url: "../LandingPage/EditCompanyHistory",
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
});