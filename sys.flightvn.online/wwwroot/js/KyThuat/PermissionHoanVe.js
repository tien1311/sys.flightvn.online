$(document).ready(function () {
    // Xử lý khi checkbox "Read" được chọn hoặc bỏ chọn
    $('#table-permission').on('change', '#checkAllRead', function () {
        let isChecked = $(this).is(':checked');
        $('.ReadCheckBox').prop('checked', isChecked);
    });

    $('#table-permission').on('change', '#checkAllWrite', function () {
        let isChecked = $(this).is(':checked');
        $('.WriteCheckBox').prop('checked', isChecked);
    });

    $('#table-permission').on('change', '#checkAllDelete', function () {
        let isChecked = $(this).is(':checked');
        $('.DeleteCheckBox').prop('checked', isChecked);
    });

    $('#table-permission').on('change', '#checkAllExcel', function () {
        let isChecked = $(this).is(':checked');
        $('.ExportExcelCheckBox').prop('checked', isChecked);
    });

    $('#submitForm').on('change', '#checkAllPermission', function () {
        let isChecked = $(this).is(':checked');
        $('.ExportExcelCheckBox').prop('checked', isChecked);
        $('.DeleteCheckBox').prop('checked', isChecked);
        $('.WriteCheckBox').prop('checked', isChecked);
        $('.ReadCheckBox').prop('checked', isChecked);
    });

    $('#department').change(function () {
        var departmentCode = $(this).val();
        $.get('/KyThuat/GetAccountFromDepartment', { departmentCode: departmentCode }, function (data) {
            $('#user').empty();
            $('#user').append('<option value="">Chọn nhân viên</option>');
            $.each(data, function (index, value) {
                $('#user').append('<option value="' + value.tenDangNhap + '">' + value.hoTen + '</option>');
            });
            $('input[type="checkbox"]').prop('checked', false);
        });
    });

    $('#user').change(function () {
        $('#loadingOverlay').css('display', 'flex');
        var userId = $(this).val();
        $.get('/KyThuat/GetUserPermission', { userId: userId }, function (data) {
            $('#table-permission').empty();
            $('#table-permission').append(data);
            $('#checkAllPermission').prop('checked', false);
            $('#loadingOverlay').css('display', 'none');
        });
    });

    $('#submitForm').on('submit', function (e) {
        e.preventDefault();
        var department = $('#department').val();
        var userId = $('#user').val();
        var formData = $(this).serializeArray();

        // Thêm department và userId vào dữ liệu gửi đi
        formData.push({ name: 'departmentCode', value: department });
        formData.push({ name: 'userId', value: userId });

        if (department && !userId) {
            Swal.fire({
                title: 'Xác nhận',
                text: "Bạn có chắc phân quyền toàn bộ phòng ban này?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Đồng ý',
                cancelButtonText: 'Hủy'
            }).then((result) => {
                if (result.isConfirmed) {
                    submitFormData(formData);
                }
            });
        } else {
            submitFormData(formData);
        }
    });

    function submitFormData(formData) {
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: 'POST',
            url: '../KyThuat/InsertOrUpdatePermission',
            data: formData,
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
                if (response.success) {
                    Swal.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thành công",
                        text: response.message,
                        button: "Đóng",
                    })
                } else {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thất bại",
                        text: response.message ,
                        button: "Đóng",
                    });
                }
            }
        });
    }
})