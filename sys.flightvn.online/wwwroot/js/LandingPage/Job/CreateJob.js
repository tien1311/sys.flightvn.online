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
CKEDITOR.config.toolbar = [
    ['Styles', 'Format', 'Font', 'FontSize'],
    '/',
    ['Bold', 'Italic', 'Underline', 'StrikeThrough', '-', 'Undo', 'Redo', '-', 'Cut', 'Copy', 'Paste', 'Find', 'Replace', '-', 'Outdent', 'Indent', '-', 'Print'],
    '/',
    ['NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
    ['Image', 'Table', '-', 'Link', 'Flash', 'Smiley', 'TextColor', 'BGColor', 'Source']
];

CKEDITOR.replace('Description', {
    height: 200,
});

CKEDITOR.replace('Requirement', {
    height: 200,
});

CKEDITOR.replace('Benefit', {
    height: 200,
});

$('#submitForm').on('submit', function (e) {
    e.preventDefault();
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(this);

    var Description = CKEDITOR.instances['Description'].getData();
    var Requirement = CKEDITOR.instances['Requirement'].getData();
    var Benefit = CKEDITOR.instances['Benefit'].getData();

    formData.append('Description', Description);
    formData.append('Requirement', Requirement);
    formData.append('Benefit', Benefit);

    $.ajax({
        type: 'POST',
        url: '../LandingPage/SaveCreateJob',
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
