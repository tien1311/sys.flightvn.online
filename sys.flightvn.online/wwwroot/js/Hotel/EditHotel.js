var countImg = 1;
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-outline-primary btn-block",
        cancelButton: "btn btn-danger"
    },
    buttonsStyling: false
});
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
        `<div class="row  contain-listImg" id="RowImg` + countImg + `">
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
                            <input class="btn btn-danger" onclick="DeleteRowImg(` + countImg + `)"  type="button" value="-" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-2 col-xs-6">
                    <input class="form-check-input" type="radio" name="flexRadioDefault" id="flexRadioDefault` + countImg + `">
                    <label class="form-check-label" >
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
    var e = 'RowImg' + SoDong;
    const element = document.getElementById(e);
    element.remove();
}
function Save() {
    $('#loadingOverlay').css('display', 'flex');
    var formData = new FormData(document.getElementById('submitForm'));

    var longDescription = CKEDITOR.instances['LongDescription'].getData();

    var checkMainImg = 0;
    var countlist = document.querySelectorAll('.contain-DetailImg').length;
    for (var i = 1; i <= countlist; i++) {
        var imgs = {};
        var imagesURL = "ImageURL" + i;
        var mainImgs = "MainImg" + i;
        imgs.imagesURL = document.getElementById(imagesURL).value;
        imgs.mainImgs = document.getElementById(mainImgs).checked;
        if (document.getElementById(mainImgs).checked == true) {
            checkMainImg++;
        }
        formData.append('mainImgs', imgs.mainImgs);
        formData.append('imagesURL', imgs.imagesURL);
    }
    for (var i = 1; i <= countImg; i++) {
        var images = {};
        var detailImage = "file" + i;
        var mainImg = "flexRadioDefault" + i;
        images.ImageURL = document.getElementById(detailImage).files[0];
        images.MainImage = document.getElementById(mainImg).checked;
        if (document.getElementById(mainImg).checked == true) {
            checkMainImg++;
        }
        formData.append('mainImages', images.MainImage);
        formData.append('imageFiles', images.ImageURL);
    }

    formData.append('LongDescription', longDescription);

    $.ajax({
        type: "POST",
        url: "../Hotel/SaveEditHotel",
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
        failure: function (response) {
            $('#loadingOverlay').css('display', 'none');
            alert(response.responseText);
        },
        error: function (response) {
            $('#loadingOverlay').css('display', 'none');
            alert(response.responseText);
        }
    });
}
function DeleteImg(id) {
    $.ajax({
        type: "POST",
        url: "../Hotel/DeleteImg",
        data: { id: id },
        success: function (response) {
            if (response.success) {
                $('#ImageContainer_' + id).remove();
                Swal.fire({
                    imageUrl: "/images/success.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            }
            else {
                Swal.fire({
                    imageUrl: "/images/fail.png",
                    imageWidth: 100,
                    imageHeight: 100,
                    title: response.message,
                    confirmButtonText: 'Đóng',
                });
            }
          
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
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
