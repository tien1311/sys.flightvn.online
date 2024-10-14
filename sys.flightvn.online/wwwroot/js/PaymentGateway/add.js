    $(document).ready(function () {
        $("#addFee").click(function () {
            var newRow = $(".feeRow").first().clone();
            newRow.find('input').val('');
            $("#feeTable tbody").append(newRow);
        });

    $(document).on('click', '.removeFee', function () {
        $(this).closest('tr').remove();
        });

    $('body').on('change', '.requestTypeImageInputFile', function () {
            var input = $(this);
    var fileName = input.val(); // Lấy tên file đã chọn
    // Loại bỏ phần "C:\fakepath\" từ đường dẫn
    fileName = fileName.replace(/^.*[\\\/]/, '');
    // Hiển thị đường dẫn của file trong ô input text
    input.closest('.feeRow').find('.requestTypeImageInputValue').val("wwwroot/lib/Content/Uploads/logo/" + fileName);
        });


    $('#addForm').submit(function (e) {
        e.preventDefault();
    var formData = new FormData($(this)[0]);
    $.ajax({
        url: '../PaymentGateway/Add',
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
            window.location.href = "Index";
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

    });
