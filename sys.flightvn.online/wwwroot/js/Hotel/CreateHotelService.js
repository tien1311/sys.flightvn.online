$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    var formData = new FormData(this); 
    $.ajax({
        type: 'POST',
        url: '../Hotel/SaveCreateHotelService',
        data: formData,
        contentType: false,
        processData: false, 
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: "Thao tác thành công",
                    text: response.message,
                    button: "Đóng",
                }).then((result) => {
                    location.reload();
                })
            } else {
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: "Thao tác thất bại",
                    text: response.message,
                    button: "Đóng",
                });
            }
        }
    });
});
