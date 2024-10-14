let rowCreateTable = 0;
function CreateRowTable() {
    const tableBody = document.querySelector('.tableFormIncrease tbody');
    // Tạo một dòng mới
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
            <td>
                <input type="text" name="endpoint[]" class="form-control endpoint" placeholder="EndPoint"/>
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
        saveButton.id = "SaveEndPoint";
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
        const saveButton = document.getElementById('SaveEndPoint');
        if (saveButton) {
            actionGroup.removeChild(saveButton);
        }
    }

}


$('body').on('change', '.isActived', function () {
    $('#loadingOverlay').css('display', 'flex');
    var id = $(this).data("id");
    $.ajax({
        type: "POST",
        url: "../KyThuat/IsIPWhiteListActive",
        data: {
            id: id
        },
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            if (response) {
                CustomSweetAlert_Success(response.message)
            }
            else {
                CustomSweetAlert_Error(response.message)
            }
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


$('body').on('click', '#SaveEndPoint', function (e) {
    e.preventDefault();
    debugger;
    // Lấy tất cả các input có tên là endpoint[]
    const endpoints = $('input[name="endpoint[]"]').map(function () {
        return $(this).val();
    }).get().filter(function (value) {
        return value !== ""; // Lọc ra các giá trị không trống
    });

    if (endpoints.length > 0) {
        const IPAddressId = document.getElementById('IPPartnerId').value;

        $.ajax({
            url: '../KyThuat/SaveCreateEndpoint',
            type: 'POST',
            data: { IPAddressId: IPAddressId, endpoint: endpoints },
            success: function (data) {
                if (data.success) {
                    CustomSweetAlert_Success_ReloadPage('Dữ liệu đã được lưu thành công!');
                } else {
                    CustomSweetAlert_Error('Có lỗi xảy ra!');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error:', errorThrown);
                CustomSweetAlert_Error('Có lỗi xảy ra khi gửi dữ liệu.');
            }
        });
    } else {
        CustomSweetAlert_Error('Vui lòng nhập ít nhất một endpoint.');
    }
});

