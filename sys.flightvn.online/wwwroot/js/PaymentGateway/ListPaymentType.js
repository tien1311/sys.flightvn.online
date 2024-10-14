$('.feeRow').each(function () {
    var row = $(this);
    var fixedCostInput = row.find('.fixedCostInput');
    var inputValue = fixedCostInput.val();
    inputValue = parseFloat(inputValue);
    // Lấy giá trị từ input
    var formattedValue = inputValue.toFixed(0);

    // Thiết lập giá trị đã làm tròn vào input FixedCosts trong hàng này
    fixedCostInput.val(formattedValue);
});
$(document).ready(function () {

    $('body').on('change', '.PaymentImage', function () {
        var id = $(this).data('id');
        var formData = new FormData();
        var fileInput = $(this);

        formData.append('PaymentId', id);
        formData.append('PaymentImage', fileInput.prop('files')[0]);


        $.ajax({
            url: '../PaymentGateway/ChangePaymentImage',
            type: 'POST',
            data: formData,
            async: false,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.success) {
                    alert('Cập nhật hình ảnh thành công.');
                    location.reload();
                } else {
                    alert('Có lỗi xảy ra khi cập nhật hình ảnh.');
                }
            },
            error: function () {
                alert('Có lỗi xảy ra khi gửi yêu cầu.');
            }
        });
    });



    $('body').on('change', '.btnActive', function () {
        var checkbox = $(this);
        var id = checkbox.data('id');

        $.ajax({
            url: '../PaymentGateway/IsActive',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                if (result.success) {
                    checkbox.attr("checked", "checked");
                }
                else {
                    checkbox.removeAttr('checked');
                }
            }
        })
    })

    $('body').on('change', '.btnFeeActive', function () {
        var checkbox = $(this);
        var id = checkbox.data('id');

        $.ajax({
            url: '../PaymentGateway/FeeIsActive',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                if (result.success) {
                    checkbox.attr("checked", "checked")
                }
                else {
                    checkbox.removeAttr('checked')
                }
            }
        })
    });

    $('body').on('click', '.btnDeleteFee', function () {
        var id = $(this).data("id");
        Swal.fire({
            title: 'Bạn có thực sự muốn xoá?',
            text: 'Bạn sẽ không thể khôi phục sau khi xoá',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xoá',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '../PaymentGateway/DeleteFee',
                    type: 'POST',
                    data: { id: id },
                    success: function (result) {
                        if (result.success) {
                            location.reload();
                        }
                    }
                });
            }
        });
    });


    // Xử lý sự kiện click cho các nút "Thêm phí" chung
    $(".addFee").click(function () {
        var tableId = $(this).data('tableid');
        var targetTable = $("#feeTable" + tableId);
        var newRow = targetTable.find(".feeRow").first().clone();
        newRow.find('input').val('');

        newRow.find('td:nth-child(2) img').attr('src', '/images/PaymentGateway/enviet-logo.png');

        // Xóa cột chứa input file và thêm thông báo
        newRow.find('td.custom-file').remove();
        newRow.find('td:nth-child(7)').html('Chức năng Đổi hình sẽ hiển thị sau khi lưu thành công');

        var removeButton = $('<button type="button" class="btn btn-danger removeFee">Xóa</button>');
        newRow.find('td:nth-child(8)').empty().append(removeButton);

        // Thêm dòng mới vào bảng
        targetTable.find("tbody").append(newRow);
    });

    $(document).on('click', '.removeFee', function () {
        $(this).closest('tr').remove();
    });


   

    $('body').on('change', '.requestTypeImage', function () {
        var id = $(this).data('id');
        var formData = new FormData();
        var fileInput = $(this);
        formData.append('PaymentId', id);
        formData.append('paymentFeeImage', fileInput.prop('files')[0]);

        $.ajax({
            url: '../PaymentGateway/ChangePaymentFeeImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success) {
                    alert(result.message);
                    location.reload();
                }
                else {
                    alert(result.message);
                }
            },
        });
    });


});

