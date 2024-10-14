// init Chosen for Province
$(".chosen-select-tourlocation").chosen({ width: '100%' });

// Handle get District
let provinceElement = document.getElementById('province')
let selectWardElement = document.getElementById('GetDistricts');

if (provinceElement.value != '') {
    $.get(
        '../TourLocation/GetDistricts',
        { provinceCode: provinceElement.value },
        (data, status) => {
            let datasetNumber = selectWardElement.dataset.codedistrict;
            let optionstring = data.map((value, index) => {
                let output = `<option value="${value.code}">${value.full_Name}</option>`;
                if (value.code == datasetNumber) {
                    output = `<option selected value="${value.code}">${value.full_Name}</option>`;
                }
                return output;
            }).join("");
            selectWardElement.innerHTML = `
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="ward" class="control-label">Quận/Huyện</label>
                                    <select class="form-control chosen-select-tourlocation" data-placeholder="Chọn Quận/Huyện" id="district" name="district">
                                <option selected disabled value="">-- Chọn Quận/Huyện --</option>
                                    ${optionstring}
                            </select>
                        </div>
                    </div>`;
            // init Chosen for Province and District
            $(".chosen-select-tourlocation").chosen({ width: '100%' });
        }
    )
}
provinceElement.onchange = (e) => {
    if (e.target.value != '') {
        $.get(
            '../TourLocation/GetDistricts',
            { provinceCode: e.target.value },
            (data, status) => {
                let optionstring = data.map((value, index) => {
                    return `<option value="${value.code}">${value.full_Name}</option>`
                }).join("");
                selectWardElement.innerHTML = `
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label for="ward" class="control-label">Quận/Huyện</label>
                                    <select class="form-control chosen-select-tourlocation" data-placeholder="Chọn Quận/Huyện" id="district" name="district">
                                <option selected disabled value="">-- Chọn Quận/Huyện --</option>
                                    ${optionstring}
                            </select>
                        </div>
                    </div>`;
                // init Chosen for Province and District
                $(".chosen-select-tourlocation").chosen({ width: '100%' });
            }
        )
    }
}

function Save() {
    let id = !document.getElementById('Id') ? 0 : document.getElementById('Id').value;
    var name = document.getElementById('Name').value
    var province = document.getElementById('province').value
    var district = document.getElementById('district')
    var email = document.getElementById('Email').value
    var phone = document.getElementById('Phone').value
    if (!district) {
        district = '';
    } else {
        district = district.value
    }

    $.post(
        '../TourLocation/UpsertTourLocation',
        {
            Id : id,
            Name: name,
            Province: province,
            District: district,
            Email: email,
            Phone: phone
        },
        (data) => {
            if (data.success) {
                location.reload();
                //Swal.fire({
                //    imageUrl: "/images/success.png",
                //    imageWidth: 100,
                //    imageHeight: 100,
                //    title: data.message,
                //    button: "Đóng",
                //})
                CustomSweetAlert_Success(`${data.message}`)

            } else {
                //Swal.fire({
                //    imageUrl: "/images/fail.png",
                //    imageWidth: 100,
                //    imageHeight: 100,
                //    title: data.message,
                //    button: "Đóng",
                //});
                CustomSweetAlert_Error(`${data.message}`)
            }
        }
    )
}