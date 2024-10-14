CKEDITOR.replace('LongDescription', {
    height: 200,
    filebrowserUploadUrl: '/Data/UploadCKEditorDuLich'
});

/// Mở modal OtherFee
var customModalOtherFee = document.getElementById('customModal');
var openModalBtnOtherFee = document.getElementById('openGalleryModal');
var closeModalBtnOtherFee = document.getElementById('closeModalBtn');
openModalBtnOtherFee.onclick = function () {
    customModalOtherFee.style.display = 'block';
    $.ajax({
        url: '../Daily/GetImagesFromFTP',
        type: 'GET',
        success: function (result) {
            $('#imageListContainer').html(result);
        }
    });
}
closeModalBtnOtherFee.onclick = function () {
    customModalOtherFee.style.display = 'none';
}
////window.onclick = function (event) {
////    if (event.target === modal) {
////        customModalOtherFee.style.display = 'none';
////    }
////}

$('body').on('submit', '#submitForm', function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    // Get data from CKEditor
    var longDescription = CKEDITOR.instances['LongDescription'].getData();
    formData.append('subject_content', longDescription);
    $.ajax({
        type: 'POST',
        url: '../Daily/SaveCreateOrUpdatePost',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                CustomSuccess(response.message);
            } else {
                CustomError(response.message);
            }
        }
    });
});
$('#inputImageFTP').on('change', function () {
    var fileName = $(this).val().split('\\').pop();
    $('#inputImageFTPName').val(fileName);
});

$('body').on('submit', '#uploadImageForm', function (e) {
    e.preventDefault();
    var formData = new FormData(this); // Create FormData object
    $.ajax({
        url: "../Daily/UploadFileToFTP",
        data: formData,
        type: 'POST',
        contentType: false,
        processData: false,
        success: function (response) {
            var imageUrl = response.imageUrl; 
            $('#inputImage').val(imageUrl);
            $('#image-container').empty();
            $('#image-container').append('<img src="' + imageUrl + '" alt="Uploaded Image" style="width:100px; height:auto;" />');
            closeModalBtnOtherFee.click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Upload failed:', textStatus, errorThrown);
        }
    })

});




$(document).on('click', '.page-item', function () {
    var page = $(this).data('page');
    $.get('../Daily/GetImagesFromFTP', { page: page }, function (data) {
        $('#imageListContainer').html(data);
    });
});

$(document).on('click', '.select-image-btn', function () {
    var imageUrl = $(this).data('image-url');
    $('#inputImage').val(imageUrl);
    $('#image-container').empty();
    $('#image-container').html('<img src="' + imageUrl + '" style="width:100px; height:auto;" />');
    closeModalBtnOtherFee.click();
});