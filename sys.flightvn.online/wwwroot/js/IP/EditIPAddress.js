let rowCreateTable = 0;
function CreateRowTable() {
    const tableBody = document.querySelector('.tableFormIncrease tbody');
    // Tạo một dòng mới
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
            <td>
                <input type="text" name="ipAddress[]" class="form-control ipAddress" placeholder="Địa chỉ IP"/>
            </td>
            <td></td>
             <td>
                <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
            </td>
        `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);
    rowCreateTable++;

    // Tạo nút Lưu nếu chưa tồn tại
    if (rowCreateTable === 1) {
        const actionGroup = document.getElementById('action-group-btn');
        const saveButton = document.createElement('a');
        saveButton.href = "javascript:;";
        saveButton.className = "btn btn-primary";
        saveButton.id = "SaveIPAddress";
        saveButton.innerText = "Lưu";
        actionGroup.appendChild(saveButton);
    }

}

function DeleteRowTable(button) {
    const row = button.closest('tr');
    row.remove();
    rowCreateTable--;
    // Xóa nút Lưu nếu không còn dòng nào
    if (rowCreateTable === 0) {
        const actionGroup = document.getElementById('action-group-btn');
        const saveButton = document.getElementById('SaveIPAddress');
        if (saveButton) {
            actionGroup.removeChild(saveButton);
        }
    }

}

$('body').on('click', '#SaveIPAddress', function (e) {
    $('#loadingOverlay').css('display', 'flex');
    e.preventDefault();
    // Lấy tất cả các input có tên là endpoint[]
    const ipAddress = $('input[name="ipAddress[]"]').map(function () {
        return $(this).val();
    }).get().filter(function (value) {
        return value !== ""; // Lọc ra các giá trị không trống
    });

    if (ipAddress.length > 0) {
        const PartnerId = document.getElementById('PartnerId').value;

        $.ajax({
            url: '../KyThuat/SaveEditIPAddress',
            type: 'POST',
            data: { PartnerId: PartnerId, ipAddress: ipAddress },
            success: function (data) {
                $('#loadingOverlay').css('display', 'none');
                if (data.success) {
                    CustomSweetAlert_Success_ReloadPage('Dữ liệu đã được lưu thành công!');
                } else {
                    CustomSweetAlert_Error('Có lỗi xảy ra!');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#loadingOverlay').css('display', 'none');
                console.error('Error:', errorThrown);
                CustomSweetAlert_Error('Có lỗi xảy ra khi gửi dữ liệu.');
            }
        });
    } else {
        CustomSweetAlert_Error('Vui lòng nhập ít nhất một endpoint.');
    }
});

$('body').on('click', '.DeleteIP', function (e) {
    e.preventDefault();
    var id = $(this).data("id");
    CustomSweetAlert_Confirm('Bạn có chắc là muốn xoá nội dung này?', 'Không thể hoàn tác sau khi xoá').then((result) => {
        if (result.isConfirmed) {
            $('#loadingOverlay').css('display', 'flex');
            $.ajax({
                url: '../KyThuat/DeleteIPAddress',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    $('#loadingOverlay').css('display', 'none');
                    if (rs.success) {
                        CustomSweetAlert_Success_ReloadPage(rs.message)
                    }
                }
            });
        }
    })
})

