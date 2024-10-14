var countImg = 1;
const CreateOrEdit = document.getElementById('TypeSave').value;
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: "btn btn-outline-primary btn-block",
        cancelButton: "btn btn-danger"
    },
    buttonsStyling: false
});
// Prevent User Input negative Value
function PreventInputNegativeValue(e) {
    if (!((e.keyCode > 95 && e.keyCode < 106)
        || (e.keyCode > 47 && e.keyCode < 58)
        || e.keyCode == 8)) {
        return false;
    }
}

let selectListServiceId = document.getElementById('ListServiceId')
let valueType = document.getElementById('TypeService').value;
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
function CustomError(message) {
    swalWithBootstrapButtons.fire({
        imageUrl: "/images/fail.png",
        imageWidth: 100,
        imageHeight: 100,
        title: message,
        confirmButtonText: 'Đóng',
    });
}

function CustomSuccess(message) {
    swalWithBootstrapButtons.fire({
        imageUrl: "/images/success.png",
        imageWidth: 100,
        imageHeight: 100,
        title: message,
        confirmButtonText: 'Đóng',
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
        `<div class="row  contain-listImg" id="RowImg` + countImg + `" data-number="${countImg}">
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
    countImg--;
}

function handleMainImage(formData) {
    let isValid = true;
    //Danh sach hình có sẵn
    var countlistEditImage = document.querySelectorAll('.contain-DetailImg').length;
    var counMainImage = 0;
    if (countlistEditImage != 0) {
        for (var i = 1; i <= countlistEditImage; i++) {
            var imgs = {};
            var imagesURL = "ImageURL" + i;
            var mainImgs = "MainImg" + i;
            imgs.imagesURL = document.getElementById(imagesURL).value;
            imgs.mainImgs = document.getElementById(mainImgs).checked;
            if (document.getElementById(mainImgs).checked == true) {
                counMainImage++;
            }
            formData.append('mainImgs', imgs.mainImgs);
            formData.append('imagesURL', imgs.imagesURL);
        }
    }

    // Danh sách hình mới thêm
    var listAddingImage = document.querySelectorAll('.contain-listImg');
    Array.from(listAddingImage).forEach((item, index) => {
        let datasetNumber = item.dataset.number
        let detailImage = `file${datasetNumber}`;
        let mainImg = `flexRadioDefault${datasetNumber}`;
        let imageFile = document.getElementById(detailImage).files[0];
        let isMainImage = document.getElementById(mainImg).checked;
        if (isMainImage == true) {
            counMainImage++;
        }
        if (countlistEditImage == 0) {
            if (!detailImage || !mainImg) {
                isValid = false;
            }
            else {
                isValid = true;;
            }
        } else {
            if (index >= 1) {
                if (!imageFile) {
                    isValid = false;
                    return isValid;
                }
            }
            if (isMainImage) {
                if (!imageFile) {
                    isValid = false;
                    return isValid;
                }
            }
        }
        if (counMainImage <= 0) {
            isValid = false;
        }

        formData.append('mainImages', isMainImage);
        formData.append('imageFiles', imageFile);
    })
    return isValid;
}


function HandleCompareDates(d1, d2) {
    let date1 = new Date(d1).getTime();
    let date2 = new Date(d2).getTime();
    let isValid = true;
    if (date1 > date2) {
        isValid = false;
    }
    return isValid;
};
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
                url: "../Voucher/DeleteImage",
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

function handleErrors(formValues, validImage) {
    let isValid = true;
    const errorMessages = {
        Id: '',
        CreateDate: '',
        CreateBy: '',
        VoucherName: 'Bạn phải thêm tên Voucher.',
        ServiceId: 'Bạn vui lòng chọn dịch vụ.',
        ShortDescription: 'Bạn phải thêm mô tả ngắn.',
        Description: 'Bạn phải thêm mô tả chi tiết.',
        Price: 'Bạn phải thêm giá bán voucher.',
        DiscountPrice: 'Bạn phải thêm giá giảm voucher.',
        ExpiryDateFrom: 'Bạn phải thêm hạn sử dụng của voucher.',
        ExpiryDateTo: 'Bạn phải thêm hạn sử dụng của voucher.'
        //Type: 'Bạn vui lòng chọn loại dịch vụ.',
        //PriceMinRequired: '',
        //DiscountAmountWhenUse: 'Bạn phải thêm mức độ giảm khi sử dụng voucher.',
    };

    for (const [key, message] of Object.entries(errorMessages)) {
        if (key == 'Id' || key == 'CreateDate' || key == 'CreateBy') {
            // Hidden value
            continue;
        } else {
            if (!formValues[key]) {
                // Có key truyền vào form nhưng gia trị bị null -> lỗi
                isValid = false;
                CustomError(message);
                return isValid;
            } else {
                // Check Negative Value
                if (Number(formValues['Price']) < 0 || Number(formValues['DiscountPrice']) < 0) {
                    isValid = false;
                    CustomError("Mức giá không thể âm.")
                    return isValid;
                }
                if (Number(formValues['Price']) == 0) {
                    isValid = false;
                    CustomError("Vui lòng nhập giá bán.")
                    return isValid;
                }
                if (Number(formValues['DiscountPrice']) == 0) {
                    isValid = false;
                    CustomError("Vui lòng nhập giá giảm.")
                    return isValid;
                }
                //if (formValues['VoucherTypeID'] == 1) {
                //    // Giảm theo % -> check DiscountAmountWhenUse
                //    if (formValues['DiscountAmountWhenUse'] > 100) {
                //        isValid = false;
                //        CustomError("Mức độ giảm giá không thể lớn hơn 100%.");
                //        return isValid;
                //    }
                //}
                // Check Giá bán < giá giảm -> Error.
                if (Number(formValues['Price']) <= Number(formValues['DiscountPrice'])) {
                    isValid = false;
                    CustomError("Giá giảm lớn hơn hoặc bằng giá bán.")
                    return isValid;
                }
                // Kiểm tra độ dài của mô tả
                if (formValues['ShortDescription'].length > 255) {
                    isValid = false;
                    CustomError("Mô tả ngắn không thể dài hơn 255 ký tự");
                    return isValid;
                }
            }
        }
    }
    let isValidCompare = HandleCompareDates(formValues.ExpiryDateFrom, formValues.ExpiryDateTo)
    if (isValidCompare == false) {
        isValid = false;
        CustomError("Ngày hết hạn không thể lớn hơn ngày bắt đầu")
        return
    }
    // Check main Image
    if (validImage == false) {
        isValid = false;
        CustomError("Bạn chưa chọn hình.")
        return;
    }
    return isValid;
}

function Save() {
    const formData = new FormData();
    const formFields = [
        { id: 'Id', name: 'Id' },
        { id: 'CreateDate', name: 'CreateDate' },
        { id: 'CreateBy', name: 'CreateBy' },
        { id: 'TypeService', name: 'Type' },
        { id: 'ListServiceId', name: 'ServiceId' },
        { id: 'NameVoucher', name: 'VoucherName' },
        { id: 'ShortDescription', name: 'ShortDescription' },
        { id: 'LongDescription', name: 'Description' },
        { id: 'ExpiryDateFrom', name: 'ExpiryDateFrom' },
        { id: 'ExpiryDateTo', name: 'ExpiryDateTo' },
        { id: 'Price', name: 'Price' },
        { id: 'DiscountPrice', name: 'DiscountPrice' }
        //{ id: 'VoucherType', name: 'VoucherTypeID' },
        //{ id: 'PriceDiscountWhenUse', name: 'DiscountAmountWhenUse' },
        //{ id: 'PriceMinRequired', name: 'PriceMinRequired' },
    ];

    // Lấy giá trị 
    let formValues = {};
    formFields.forEach(field => {
        var value;
        if (CreateOrEdit == 'CREATE') {
            if (field.name != 'Id' && field.name != 'CreateDate' && field.name != 'CreateBy') {
                value = document.getElementById(field.id).value
                formValues[field.name] = value;
            }
        } else {
            value = document.getElementById(field.id).value
            formValues[field.name] = value;
        }
        // Lấy nội dung từ CKEDITOR
        if (field.name == 'Description') {
            value = CKEDITOR.instances['LongDescription'].getData();
            formValues[field.name] = value;
        }
        if (field.name == 'Price' || field.name == 'DiscountPrice') {
            value = value.replace(/,/g, '')
            formValues[field.name] = value;
        }
        // Set price Min = 0 khi bị bỏ trống
        //if (field.name == 'PriceMinRequired') {
        //    value = document.getElementById(field.id).value
        //    if (value === '') {
        //        value = '0';
        //        formValues[field.name] = value;
        //    }
        //}
    });
    // HandleMainImage
    let validImage = handleMainImage(formData);

    // Handle Error
    let isValid = handleErrors(formValues, validImage);
    if (isValid == true) {
        // Gắn dữ liệu từ formValues vào formData
        for (const [key, value] of Object.entries(formValues)) {
            formData.append(key, value);
        }
        let myUrl;
        if (CreateOrEdit == 'CREATE') {
            myUrl = "/Voucher/CreateVoucher";
        } else {
            myUrl = "/Voucher/EditVoucher";
        }
        $.ajax({
            type: "POST",
            url: myUrl,
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success == true) {
                    CustomSuccess('Lưu thành công');
                    location.reload();
                } else {
                    CustomError("Lưu không thành công.")
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
}
//endregion
