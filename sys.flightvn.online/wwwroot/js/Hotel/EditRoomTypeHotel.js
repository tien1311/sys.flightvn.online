document.querySelector('#roomTypeTable').addEventListener('click', function (e) {
    if (e.target && e.target.matches('a.remove-row')) {
        e.preventDefault();
        removeRow(e.target);
    }
});

document.querySelector('#addRowBtn').addEventListener('click', function (e) {
    e.preventDefault();
    addNewRow();
});

function addNewRow() {
    const table = document.querySelector('#roomTypeTable tbody');
    const firstRow = table.querySelector('tr');
    const newRow = firstRow.cloneNode(true);

    // Clear values of the cloned row
    newRow.querySelectorAll('input, textarea').forEach(input => {
        input.value = '';
    });

    // Remove the add button from the cloned row if it exists
    const addButton = newRow.querySelector('#addRowBtn');
    if (addButton) {
        addButton.remove();
    }

    // Add remove button to the new row
    const lastCell = newRow.querySelector('td:last-child');
    const removeButton = document.createElement('a');
    removeButton.href = '#';
    removeButton.className = 'btn btn-danger remove-row';
    removeButton.textContent = ' - ';
    lastCell.appendChild(removeButton);

    table.appendChild(newRow);

    // Gắn sự kiện format cho các ô input số mới
    const priceInputs = newRow.querySelectorAll('.price-input');
    priceInputs.forEach(input => {
        input.addEventListener('input', function () {
            var inputValue = this.value.replace(/[^\d]/g, '');
            if (inputValue === '') {
                // If empty, set input value to an empty string
                this.value = '';
                return; // Exit the function
            }
            var number = parseFloat(inputValue);
            var formattedNumber = formatNumber(number);
            this.value = formattedNumber;
        });
    });

}

function removeRow(button) {
    const row = button.closest('tr');
    row.parentNode.removeChild(row);
}



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

var priceInputs = document.getElementsByClassName('price-input');
for (var i = 0; i < priceInputs.length; i++) {
    // Add input event listener to format input value
    priceInputs[i].addEventListener('input', function () {
        // Get input value and remove non-digit characters
        var inputValue = this.value.replace(/[^\d]/g, '');
        // Check if the input value is empty
        if (inputValue === '') {
            // If empty, set input value to an empty string
            this.value = '';
            return; // Exit the function
        }
        // Parse input value as float
        var number = parseFloat(inputValue);
        // Format number
        var formattedNumber = formatNumber(number);
        // Update input value with formatted number
        this.value = formattedNumber;
    });
}

function Save() {
    debugger
    const formData = new FormData();

    // Collect room type data
    const roomTypeNames = document.getElementsByName("roomTypeName[]");
    const roomTypeMaxPersons = document.getElementsByName("roomTypeMaxPerson[]");
    const roomTypePrices = document.getElementsByName("roomTypePrice[]");
    const roomTypeDiscountPrices = document.getElementsByName("roomTypeDiscountPrice[]");
    const roomTypeDescriptions = document.getElementsByName("roomTypeDescription[]");
    const roomTypeProductID = document.getElementsByName("productId[]");

    for (let i = 0; i < roomTypeNames.length; i++) {
        debugger
        let roomType = {};
        roomType.Name = roomTypeNames[i].value;
        roomType.MaxPerson = roomTypeMaxPersons[i].value;
        roomType.Price = roomTypePrices[i].value;

        var new_price = roomTypePrices[i].value;
        new_price = new_price.trim().replace(/\D/g, '');

        roomType.DiscountPrice = roomTypeDiscountPrices[i].value ? roomTypeDiscountPrices[i].value : 0; // Default to 0 if DiscountPrice is absent

        var new_discountPrice = roomTypeDiscountPrices[i].value;
        new_discountPrice = new_discountPrice.trim().replace(/\D/g, '');

        roomType.Description = roomTypeDescriptions[i].value;
        roomType.ProductID = parseFloat(roomTypeProductID[i].value);

        if (!roomType.Name || !roomType.MaxPerson || !roomType.Price || !roomType.Description) {
            Swal.fire({
                imageUrl: "/images/fail.png",
                imageWidth: 100,
                imageHeight: 100,
                title: 'Bạn phải thêm đủ thông tin cho loại phòng của khách sạn',
                confirmButtonText: 'Đóng',
            });
            return;
        }

        debugger
        if (parseFloat(new_discountPrice) >= parseFloat(new_price)) {
            Swal.fire({
                imageUrl: "/images/fail.png",
                imageWidth: 100,
                imageHeight: 100,
                title: 'Giá chiết khấu không thể lớn hơn Giá bán',
                confirmButtonText: 'Đóng',
            });
            return;
        }

        formData.append('roomTypeName[]', roomType.Name);
        formData.append('roomTypeMaxPerson[]', roomType.MaxPerson);
        formData.append('roomTypePrice[]', roomType.Price);
        formData.append('roomTypeDiscountPrice[]', roomType.DiscountPrice);
        formData.append('roomTypeDescription[]', roomType.Description);
        formData.append('productId[]', roomType.ProductID);
    }

    // Gather additional form data if needed
    // Example:
    // formData.append('exampleField', document.getElementById('exampleField').value);

    $.ajax({
        type: "POST",
        url: "../Hotel/SaveEditRoomTypeHotel",  // Adjust the URL to match your backend endpoint
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            Swal.fire({
                imageUrl: "/images/success.png",
                imageWidth: 100,
                imageHeight: 100,
                title: 'Lưu thành công',
                confirmButtonText: 'Đóng',
            });
            location.reload();
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}