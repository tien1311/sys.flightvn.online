$(document).ready(function () {
    $('body').on('submit', '#submitForm', function (e) {
        e.preventDefault();
        $('#loadingOverlay').css('display', 'flex');
        var formData = new FormData(this);
        $.ajax({
            type: "POST",
            url: "../LandingPage/UpdateLogo",
            data: formData,
            processData: false, 
            contentType: false,
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
                if (response.success) {
                    CustomSweetAlert_Success_ReloadPage(response.message);
                }
                else {
                    CustomSweetAlert_Error(response.message);
                }
            }
        })
    })
})