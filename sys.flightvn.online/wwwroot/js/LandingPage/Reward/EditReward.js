function CreateRowTable() {
    debugger;
    const tableBody = document.querySelector('#tableAddRow tbody');

    // Tạo một dòng mới
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
            <td>
                <input type="text" name="RewardName[]" class="form-control" placeholder="Giải thưởng" />
            </td>
            <td>
                <input type="text" name="RewardFrom[]" class="form-control" placeholder="Hãng" />
            </td>
            <td>
                <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
            </td>
        `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);
}

function DeleteRowTable(button) {
    const row = button.closest('tr');
    row.remove();
}


$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(this);
    $.ajax({
        type: 'POST',
        url: '../LandingPage/SaveEditReward',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            if (response.success) {
                CustomSweetAlert_Success_ReloadPage(response.message);
            } else {
                CustomSweetAlert_Error(response.message);
            }
        }
    });
});
