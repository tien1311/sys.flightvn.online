var countImg = 1;
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-outline-primary btn-block",
        cancelButton: "btn btn-danger"
    },
    buttonsStyling: false
});

$('.chosen-select-multi-create').chosen();

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


function previewImage(event) {
    var id = (event.target.id).slice(4, 5);
    var ImgPreview = "imagePreview" + id;
    var fileName = "file-name-popup" + id;
    // Get the file from the input field
    var file = event.target.files[0];


    // Create a FileReader object
    var reader = new FileReader();

    // Set the onload event handler
    reader.onload = function () {

        // Remove the previous img element, if any
        var imagePreview = document.getElementById(ImgPreview);
        while (imagePreview.firstChild) {
            imagePreview.removeChild(imagePreview.firstChild);
        }
        // Create an img element
        var img = document.createElement('img');

        // Set the src attribute to the contents of the file
        img.src = reader.result;

        for (var i = 0; i < event.target.files.length; i++) {
            document.getElementById(fileName).value = event.target.files[i].name;
        }

        // Append the img element to the div element
        imagePreview.appendChild(img);
    }

    // Read the file as a data URL
    reader.readAsDataURL(file);
}
function CreateRowImg() {
    countImg++;
    document.querySelector('#addRowsImg').insertAdjacentHTML(
        'beforebegin',
        `<div class="row contain-listImg" id="RowImg` + countImg + `">
            <div class="col-sm-7 col-xs-12">
                <div class="row">
                    <div class="col-xs-10">
                        <div class="inputfile-box">
                            <input type="file" id="file` + countImg + `" class="inputfile" onchange="previewImage(event)" accept="image/*">
                            <label for="file` + countImg + `">
                                <span class="file-button btn btn-primary" style="margin-bottom: 6px;">
                                    Chọn file
                                </span>
                            </label>
                            <input id="file-name-popup` + countImg + `" class="file-box" placeholder="Chọn tập tin">
                        </div>
                    </div>
                    <div class="col-xs-2">
                        <input class="btn btn-danger" onclick="DeleteRowImg(` + countImg + `)" type="button" value="-" />
                    </div>
                </div>
            </div>
            <div class="col-sm-2 col-xs-6">
                <input class="form-check-input" type="radio" name="flexRadioDefault" id="flexRadioDefault` + countImg + `">
                <label class="form-check-label">
                    Hình đại diện
                </label>
            </div>
            <div class="col-sm-3 col-xs-6">
                <div class="main-img" id="imagePreview` + countImg + `">
                </div>
            </div>
        </div>`);
}
function DeleteRowImg(SoDong) {
    countImg--;
    var e = 'RowImg' + SoDong;
    const element = document.getElementById(e);
    element.remove();
}

function CreateRowTable() {
    const tableBody = document.querySelector('.tableRoomType tbody');

    // Tạo một dòng mới
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
            <td>
                <input type="text" name="roomTypeName[]" class="form-control" placeholder="Tên phòng" />
            </td>
            <td>
                <input type="number" step="1" min="1" name="roomTypeMaxPerson[]" class="form-control" placeholder="Số người tối đa" />
            </td>
            <td>
                <input type="text" name="roomTypePrice[]" class="form-control Price price-input" placeholder="Giá bán" />
            </td>
            <td>
                <input type="text" name="roomTypeDiscountPrice[]"  class="form-control DiscountPrice price-input" placeholder="Giá chiết khấu" />
            </td>
            <td>
                <textarea class="form-control" name="roomTypeDescription[]"> </textarea>
            </td>
            <td>
                <button class="btn btn-danger" onclick="DeleteRowTable(this);" type="button">-</button>
            </td>
        `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);

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

function DeleteRowTable(button) {
    const row = button.closest('tr');
    row.remove();
}

function Save() {
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(document.getElementById('submitForm'));

    var checkMainImg = 0;
    for (var i = 1; i <= countImg; i++) {
        var images = {};
        var detailImage = "file" + i;
        var mainImg = "flexRadioDefault" + i;
        var fileInput = document.getElementById(detailImage);
        var mainImageInput = document.getElementById(mainImg);

        if (fileInput && mainImageInput) {
            images.ImageURL = fileInput.files[0];
            images.MainImage = mainImageInput.checked;
            if (mainImageInput.checked == true) {
                checkMainImg++;
            }
            formData.append('mainImages', images.MainImage);
            formData.append('imageFiles', images.ImageURL);
        }
    }

    var longDescription = CKEDITOR.instances['LongDescription'].getData();
    formData.append('LongDescription', longDescription);


    $.ajax({
        type: "POST",
        url: "../Hotel/SaveCreateHotel",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            Swal.fire({
                icon: response.success ? 'success' : 'error',
                title: response.success ? 'Lưu thành công' : 'Lưu không thành công',
                text: response.message,
            }).then(() => {
                if (response.success) {
                    location.reload();
                }
            });
        },
        error: function (xhr) {
            $('#loadingOverlay').css('display', 'none');
            Swal.fire({
                icon: 'error',
                title: 'Có lỗi xảy ra',
                text: xhr.responseText,
            });
        }
    });
}

const textarea = document.getElementById('ShortDescription');
const charCount = document.getElementById('charCount');

function updateCharCount() {
    const currentLength = textarea.value.length;
    charCount.textContent = `${currentLength}/200 ký tự`;
}

textarea.addEventListener('input', updateCharCount);
updateCharCount(); // Initialize the character count on page load
