$(document).ready(function () {
    $('body').on('click', '#BtnCreate', function () {
        debugger
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../LandingPage/CreatePost",
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
            url: "../LandingPage/EditPost",
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

    $('body').on('click', '.isActived', function () {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../LandingPage/IsActivedPost",
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


});