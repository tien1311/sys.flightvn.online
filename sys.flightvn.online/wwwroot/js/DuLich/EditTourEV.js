$(document).ready(function () {
    CKEDITOR.replace('long_notes', {
        height: 200,
        filebrowserUploadUrl: '../Data/UploadCKEditorDuLich'
    });

    $('form').on('submit', function (e) {
        for (instance in CKEDITOR.instances) {
            CKEDITOR.instances[instance].updateElement();
        }
    });
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


$(document).ready(function () {
    $('#loading-overlay').hide();


    $("#gridReport").on('click', '.Chitiet', function () {
        $('#loading-overlay').show();

        var Tour_Id = $(this).closest('tr').find('td:nth-child(3)').text().trim();

        $.ajax({
            type: "POST",
            url: "../DuLich/EditTourEV",
            data: { Tour_Id: Tour_Id },
            success: function (response) {
                $('#loading-overlay').hide();
                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
                });
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });
});