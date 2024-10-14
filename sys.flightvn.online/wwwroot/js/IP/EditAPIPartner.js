let rowCreateTable = 1
function CreateRowTable() {
    const tableBody = document.querySelector('.tableFormIncrease tbody');
    // Tạo một dòng mới
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
            <td>
                <input type="text" name="IPPartners[${rowCreateTable}].IPAddress" class="form-control" placeholder="Địa chỉ IP" />
            </td>
             <td>
                <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
            </td>
        `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);
    // Cập nhật lại chỉ số cho các input còn lại
    const rows = tableBody.querySelectorAll('tr');
    rows.forEach((currentRow, index) => {
        const input = currentRow.querySelector('input[type="text"]');
        if (input) {
            input.name = `IPPartners[${index}].IPAddress`; // Cập nhật tên input
        }
    });
    // Cập nhật lại biến đếm dòng
    rowCreateTable = rows.length; // Đếm số dòng còn lại
}

function DeleteRowTable(button) {

    const row = button.closest('tr');
    const tableBody = row.parentNode;
    row.remove();
    // Cập nhật lại chỉ số cho các input còn lại
    const rows = tableBody.querySelectorAll('tr');
    rows.forEach((currentRow, index) => {
        const input = currentRow.querySelector('input[type="text"]');
        if (input) {
            input.name = `IPPartners[${index}].IPAddress`; // Cập nhật tên input
        }
    });
    // Cập nhật lại biến đếm dòng
    rowCreateTable = rows.length; // Đếm số dòng còn lại
}

$('body').on('submit', '#submitForm', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = $(this).serialize();
    $.ajax({
        type: 'POST',
        data: formData,
        url: "../KyThuat/SaveEditAPIPartner",
        success: function (rs) {
            $('#loadingOverlay').css('display', 'none');
            if (rs.success) {
                CustomSweetAlert_Success_ReloadPage(rs.message);
            }
            else {
                CustomSweetAlert_Error(rs.message);
            }
        }
    })
})
