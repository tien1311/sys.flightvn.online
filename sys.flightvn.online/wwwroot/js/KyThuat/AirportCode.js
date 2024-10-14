
var count = 1;
function ImportExcel() {
    //Reference the FileUpload element.
    var fileUpload = document.getElementById("files_new");

    //Validate whether File is valid Excel file.
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$/;
    if (regex.test(fileUpload.value.toLowerCase())) {
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();

            //For Browsers other than IE.
            if (reader.readAsBinaryString) {
                reader.onload = function (e) {
                    GetTableFromExcel(e.target.result);
                };
                reader.readAsBinaryString(fileUpload.files[0]);
            } else {
                //For IE Browser.
                reader.onload = function (e) {
                    var data = "";
                    var bytes = new Uint8Array(e.target.result);
                    for (var i = 0; i < bytes.byteLength; i++) {
                        data += String.fromCharCode(bytes[i]);
                    }
                    GetTableFromExcel(data);
                };
                reader.readAsArrayBuffer(fileUpload.files[0]);
            }
        } else {
            alert("This browser does not support HTML5.");
        }
    } else {
        alert("Please upload a valid Excel file.");
    }
};

function GetTableFromExcel(data) {
    //Read the Excel File data in binary
    var workbook = XLSX.read(data, {
        type: 'binary'
    });
    //get the name of First Sheet.
    var Sheet = workbook.SheetNames[0];
    //Read all rows from First Sheet into an JSON array.
    var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[Sheet]);
    //Add the data rows from Excel file.
    for (var i = 0; i < excelRows.length; i++) {
        if (excelRows[i].GiaMua == null || excelRows[i].GiaMua == "") {
            excelRows[i].GiaMua = 0;
        }
        if (excelRows[i].PhiDVMua == null || excelRows[i].PhiDVMua == "") {
            excelRows[i].PhiDVMua = 0;
        }
        if (excelRows[i].PhiDVBan == null || excelRows[i].PhiDVBan == "") {
            excelRows[i].PhiDVBan = 0;
        }
        if (excelRows[i].PhiHoan == null || excelRows[i].PhiHoan == "") {
            excelRows[i].PhiHoan = 0;
        }
        if (excelRows[i].ChietKhau == null || excelRows[i].ChietKhau == "") {
            excelRows[i].ChietKhau = 0;
        }
    

        if (excelRows[i].SoVe == null || excelRows[i].SoVe == "") {

            excelRows[i].SoVe = "";
        }
        else {
            excelRows[i].SoVe = excelRows[i].SoVe.toUpperCase();
        }
       
        if (excelRows[i].MaKH == null || excelRows[i].MaKH == "") {

            excelRows[i].MaKH = "";
        }
        else {
            excelRows[i].MaKH = excelRows[i].MaKH.toUpperCase();
        }
       

        if (excelRows[i].PNR == null || excelRows[i].PNR == "") {

            excelRows[i].PNR = "";
        }
        else {
            excelRows[i].PNR = excelRows[i].PNR.toUpperCase();
        }

        if (excelRows[i].MaHang == null || excelRows[i].MaHang == "") {

            excelRows[i].MaHang = "";
        }
        else {
            excelRows[i].MaHang = excelRows[i].MaHang.toUpperCase();
        }

        if (excelRows[i].GhiChu == null || excelRows[i].GhiChu == "") {

            excelRows[i].GhiChu = "";
        }

        if (excelRows[i].MaGioiThieu == null || excelRows[i].MaGioiThieu == "") {

            excelRows[i].MaGioiThieu = "";
        }

        if (excelRows[i].NguoiGioiThieu == null || excelRows[i].NguoiGioiThieu == "") {

            excelRows[i].NguoiGioiThieu = "";
        }
       
        if (i == 0) {
            document.getElementById("SoVe").value = excelRows[i].SoVe;
            document.getElementById("MAHHK").value = excelRows[i].MaHang;
            document.getElementById("GiaMua").value = excelRows[i].GiaMua;
            document.getElementById("PhiDVMua").value = excelRows[i].PhiDVMua;
            document.getElementById("PhiDVBan").value = excelRows[i].PhiDVBan;
            document.getElementById("PhiHoan").value = excelRows[i].PhiHoan;
            document.getElementById("ChietKhau").value = excelRows[i].ChietKhau;
            document.getElementById("MAKH_NEW").value = excelRows[i].MaKH;
            document.getElementById("PNR_NEW").value = excelRows[i].PNR;
            document.getElementById("GhiChu").value = excelRows[i].GhiChu;
            document.getElementById("MaGioiThieu").value = excelRows[i].MaGioiThieu;
            document.getElementById("NguoiGioiThieu").value = excelRows[i].NguoiGioiThieu;
        }
        else {
            var checkTrung = 0;
            for (var y = 0; y < i; y++) {
                if (excelRows[y].SoVe === excelRows[i].SoVe && excelRows[y].PNR === excelRows[i].PNR && excelRows[y].MaHang === excelRows[i].MaHang) {
                    alert("Số vé " + excelRows[y].SoVe + " có trên 1 dòng");
                    checkTrung++;
                    break;
                }
            }
            if (checkTrung == 0) {
                document.querySelector('#addRows').insertAdjacentHTML(
                    'beforebegin',
                    `     <div class="row" id="RowTenKH` + count + `">
                                    <div style="margin-top: 10px;margin-left: 10px;">
                                        <div class="item form-group" style="margin-bottom: 0px;">
                                             <span style="font-size: 12px;color: #3300ff">
                                                <label id="TENKH_NEW` + count + `" ></label>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                    <div class="row" id="SoDong` + count + `" >
                                     <div class="col-md-1 col-xs-4">
                                            <div class="item form-group">                                           
                                                    <input type="text" placeholder="Hãng" value="` + excelRows[i].MaHang + `" id="MAHHK` + count + `" asp-for="MAHHK` + count + `" name="MAHHK` + count + `" class="form-control ">
                                            </div>
                                        </div>
                         <div class="col-md-2 col-xs-8">
                                            <div class="item form-group">                                              
                                                <div class="input-group" style="margin:0px">
                                                    <input type="text" placeholder="Mã KH" value="` + excelRows[i].MaKH + `" id="MAKH_NEW` + count + `" asp-for="MAKH_NEW` + count + `" name="MAKH_NEW` + count + `" class="form-control ">
                                                    <span class="input-group-btn">                                                      
                                                        <button id="check" onclick="CheckMaKH('MAKH_NEW` + count + `','TENKH_NEW` + count + `');" class="btn btn-info" type="button" style="margin-bottom:0px; "><i class="fa fa-search" aria-hidden="true"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                               
                                            <div class="col-md-1 col-xs-4">
                                                <div class="item form-group">
                                                    <input type="text" placeholder="PNR" value="` + excelRows[i].PNR + `" id="PNR_NEW` + count + `" asp-for="PNR_NEW` + count + `" name="PNR_NEW` + count + `" class="form-control ">
                                                </div>
                                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">                            
                                        <input type="text" placeholder="Số vé" id="SoVe` + count + `" value="` + excelRows[i].SoVe + `" asp-for="SoVe` + count + `" name="SoVe` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">                               
                                        <input type="text" placeholder="Giá mua" value="` + excelRows[i].GiaMua + `" onkeyup="formatNumber(document.getElementById(this.id).value,this.id);" id="GiaMua` + count + `" asp-for="GiaMua` + count + `" name="GiaMua` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4" hidden>
                                <div class="item form-group">                              
                                        <input type="text" placeholder="Phí dịch vụ mua" value="` + excelRows[i].PhiDVMua + `" onkeyup="formatNumber(document.getElementById(this.id).value,this.id);" id="PhiDVMua` + count + `" asp-for="PhiDVMua` + count + `" name="PhiDVMua` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">                                
                                        <input type="text" placeholder="Phí dịch vụ bán" value="` + excelRows[i].PhiDVBan + `" onkeyup="formatNumber(document.getElementById(this.id).value,this.id);" id="PhiDVBan` + count + `" asp-for="PhiDVBan` + count + `" name="PhiDVBan` + count + `" class="form-control ">
                                </div>
                            </div>
                             <div class="col-md-2 col-xs-4">
                                            <div class="item form-group">                                          
                                                    <input type="text" placeholder="Phí hoàn" value="` + excelRows[i].PhiHoan + `" onkeyup="formatNumber(document.getElementById(this.id).value,this.id);" id="PhiHoan` + count + `" asp-for="PhiHoan` + count + `" name="PhiHoan` + count + `" class="form-control ">
                                            </div>
                                        </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Chiết khấu" value="` + excelRows[i].ChietKhau + `" onkeyup="formatNumber(document.getElementById(this.id).value,this.id);" id="ChietKhau` + count + `" asp-for="ChietKhau` + count + `" name="ChietKhau` + count + `" class="form-control ">
                                </div>
                            </div>
                         <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Ghi chú" value="` + excelRows[i].GhiChu + `"  id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" class="form-control ">
                                </div>
                            </div>
                          <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Mã giới thiệu" value="` + excelRows[i].MaGioiThieu + `"  id="MaGioiThieu` + count + `" asp-for="MaGioiThieu` + count + `" name="MaGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                          <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Người giới thiệu" value="` + excelRows[i].NguoiGioiThieu + `"  id="NguoiGioiThieu` + count + `" asp-for="NguoiGioiThieu` + count + `" name="NguoiGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-1 col-xs-6">
                                <div class="item form-group">                               
                                        <input class="btn btn-danger" onclick="XoaDong(` + count + `)"  type="button" value="-" />
                                </div>
                            </div>
                        </div>` );
                count++;
            }
        }  
    }
};

function CreateELement(arr) {
    var Details = document.getElementById("Details");
    var div_detail_root = document.createElement("div");
    div_detail_root.id = "Detail_" + count;
    div_detail_root.className = "row";
    arr.map((item) => {
        //Add LocaleId
        var div_Element_parent = document.createElement("div");
        if (item == "button") {
            div_Element_parent.classList = "col-md-1 col-xs-6";
        }
        else {
            if (item == "Description") {
                div_Element_parent.className = "col-sm-3";
            }
            else {
                div_Element_parent.className = "col-sm-2";
            }
        }

    
        var div_Element_child = document.createElement("div");
        div_Element_child.classList = "item form-group";

        var input_element = document.createElement("input");
        if (item == "button") {
            input_element.type = "button";
            input_element.classList = "btn btn-primary";
            input_element.value = "-";
            input_element.style = "background-color:red";
            const index = count;
            input_element.onclick = function () {
                XoaDong(index);
            };
        }
        else {
            input_element.id = item;
            input_element.name = item;
            input_element.placeholder = item;
            input_element.className = "form-control";
        }
       
        div_Element_child.appendChild(input_element);
        div_Element_parent.appendChild(div_Element_child);
        div_detail_root.appendChild(div_Element_parent);
    })
    Details.appendChild(div_detail_root);
}
function ThemDong() {
    const ArrayData = ["LocaleId" + count + "", "AirportName" + count + "", "Description" + count + "", "CityName" + count + "", "CountryName" + count +"", "button"];
    CreateELement(ArrayData);
    count++;
}

function XoaDong(index) {
    var t = 'Detail_' + index;
    const element1 = document.getElementById(t);
    element1.remove();
}

function formatNumber(number, id) {
    number = number.replaceAll(",", "");
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    document.getElementById(id).value = x1 + x2;

    return x1 + x2;
}

function FormatPhiDichVu() {
    var TongphiDV = document.getElementById("PHIDICHVU").value.replace(/\$|\,/g, '');
    document.getElementById("PHIDICHVU").value = formatNumber(Number(TongphiDV));
}


if (document.getElementById("Save")) {
    var ButtonSave = document.getElementById("Save");
    ButtonSave.addEventListener("click", function (event) {
      
        var airportCode = document.getElementById("AirportCode").value;
        var latitude = document.getElementById("Latitude").value;
        var longitude = document.getElementById("Longitude").value;
        var timeZoneOffset = document.getElementById("TimeZoneOffset").value;
        var iataCode = document.getElementById("IataCode").value;
        var cityCode = document.getElementById("CityCode").value;
        var countryCode = document.getElementById("CountryCode").value;
        var regionCode = document.getElementById("RegionCode").value;
        var description = document.getElementById("Description").value;
        var request = {};
        request.AirportCode = airportCode;
        request.Latitude = latitude;
        request.Longitude = longitude;
        request.TimeZoneOffset = timeZoneOffset;
        request.IataCode = iataCode;
        request.CityCode = cityCode;
        request.CountryCode = countryCode;
        request.RegionCode = regionCode
        request.Description = description;
        request.Profiles = [];
        var Profiles = [];

        for (var i = 0; i < count; i++) {
            if (document.getElementById("AirportName" + i)) {
                var Profile = {};
                Profile.LocaleId = document.getElementById("LocaleId" + i).value;
                Profile.AirportName = document.getElementById("AirportName" + i).value;
                Profile.Description = document.getElementById("Description" + i).value;
                Profile.CityName = document.getElementById("CityName" + i).value;
                Profile.CountryName = document.getElementById("CountryName" + i).value;
                Profiles.push(Profile);
            }
            console.log(Profile);
        }

        request.Profiles = Profiles;

        Swal.fire({
            html: '<img src="/images/loading.gif" >',
            showCancelButton: false,
            showConfirmButton: false,
            allowOutsideClick: false,
            background: 'transparent',
            onBeforeOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            type: "POST",
            url: "../KyThuat/SaveCreateAirportCode",
            //processData: false,
            //cache: false,
            //async: false,
            data: { model: request },
            dataType: "json",
            beforeSend: function () {
                //show loading
                //$("#loader").show();
            },
            complete: function () {
                //hide loading
                //$("#loader").hide();
            },
            success: function (resultData) {
                Swal.close();
                console.log(resultData);
                if (resultData.message == "Success") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Your work has been saved',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    location.reload(true);
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Your work has been saved',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
                //$("#loader").hide();
            },
            error: function (xhr, status, p3, p4) {
                //$("#loader").hide();
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText)
                    err = xhr.responseText;
                alert(err);
            }
        })
    })
}
if (document.getElementById("Update")) {
   
    var ButtonUpdate = document.getElementById("Update");
    ButtonUpdate.addEventListener("click", function (event) {
       
        var airportCode = document.getElementById("AirportCode").value;
        var latitude = document.getElementById("Latitude").value;
        var longitude = document.getElementById("Longitude").value;
      
        var timeZoneOffset = document.getElementById("TimeZoneOffset").value;
        var iataCode = document.getElementById("IataCode").value;
        var cityCode = document.getElementById("CityCode").value;
        var countryCode = document.getElementById("CountryCode").value;
        var regionCode = document.getElementById("RegionCode").value;
        var description = document.getElementById("Description").value;
        var request = {};
        request.AirportCode = airportCode;
        request.Latitude = latitude;
        request.Longitude = longitude;
        request.TimeZoneOffset = timeZoneOffset;
        request.IataCode = iataCode;
        request.CityCode = cityCode;
        request.CountryCode = countryCode;
        request.RegionCode = regionCode
        request.Description = description;
        request.Profiles = [];
        var Profiles = [];

        var Details = document.getElementById("Details");

        var div_row = Details.getElementsByClassName("row");
     
      
        for (var i = 0; i < div_row.length; i++) {
            if (document.getElementById("AirportName" + i)) {
                var Profile = {};
                Profile.LocaleId = document.getElementById("LocaleId" + i).value;
                Profile.AirportName = document.getElementById("AirportName" + i).value;
                Profile.Description = document.getElementById("Description" + i).value;
                Profile.CityName = document.getElementById("CityName" + i).value;
                Profile.CountryName = document.getElementById("CountryName" + i).value;
                console.log(Profile);
                Profiles.push(Profile);
            }
            console.log(Profiles);
        }

        request.Profiles = Profiles;

        Swal.fire({
            html: '<img src="/images/loading.gif" >',
            showCancelButton: false,
            showConfirmButton: false,
            allowOutsideClick: false,
            background: 'transparent',
            onBeforeOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            type: "POST",
            url: "../KyThuat/SaveEditAirportCode",
            //processData: false,
            //cache: false,
            //async: false,
            data: { model: request },
            dataType: "json",
            beforeSend: function () {
                //show loading
                //$("#loader").show();
            },
            complete: function () {
                //hide loading
                //$("#loader").hide();
            },
            success: function (resultData) {
                Swal.close();
                console.log(resultData);
                if (resultData.message == "Success") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Sửa thông tin thành công',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    location.reload(true);
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Sửa thông tin thất bại',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
                //$("#loader").hide();
            },
            error: function (xhr, status, p3, p4) {
                //$("#loader").hide();
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText)
                    err = xhr.responseText;
                alert(err);
            }
        })
    })
}







