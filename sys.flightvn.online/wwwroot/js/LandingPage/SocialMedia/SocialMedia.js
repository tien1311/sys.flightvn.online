﻿$(document).ready(function () {
    $('body').on('click', '#BtnCreate', function () {
        debugger
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../LandingPage/CreateSocialMedia",
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
            url: "../LandingPage/EditSocialMedia",
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

    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data("id");
        CustomSweetAlert_Confirm('Bạn thực sự muốn xoá?', 'Sau khi xoá thì sẽ không thể hoàn tác').then((result) => {
            if (result.isConfirmed) {
                $('#loadingOverlay').css('display', 'flex');
                $.ajax({
                    url: '../LandingPage/DeleteSocialMedia',
                    type: 'POST',
                    data: { id: id },
                    success: function (rs) {
                        $('#loadingOverlay').css('display', 'none');
                        if (rs.success) {
                            CustomSweetAlert_Success_ReloadPage(rs.message)
                        } else {
                            CustomSweetAlert_Error(rs.message)
                        }
                    },
                    error: function () {
                        CustomSweetAlert_Error(rs.message)
                    }
                });
            }
        });
    });

});