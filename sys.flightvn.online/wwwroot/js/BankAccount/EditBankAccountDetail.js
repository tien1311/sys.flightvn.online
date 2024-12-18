﻿$('body').on('submit', '#submitForm', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var submitButton = $(this).find('button[type="submit"]');
    submitButton.prop('disabled', true);
    var formData = $(this).serialize();

    $.ajax({
        type: 'POST',
        url: '../KeToan/SaveEditBankAccountDetail',
        data: formData,
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            submitButton.prop('disabled', false);
            if (response.success) {
                CustomSweetAlert_Success_ReloadPage(response.message);
            }
            else {
                CustomSweetAlert_Error(response.message);
            }
        }
    })
})
