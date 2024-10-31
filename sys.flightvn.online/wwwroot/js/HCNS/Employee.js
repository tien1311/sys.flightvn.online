$("#BtnCreate").click(function () {
    $('#loadingOverlay').css('display', 'flex');
    $.ajax({
        type: "Get",
        url: "../Employee/CreateEmployee",
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            $('#openPopup').html(response);
            $('#openPopup').modal({
                backdrop: 'static',
                keyboard: false,
                show: true
            });
        },
        failure: function (response) {
            $('#loadingOverlay').css('display', 'none');
            alert(response.responseText);
        },
        error: function (response) {
            $('#loadingOverlay').css('display', 'none');
            alert(response.responseText);
        }
    });
});

function DeleteEmployeeID(id) {

    CustomSweetAlert_Confirm('Bạn thực sự muốn xoá?', 'Sau khi xoá thì chỉ có thể hoàn tác trong vòng 5s').then((result) => {
        if (result.isConfirmed) {
            $('#loadingOverlay').css('display', 'flex');
            $.ajax({
                url: '../Employee/DeleteEmployeeID',
                type: 'POST',
                data: { EmployeeID: id },
                success: function (rs) {
                    $('#loadingOverlay').css('display', 'none');
                    if (rs.success) {
                        CustomSweetAlert_Success_ReloadPage(rs.message)
                    } else {
                        CustomSweetAlert_Error(rs.message)
                    }
                },
                error: function () {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thất bại",
                        text: rs.message,
                        button: "Đóng",
                    });
                }
            });
        }
    });
}
