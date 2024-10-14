CKEDITOR.replace('LongDescription', {
    height: 200,
});

$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(this);

    var longDescription = CKEDITOR.instances['LongDescription'].getData();
    formData.append('LongDescription', longDescription);

    $.ajax({
        type: 'POST',
        url: '../LandingPage/SaveEditNews',
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
