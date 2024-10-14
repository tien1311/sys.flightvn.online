$("#BtnCreate").click(function () {
    $.ajax({
        type: "GET",
        url: `../Voucher/CreateVoucher`,
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});

$("#gridVoucher .Edit").click(function () {
    var id = String($(this).closest('tr').attr('id'));
    $.ajax({
        type: "GET",
        url: `../Voucher/EditVoucher`,
        data: {
            ID: id
        },
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});
function ActiveVoucher(active, id) {
    $.ajax({
        type: "POST",
        url: `../Voucher/ChangeActiveVoucher`,
        data: {
            ID: id,
            Active: active
        },
        success: function (response) {
            if (response == true) {
                alert("Bạn đã thay đổi thành công");
            }
            else {
                alert("Bạn đã thay đổi không thành công");
                location.reload();
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function DeleteVoucher(id) {
    Swal.fire({
        title: 'Bạn thực sự muốn xoá?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Huỷ bỏ',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `../Voucher/DeleteVoucher`,
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.success) {
                        Swal.fire({
                            imageUrl: "/images/success.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: rs.message,
                            button: "Đóng",
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire({
                            imageUrl: "/images/fail.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: rs.message,
                            button: "Đóng",
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thất bại.",
                        button: "Đóng",
                    });
                }
            });
        }
    });
}