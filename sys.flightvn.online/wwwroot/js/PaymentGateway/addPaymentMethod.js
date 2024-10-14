    $('#addForm').submit(function (e) {
        e.preventDefault();
    var formData = new FormData($(this)[0]);
    $.ajax({
        url: '../PaymentGateway/AddPaymentMethod',
    type: 'POST',
    data: formData,
    async: false,
    cache: false,
    contentType: false,
    processData: false,
    success: function (data) {
                if (data.success) {
        Swal.fire({
            title: data.message,
            icon: "success"
        }).then((result) => {
            window.location.href = "../PaymentGateway/Index";
        })
    } else {
        Swal.fire({
            title: "Lỗi",
            text: data.message,
            icon: "error"
        })

    }
            },
    error: function () {
        alert('Có lỗi xảy ra khi gửi yêu cầu.');
            }
        });
    return false;
    });
