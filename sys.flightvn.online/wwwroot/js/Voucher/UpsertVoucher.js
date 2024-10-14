var countImg = 1;
let selectListServiceId = document.getElementById('ListServiceId')
let valueType = document.getElementById('TypeService').value;
$.get(
    `../Voucher/GetServicesByType`,
    { type: valueType },
    (data) => {
        if (data.success === true) {
            let optionstring = data.data.map((value, index) => {
                let output = `<option value="${value.id}">${value.name}</option>`;
                if (value.id == selectListServiceId.dataset.serviceid) {
                    output = `<option selected value="${value.id}">${value.name}</option>`;
                }
                return output;
            }).join("");
            selectListServiceId.innerHTML = optionstring;
            // init Chosen
            $(".chosen-select-tourlocation").chosen({ width: '100%' });
        }
    }
)
/********************* Tạm thời ẩn 

 Get Selected Service
let selectListServiceId = document.getElementById('ListServiceId')
let valueType = document.getElementById('ListServiceType').value
if (valueType != '') {
    $.get(
        '/Voucher/GetServicesByType',
        { type: valueType },
        (data) => {
            if (data.success === true) {
                let optionstring = data.data.map((value, index) => {
                    let output = `<option value="${value.id}">${value.name}</option>`;
                    if (value.id == selectListServiceId.dataset.serviceid) {
                        output = `<option selected value="${value.id}">${value.name}</option>`;
                    }
                    return output;
                }).join("");
                selectListServiceId.innerHTML = optionstring;
                selectListServiceId.classList.add("chosen-select-voucher")
                // init Chosen
                $(".chosen-select-voucher").chosen({ width: '100%' });
            }
        }
    )
}

 Handle Event
document.getElementById('ListServiceType').onchange = (e) => {
    let valueType = e.target.value;
    $.get(
        '/Voucher/GetServicesByType',
        { type: valueType },
        (data) => {
            if (data.success === true) {
                let optionstring = data.data.map((value, index) => {
                    let output = `<option value="${value.id}">${value.name}</option>`;
                    return output;
                }).join("");
                selectListServiceId.innerHTML = optionstring;
            }
        }
    )
}
*********************/

// Init CKEDITOR
CKEDITOR.replace('LongDescription', {
    height: 200,
});
function previewImage(event) {
    var id = (event.target.id).slice(4, 5);
    var ImgPreview = "imagePreview" + id;
    var fileName = "file-name-popup" + id;
    var radioMainId = "flexRadioDefault" + id;
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
            document.getElementById(radioMainId).value = event.target.files[i].name;
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
        `<div class="row  contain-listImg" id="RowImg` + countImg + `" data-number="${countImg}">
            <div class="col-sm-7 col-xs-12">
                <div class="row">
                    <div class="col-xs-10">
                        <div class="inputfile-box">
                            <input type="file" id="file` + countImg + `" class="inputfile" onchange="previewImage(event)" accept="image/*" name="imageFiles" multiple>
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
                <input class="form-check-input" type="radio" id="flexRadioDefault` + countImg + `" name="mainImageName">
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
    countImg--;
}

function DeleteImg(id, voucherId) {
    Swal.fire({
        title: 'Bạn thực sự muốn xoá?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Huỷ bỏ',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `../Voucher/DeleteImage`,
                type: 'POST',
                data: { ID: id, voucherId: voucherId },
                success: function (rs) {
                    if (rs.success) {
                        Swal.fire({
                            imageUrl: "/images/success.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: rs.message,
                            button: "Đóng",
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire({
                            imageUrl: "/images/fail.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: rs.message,
                            button: "Đóng",
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        imageUrl: "/images/fail.png",
                        imageWidth: 100,
                        imageHeight: 100,
                        title: "Thao tác thất bại",
                        button: "Đóng",
                    });
                }
            });
        }
    });
}

// Handle Submit form
const btnSubmit = $('button[type=submit]')[0];
document.getElementById('Upsert_Form').onsubmit = (e) => {
    e.preventDefault();
    // Khóa nút submit
    btnSubmit.disabled = true;
    // Cập nhật giá trị của CKEditor vào trường textarea
    if (CKEDITOR.instances['LongDescription']) {
        CKEDITOR.instances['LongDescription'].updateElement();
        var editorContent = CKEDITOR.instances['LongDescription'].getData();
        if (editorContent.includes("simple-translate-system-theme")) {
            editorContent = '';
        }
        CKEDITOR.instances['LongDescription'].setData(editorContent);
    }

    var formData = new FormData(e.target);
    formData.set('Description', CKEDITOR.instances['LongDescription'].getData());

    $.ajax({
        url: `../Voucher/UpsertVoucher`,
        type: 'POST',
        data: formData,
        processData: false, // Không chuyển đổi dữ liệu thành chuỗi truy vấn
        contentType: false, // Không đặt Content-Type tự động
        success: (data, status) => {
            if (data.success === true) {
                CustomSweetAlert_Success('Lưu thành công');
                location.reload();
            } else {
                // Mở nút submit
                btnSubmit.disabled = false;
                CustomSweetAlert_Error(data.message);
            }
        },
        error: (xhr, status, error) => {
            // Mở nút submit
            btnSubmit.disabled = false;
            CustomSweetAlert_Error('Có lỗi xảy ra, vui lòng thử lại');
        }
    });
}
//endregion
