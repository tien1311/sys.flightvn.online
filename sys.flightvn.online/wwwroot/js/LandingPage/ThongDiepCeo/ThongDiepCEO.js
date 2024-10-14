CKEDITOR.replace('Description', {
    height: 200,
});

$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(this);
    var Description = CKEDITOR.instances['Description'].getData();
    formData.append('Description', Description);

    $.ajax({
        type: 'POST',
        url: '../LandingPage/UpdateThongDiepCEO',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            if (response.success) {
                CustomSweetAlert_Success_ReloadPage(response.message);
            } else {
                CustomSweetAlert_Error(response.message);
            }
        }
    });
});