$('.chosen-select-multi').chosen();
$('#submitEditForm').on('submit', function (e) {
    e.preventDefault();
    var formData = $(this).serialize()
    $.ajax({
        type: 'POST',
        url: '../Hotel/SaveEditProductHotelService',
        data: formData,
        success: function (response) {
            if (response.success) {
                $('.btnModalClose').click();
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                })
            }
            else {
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                })
            }
        }
    })
})