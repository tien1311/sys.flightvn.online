




function formatNumber(number) {
    number = number.toFixed(0) + '';
    var x = number.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

var priceInputs = document.getElementsByClassName('priceInput');
for (var i = 0; i < priceInputs.length; i++) {
    priceInputs[i].addEventListener('input', function () {
        var inputValue = this.value.replace(/[^\d]/g, '');
        if (inputValue === '') {
            this.value = '';
            return;
        }
        var number = parseFloat(inputValue);
        var formattedNumber = formatNumber(number);
        this.value = formattedNumber;
    });
}
function CreateRowTable() {
    const tableBody = document.querySelector('.table-price tbody');
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
        <td>
            <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
        </td>
        <td>
            <select class="form-control" name="gia_loai">
                <option value="1s">KS 1s</option>
                <option value="2s">KS 2s</option>
                <option value="3s">KS 3s</option>
                <option value="4s">KS 4s</option>
                <option value="5s">KS 5s</option>
                <option value="rs3s">RS 3s</option>
                <option value="rs4s">RS 4s</option>
                <option value="rs5s">RS 5s</option>
            </select>
        </td>
        <td><input type="text" class="form-control priceInput" name="gia_nguoi_lon[]" /></td>
        <td><input type="text" class="form-control priceInput" name="gia_tre_em[]" /></td>
        <td><input type="text" class="form-control priceInput" name="gia_em_be[]" /></td>
        <td><input type="text" class="form-control priceInput" name="phu_thu_don[]" /></td>
        <td><input type="text" class="form-control priceInput" name="phu_thu_quoctich[]" /></td>
        <td><input type="text" class="form-control priceInput" name="hh_gia_nguoi_lon[]" /></td>
        <td><input type="text" class="form-control priceInput" name="hh_gia_tre_em[]" /></td>
        <td><input type="text" class="form-control priceInput" name="hh_gia_em_be[]" /></td>
        <td><input type="text" class="form-control priceInput" name="km_gia_nguoi_lon[]" /></td>
        <td><input type="text" class="form-control priceInput" name="km_gia_tre_em[]" /></td>
        <td><input type="text" class="form-control priceInput" name="km_gia_em_be[]" /></td>
    `;
    tableBody.appendChild(newRow);

    // Attach event listeners for new inputs
    const priceInputs = newRow.querySelectorAll('.priceInput');
    priceInputs.forEach(input => {
        input.addEventListener('input', function () {
            var inputValue = this.value.replace(/[^\d]/g, '');
            if (inputValue === '') {
                this.value = '';
                return;
            }
            var number = parseFloat(inputValue);
            var formattedNumber = formatNumber(number);
            this.value = formattedNumber;
        });
    });
}

function DeleteRowTable(button) {
    const row = button.closest('tr');
    row.remove();
}


