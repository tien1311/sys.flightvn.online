const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-outline-primary btn-block",
        cancelButton: "btn btn-danger"
    },
    buttonsStyling: false
});



function previewImage(event) {
    var id = (event.target.id).slice(4);
    var ImgPreview = "imagePreview" + id;
    var fileName = "file-name-popup" + id;

    // Get the file from the input field
    var file = event.target.files[0];

    if (file) {
        // Create a FileReader object
        var reader = new FileReader();

        // Set the onload event handler
        reader.onload = function () {
            // Remove the previous img element, if any
            var imagePreview = document.getElementById(ImgPreview);
            if (imagePreview) {
                while (imagePreview.firstChild) {
                    imagePreview.removeChild(imagePreview.firstChild);
                }

                // Create an img element
                var img = document.createElement('img');
                img.src = reader.result;
                img.style.maxWidth = "100%"; // Ensure the image fits within the preview area
                imagePreview.appendChild(img);

                // Set the file name in the input field
                document.getElementById(fileName).value = file.name;
            } else {
                console.error("Image preview element not found: " + ImgPreview);
            }
        };

        reader.onerror = function () {
            console.error("There was an error reading the file!");
        };

        reader.readAsDataURL(file);
    } else {
        console.error("No file selected or file not readable.");
    }
}

var countImg = 1;
function CreateRowImg() {
    countImg++;
    var newRow = `
    <div class="row contain-listImg" id="RowImg${countImg}">
        <div class="col-sm-7 col-xs-12">
            <div class="row">
                <div class="col-xs-10">
                    <div class="inputfile-box">
                        <input type="file" id="file${countImg}" class="inputfile" onchange="previewImage(event)" accept="image/*">
                            <label for="file${countImg}">
                                <span class="file-button btn btn-primary" style="margin-bottom: 6px;">
                                    Chọn file
                                </span>
                            </label>
                            <input id="file-name-popup${countImg}" class="file-box" placeholder="Chọn tập tin">
                    </div>
                </div>
                        <div class="col-xs-2">
                            <input class="btn btn-danger" onclick="DeleteRowImg(${countImg})" type="button" value="-" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 col-xs-6">
                    <input class="form-check-input" type="radio" name="mainImage" id="flexRadioDefault${countImg}" onchange="checkOnlyOneMainImage()">
                        <label class="form-check-label" for="flexRadioDefault${countImg}">
                            Hình đại diện
                        </label>
        </div>
                    <div class="col-sm-2 col-xs-6">
                        <div class="main-img" id="imagePreview${countImg}">
                        </div>
                    </div>
                </div>`;
    document.querySelector('#addRowsImg').insertAdjacentHTML('beforebegin', newRow);
}

function DeleteRowImg(SoDong) {
    var e = 'RowImg' + SoDong;
    const element = document.getElementById(e);
    if (element) {
        element.remove();
    } else {
        console.error("Row element not found: " + e);
    }
}

function checkOnlyOneMainImage() {
    var radios = document.querySelectorAll('input[name="mainImage"]');
    radios.forEach((radio) => {
        radio.addEventListener('change', function () {
            if (this.checked) {
                radios.forEach((r) => {
                    if (r !== this) {
                        r.checked = false;
                    }
                });
            }
        });
    });
}

document.addEventListener('DOMContentLoaded', checkOnlyOneMainImage);



function handleFileChange(event, outputId) {
    var input = event.target;
    var fileName = input.files[0].name;
    document.getElementById(outputId).value = fileName;
}

function handleImageChange(event) {
    var input = event.target;
    var file = input.files[0];
    var reader = new FileReader();

    reader.onload = function (e) {
        var preview = document.getElementById('imagePreview');
        preview.innerHTML = '';
        var img = document.createElement('img');
        img.src = e.target.result;
        img.style.maxWidth = '100%';
        preview.appendChild(img);
    };

    reader.readAsDataURL(file);
    document.getElementById('fileImageName').value = file.name;
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


var priceInputs = document.getElementsByClassName('priceInput');
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
                        <option value="rs3s">KS 3s</option>
                        <option value="rs4s">RS 4s</option>
                        <option value="rs5s">RS 5s</option>
                    </select>
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="gia_nguoi_lon[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="gia_tre_em[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="gia_em_be[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="phu_thu_don[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="phu_thu_quoctich[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="hh_gia_nguoi_lon[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="hh_gia_tre_em[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="hh_gia_em_be[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="km_gia_nguoi_lon[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="km_gia_tre_em[]" />
                </td>
                <td>
                    <input type="text" class="form-control priceInput" name="km_gia_em_be[]" />
                </td>

                `;
    // Thêm dòng mới vào cuối bảng
    tableBody.appendChild(newRow);

    // Gắn sự kiện format cho các ô input số mới
    const priceInputs = newRow.querySelectorAll('.priceInput');
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

$(document).ready(function () {
    // Khởi tạo CKEditor cho trường 'long_notes'
    CKEDITOR.replace('long_notes', {
        height: 200,
        filebrowserUploadUrl: '/Data/UploadCKEditorDuLich'
    });

    // Khi form được submit, cập nhật nội dung từ CKEditor vào textarea
    $('#createTourForm').on('submit', function (e) {
        // Cập nhật nội dung CKEditor vào textarea
        for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }

        e.preventDefault();  // Ngăn chặn hành vi mặc định của form submit

        var formData = new FormData(this);
        var mainImageId = $('input[name="mainImage"]:checked').val();

        for (var i = 1; i <= countImg; i++) {
            var detailImage = "file" + i;
            var fileInput = document.getElementById(detailImage);
            if (fileInput && fileInput.files[0]) {
                var imageFile = fileInput.files[0];
                var isMainImage = (i == mainImageId);

                formData.append('mainImages', isMainImage);
                formData.append('imageFiles', imageFile);
            }
        }

        $.ajax({
            url: '../TourHot/CreateTour',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                // Ẩn overlay loading
                $('#custom-loading-overlay').hide();

                if (response.success) {
                    swalWithBootstrapButtons.fire({
                        imageUrl: "/images/success.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: response.message,
                        confirmButtonText: 'Đóng',
                    });
                    location.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        text: response.message || 'Đã có lỗi xảy ra!',
                        confirmButtonText: 'Đóng',
                    });
                }
            },
            error: function (error) {
                // Ẩn overlay loading
                $('#custom-loading-overlay').hide();
                console.error('Lỗi:', error);
            }
        });
    });

    // Gán sự kiện cho nút gửi
    $('#submitForm').click(function () {
        $('#createTourForm').submit();
    });
});




