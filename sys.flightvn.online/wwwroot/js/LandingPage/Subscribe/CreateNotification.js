CKEDITOR.replace('Body', {
    height: 200,
});

$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(this);

    var longDescription = CKEDITOR.instances['Body'].getData();
    formData.append('Body', longDescription);

    $.ajax({
        type: 'POST',
        url: '../LandingPage/SendEmailToSubcribers',
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
