$(document).ready(function () {
    $("#BtnCreate").click(function () {
        $.ajax({
            type: "POST",
            url: "../Hotel/CreateHotelService",
            success: function (response) {
                debugger;
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
    $(".btnEdit").click(function () {
        var id = $(this).data('id');
        $.ajax({
            type: "POST",
            url: "../Hotel/EditHotelService",
            data: {id:id},
            success: function (response) {
                debugger;
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

    $('.btnDelete').click(function (e) {
        e.preventDefault();
        var id = $(this).data('id');

        Swal.fire({
            title: 'Bạn có thực sự muốn xoá?',
            text: "Hành động này không thể hoàn tác!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có, xoá nó!',
            cancelButtonText: 'Hủy bỏ'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: "../Hotel/DeleteHotelService",
                    data: { id: id },
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
                    },
                });
            }
        });
    });

});