function myFunction() {

    document.getElementById("NOIDUNG").value = $("#editor-one").html();
    document.getElementById("DIEUKIEN").value = $("#editor-two").html();
}
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
    console.log(excelRows);
    for (var i = 0; i < excelRows.length; i++) {
        if (excelRows[i].PNR != "" && excelRows[i].PNR != null)  {

        if (excelRows[i].GiaMua == null || excelRows[i].GiaMua == "") {
            excelRows[i].GiaMua = 0;
        }
        if (excelRows[i].PhiDVMua == null || excelRows[i].PhiDVMua == "") {
            excelRows[i].PhiDVMua = 0;
        }
        if (excelRows[i].LoaiPhi == null || excelRows[i].LoaiPhi == "") {
            excelRows[i].LoaiPhi = "";
        }
        if (excelRows[i].PhiXuatVe == null || excelRows[i].PhiXuatVe == "") {
            excelRows[i].PhiXuatVe = 0;
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
        if (excelRows[i].SoLuong == null || excelRows[i].SoLuong == "") {
                excelRows[i].SoLuong = 1;
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

            var sourceSelect0 = document.getElementById("KindName");
            // Clear existing options in the destination select element

            //var Selected = sourceSelect0.options[sourceSelect.selectedIndex].textContent;
            // Iterate over options in the source select element and append clones to the destination
          
            for (var z = 0; z < sourceSelect0.options.length; z++) {
                var sourceOption0 = sourceSelect0.options[z];
                if (sourceOption0.textContent.toUpperCase().trim() == excelRows[i].LoaiPhi.toUpperCase().trim()) {
                    sourceOption0.selected = true;
                 
                    if (excelRows[i].LoaiPhi.toUpperCase().trim() != "WEBBSP" && excelRows[i].LoaiPhi.toUpperCase().trim() != "HOAHONG") {
                        document.getElementById("PhiXuatVe").disabled = true;
                        document.getElementById("PhiXuatVe").value = sourceOption0.value;
                    }
                    else {
                        document.getElementById("PhiXuatVe").disabled = false;
                        document.getElementById("PhiXuatVe").value = excelRows[i].PhiXuatVe;
                    }
                }
            }
        
            document.getElementById("PhiDVBan").value = excelRows[i].PhiDVBan;
            document.getElementById("PhiHoan").value = excelRows[i].PhiHoan;
            document.getElementById("ChietKhau").value = excelRows[i].ChietKhau;
            document.getElementById("MAKH_NEW").value = excelRows[i].MaKH;
            document.getElementById("PNR_NEW").value = excelRows[i].PNR;
            document.getElementById("GhiChu").value = excelRows[i].GhiChu;
            document.getElementById("MaGioiThieu").value = excelRows[i].MaGioiThieu;
            document.getElementById("NguoiGioiThieu").value = excelRows[i].NguoiGioiThieu;
            document.getElementById("SoLuong").value = excelRows[i].SoLuong;
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
                                <div class="col-md-1 col-xs-4">
                            <div class="item form-group">
                                <input type="text" placeholder="Số lượng" onkeyup="ShowPhiXuatVe('PhiXuatVe` + count + `', 'KindName` + count + `', 'SoLuong` + count + `')" id="SoLuong` + count + `" asp-for="SoLuong" name="SoLuong` + count + `" value="` + excelRows[i].SoLuong + `" class="form-control ">
                            </div>
                        </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">                               
                                        <input type="text" placeholder="Giá mua" value="` + excelRows[i].GiaMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="GiaMua` + count + `" asp-for="GiaMua` + count + `" name="GiaMua` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4" hidden>
                                <div class="item form-group">                              
                                        <input type="text" placeholder="Phí dịch vụ mua" value="` + excelRows[i].PhiDVMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVMua` + count + `" asp-for="PhiDVMua` + count + `" name="PhiDVMua` + count + `" class="form-control ">
                                </div>
                            </div>
                             <div class="col-md-1 col-xs-4">
                                    <select onchange="ShowPhiXuatVe('PhiXuatVe` + count + `', this.id, 'SoLuong` + count + `')" id="KindName` + count + `"  class="form-control"> 
                                        
                                    </select>
                            </div>
                           <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí xuất vé" disabled value="` + excelRows[i].PhiXuatVe + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiXuatVe` + count + `" asp-for="PhiXuatVe` + count + `" name="PhiXuatVe` + count + `" class="form-control ">
                            </div>
                        </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">                                
                                        <input type="text" placeholder="Phí dịch vụ bán" value="` + excelRows[i].PhiDVBan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVBan` + count + `" asp-for="PhiDVBan` + count + `" name="PhiDVBan` + count + `" class="form-control ">
                                </div>
                            </div>
                             <div class="col-md-2 col-xs-4">
                                            <div class="item form-group">                                          
                                                    <input type="text" placeholder="Phí hoàn" value="` + excelRows[i].PhiHoan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiHoan` + count + `" asp-for="PhiHoan` + count + `" name="PhiHoan` + count + `" class="form-control ">
                                            </div>
                                        </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Chiết khấu" value="` + excelRows[i].ChietKhau + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="ChietKhau` + count + `" asp-for="ChietKhau` + count + `" name="ChietKhau` + count + `" class="form-control ">
                                </div>
                            </div>
                         <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Ghi chú" value="` + excelRows[i].GhiChu + `"  id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" class="form-control ">
                                </div>
                            </div>
                          <div class="col-md-1 col-xs-4">
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

                var sourceSelect = document.getElementById("KindName");
                var KindNameID = `KindName` + count + ``;
                var destinationSelect = document.getElementById(KindNameID);
                // Clear existing options in the destination select element
                destinationSelect.innerHTML = '';

                var Selected = excelRows[i].LoaiPhi.toUpperCase().trim();
                console.log("Select" + Selected);
                // Iterate over options in the source select element and append clones to the destination
                for (var k = 0; k < sourceSelect.options.length; k++) {
                    var sourceOption = sourceSelect.options[k];
                    var clonedOption = sourceOption.cloneNode(true);
                    if (sourceOption.textContent.toUpperCase().trim() == Selected) {
                        console.log("File dữ liệu" + sourceOption.textContent);
                        clonedOption.selected = true;

                        //var PhiXuatVeID = `PhiXuatVe` + count + ``;
                        //if (Selected.toUpperCase().trim() == "WEBBSP") {
                        //    document.getElementById(PhiXuatVeID).disabled = false;
                        //    clonedOption.disabled = true;
                        //}
                        //else {
                        //    document.getElementById(PhiXuatVeID).disabled = true;
                        //    clonedOption.disabled = true;
                        //}
                        var PhiXuatVeID = `PhiXuatVe` + count + ``;
                        if (excelRows[i].LoaiPhi.toUpperCase().trim() != "WEBBSP" && excelRows[i].LoaiPhi.toUpperCase().trim() != "HOAHONG") {
                            document.getElementById(PhiXuatVeID).disabled = true;
                            if (sourceOption.value == -1) {
                                sourceOption.value = 0;
                            }
                            document.getElementById(PhiXuatVeID).value = sourceOption.value;
                            
                            //clonedOption.value = sourceOption.value;
                        }
                        else {
                            document.getElementById(PhiXuatVeID).disabled = false;
                            document.getElementById(PhiXuatVeID).value = excelRows[i].PhiXuatVe;
                        }

                        //if (excelRows[i].LoaiPhi.toUpperCase().trim() != "WEBBSP") {
                        //    clonedOption.disabled = true;
                        //    clonedOption.value = sourceOption.value;
                        //}
                        //else {
                        //    clonedOption.disabled = false;
                        //    clonedOption.value = excelRows[i].PhiXuatVe;
                        //}

                    }
                 
                    destinationSelect.appendChild(clonedOption);
                }

                count++;
            }
        }  
        }
    }
};


function ThemDong() {
    var SoVe = document.getElementById("SoVe").value;
    var MAHHK = document.getElementById("MAHHK").value;
    var GiaMua = document.getElementById("GiaMua").value;
    var PhiDVMua = document.getElementById("PhiDVMua").value;
    var KindName = document.getElementById("KindName").value;
    var PhiXuatVe = document.getElementById("PhiXuatVe").value;
    var PhiDVBan = document.getElementById("PhiDVBan").value;
    var PhiHoan = document.getElementById("PhiHoan").value;
    var ChietKhau = document.getElementById("ChietKhau").value;
    var MAKH = document.getElementById("MAKH_NEW").value;
    var PNR = document.getElementById("PNR_NEW").value;
    var GhiChu = document.getElementById("GhiChu").value;
    var MaGioiThieu = document.getElementById("MaGioiThieu").value;
    var NguoiGioiThieu = document.getElementById("NguoiGioiThieu").value;
    var SoLuong = document.getElementById("SoLuong").value;

    if (GiaMua == null || GiaMua == "") {
        GiaMua = 0;
    }
    if (PhiDVMua == null || PhiDVMua == "") {
        PhiDVMua = 0;
    }
    if (PhiDVBan == null || PhiDVBan == "") {
        PhiDVBan = 0;
    }
    if (PhiHoan == null || PhiHoan == "") {
        PhiHoan = 0;
    }
    if (ChietKhau == null || ChietKhau == "") {
        ChietKhau = 0;
    }
    SoVe = SoVe.toUpperCase();
    MAKH = MAKH.toUpperCase();
    PNR = PNR.toUpperCase();
    MAHHK = MAHHK.toUpperCase();

    if (!isNaN(SoVe)) {
        if (count == 1) {
            var n = document.getElementById("SoVe").value.charAt(0);
            if (n == "0") {
                SoVe = Number(document.getElementById("SoVe").value) + count;
                SoVe = "0" + SoVe;
            }
            else {
                SoVe = Number(document.getElementById("SoVe").value) + count;
            } 
        }
        else {
            var IDSoVe = "SoVe" + (count - 1);
            var n = document.getElementById(IDSoVe).value.charAt(0);
            if (n == "0") {
                SoVe = Number(document.getElementById(IDSoVe).value) + 1;
                SoVe = "0" + SoVe;
            }
            else {
                SoVe = Number(document.getElementById(IDSoVe).value) + 1;
            }
        }
        document.querySelector('#addRows').insertAdjacentHTML(
            'beforebegin',
            ` <div class="row" id="RowTenKH` + count + `">
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
                                                <input type="text" placeholder="Hãng" value="` + MAHHK + `" id="MAHHK` + count + `" asp-for="MAHHK` + count + `" name="MAHHK` + count + `" class="form-control ">
                                        </div>
                                    </div>

                             <div class="col-md-2 col-xs-8">
                                            <div class="item form-group">
                                                <div class="input-group" style="margin:0px">
                                                    <input type="text" placeholder="Mã KH" value="` + MAKH + `" id="MAKH_NEW` + count + `" asp-for="MAKH_NEW` + count + `" name="MAKH_NEW` + count + `" class="form-control ">
                                                    <span class="input-group-btn">
                                                        <button id="check" onclick="CheckMaKH('MAKH_NEW` + count + `','TENKH_NEW` + count + `');" class="btn btn-info" type="button" style="margin-bottom:0px; "><i class="fa fa-search" aria-hidden="true"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                           
                                        <div class="col-md-1 col-xs-4">
                                            <div class="item form-group">
                                                <input type="text" placeholder="PNR" value="` + PNR + `" id="PNR_NEW` + count + `" asp-for="PNR_NEW` + count + `" name="PNR_NEW` + count + `" class="form-control ">
                                            </div>
                                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Số vé" id="SoVe` + count + `" value="` + SoVe + `" asp-for="SoVe` + count + `" name="SoVe` + count + `" class="form-control ">
                            </div>
                        </div>
                          <div class="col-md-1 col-xs-4">
                            <div class="item form-group">
                                <input type="text" placeholder="Số lượng" onkeyup="ShowPhiXuatVe('PhiXuatVe` + count + `', 'KindName` + count + `', 'SoLuong` + count + `')" id="SoLuong` + count + `" asp-for="SoLuong" name="SoLuong` + count + `" value="` + SoLuong + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Giá mua" value="` + GiaMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="GiaMua` + count + `" asp-for="GiaMua` + count + `" name="GiaMua` + count + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4" hidden>
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí dịch vụ mua" value="` + PhiDVMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVMua` + count + `" asp-for="PhiDVMua` + count + `" name="PhiDVMua` + count + `" class="form-control ">
                            </div>
                        </div>
                         <div class="col-md-1 col-xs-4">
                                <select onchange="ShowPhiXuatVe('PhiXuatVe` + count + `', this.id, 'SoLuong` + count + `')"  id="KindName` + count + `"  class="form-control"> 
                                   
                                </select>
                        </div>
                           <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí xuất vé" disabled value="` + PhiXuatVe + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiXuatVe` + count + `" asp-for="PhiXuatVe` + count + `" name="PhiXuatVe` + count + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí dịch vụ bán" value="` + PhiDVBan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVBan` + count + `" asp-for="PhiDVBan` + count + `" name="PhiDVBan` + count + `" class="form-control ">
                            </div>
                        </div>
                         <div class="col-md-2 col-xs-4">
                                        <div class="item form-group">
                                                <input type="text" placeholder="Phí hoàn" value="` + PhiHoan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiHoan` + count + `" asp-for="PhiHoan` + count + `" name="PhiHoan` + count + `" class="form-control ">
                                        </div>
                                    </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Chiết khấu" value="` + ChietKhau + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="ChietKhau` + count + `" asp-for="ChietKhau` + count + `" name="ChietKhau` + count + `" class="form-control ">
                            </div>
                        </div>
                    <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Ghi chú" value="` + GhiChu + `"  id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" class="form-control ">
                                </div>
                            </div>
                    <div class="col-md-1 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Mã giới thiệu" value="` + MaGioiThieu + `"  id="MaGioiThieu` + count + `" asp-for="MaGioiThieu` + count + `" name="MaGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Người giới thiệu" value="` + NguoiGioiThieu + `"  id="NguoiGioiThieu` + count + `" asp-for="NguoiGioiThieu` + count + `" name="NguoiGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                        <div class="col-md-1 col-xs-6">
                            <div class="item form-group">
                                    <input class="btn btn-danger" onclick="XoaDong(` + count + `)"  type="button" value="-" />
                            </div>
                        </div>
                       

                    </div>` );
    }
    else {
        document.querySelector('#addRows').insertAdjacentHTML(
            'beforebegin',
            `    <div class="row" id="RowTenKH` + count + `">
                                    <div style="margin-top: 10px;margin-left: 10px;">
                                        <div class="item form-group" style="margin-bottom: 0px;">
                                            <span style="font-size: 12px;color: #3300ff">
                                                <label id="TENKH_NEW` + count + `" ></label>
                                            </span>
                                        </div>
                                    </div>
                                </div>
            <div class="row" id="SoDong` + count + `">
                                 <div class="col-md-1 col-xs-4">
                                        <div class="item form-group">
                                                <input type="text" placeholder="Hãng" id="MAHHK` + count + `" value="` + MAHHK + `" asp-for="MAHHK` + count + `" name="MAHHK` + count + `" class="form-control ">
                                        </div>
                                    </div>

                             <div class="col-md-2 col-xs-8">
                                            <div class="item form-group">
                                                <div class="input-group" style="margin:0px">
                                                    <input type="text" placeholder="Mã KH" value="` + MAKH + `" id="MAKH_NEW` + count + `" asp-for="MAKH_NEW` + count + `" name="MAKH_NEW` + count + `" class="form-control ">
                                                    <span class="input-group-btn">
                                                        <button id="check" onclick="CheckMaKH('MAKH_NEW` + count + `','TENKH_NEW` + count + `');" class="btn btn-info" type="button" style="margin-bottom:0px; "><i class="fa fa-search" aria-hidden="true"></i></button>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-xs-4">
                                            <div class="item form-group">
                                                <input type="text" placeholder="PNR" value="` + PNR + `" id="PNR_NEW` + count + `" asp-for="PNR_NEW` + count + `" name="PNR_NEW` + count + `" class="form-control ">
                                            </div>
                                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Số vé" id="SoVe` + count + `" value="` + SoVe + `" asp-for="SoVe` + count + `" name="SoVe` + count + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-1 col-xs-4">
                            <div class="item form-group">
                                <input type="text" placeholder="Số lượng" onkeyup="ShowPhiXuatVe('PhiXuatVe` + count + `', 'KindName` + count + `', 'SoLuong` + count + `')" id="SoLuong` + count + `" asp-for="SoLuong" name="SoLuong` + count + `" value="` + SoLuong + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Giá mua" value="` + GiaMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="GiaMua` + count + `" asp-for="GiaMua` + count + `" name="GiaMua` + count + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4" hidden>
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí dịch vụ mua" value="` + PhiDVMua + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVMua` + count + `" asp-for="PhiDVMua` + count + `" name="PhiDVMua` + count + `" class="form-control ">
                            </div>
                        </div>
                                   <div class="col-md-1 col-xs-4">
                                <select onchange="ShowPhiXuatVe('PhiXuatVe` + count + `', this.id, 'SoLuong` + count + `')"  id="KindName` + count + `"  class="form-control"> 
                                  
                                </select>
                        </div>
                           <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí xuất vé" disabled value="` + PhiXuatVe + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiXuatVe` + count + `" asp-for="PhiXuatVe` + count + `" name="PhiXuatVe` + count + `" class="form-control ">
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Phí dịch vụ bán" value="` + PhiDVBan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiDVBan` + count + `" asp-for="PhiDVBan` + count + `" name="PhiDVBan` + count + `" class="form-control ">
                            </div>
                        </div>
                         <div class="col-md-2 col-xs-4">
                                        <div class="item form-group">
                                                <input type="text" placeholder="Phí hoàn" value="` + PhiHoan + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="PhiHoan` + count + `" asp-for="PhiHoan` + count + `" name="PhiHoan` + count + `" class="form-control ">
                                        </div>
                                    </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                    <input type="text" placeholder="Chiết khấu" value="` + ChietKhau + `" onkeyup="formatCurrenyGuiMail(document.getElementById(this.id).value,this.id);" id="ChietKhau` + count + `" asp-for="ChietKhau` + count + `" name="ChietKhau` + count + `" class="form-control ">
                            </div>
                        </div>
                     <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Ghi chú" value="` + GhiChu + `"  id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" class="form-control ">
                                </div>
                            </div>
                          <div class="col-md-1 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Mã giới thiệu" value="` + MaGioiThieu + `"  id="MaGioiThieu` + count + `" asp-for="MaGioiThieu` + count + `" name="MaGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                        <input type="text" placeholder="Người giới thiệu" value="` + NguoiGioiThieu + `"  id="NguoiGioiThieu` + count + `" asp-for="NguoiGioiThieu` + count + `" name="NguoiGioiThieu` + count + `" class="form-control ">
                                </div>
                            </div>
                        <div class="col-md-1 col-xs-6">
                            <div class="item form-group">
                                    <input class="btn btn-danger" onclick="XoaDong(` + count + `)"  type="button" value="-" />
                            </div>
                        </div>
                    </div>` );
    }

    var sourceSelect = document.getElementById("KindName");
    var KindNameID = `KindName` + count + ``;
    var destinationSelect = document.getElementById(KindNameID);
    // Clear existing options in the destination select element
    destinationSelect.innerHTML = '';

    var Selected = sourceSelect.options[sourceSelect.selectedIndex].textContent;
    // Iterate over options in the source select element and append clones to the destination
    for (var i = 0; i < sourceSelect.options.length; i++) {
        var sourceOption = sourceSelect.options[i];
        var clonedOption = sourceOption.cloneNode(true);
        if (sourceOption.textContent.toUpperCase().trim() == Selected.toUpperCase().trim()) {
            clonedOption.selected = true;

            var PhiXuatVeID = `PhiXuatVe` + count + ``;
            if (Selected.toUpperCase().trim() == "WEBBSP" || Selected.toUpperCase().trim() == "HOAHONG") {
                document.getElementById(PhiXuatVeID).disabled = false;
                clonedOption.disabled = true;
            }
            else {
                document.getElementById(PhiXuatVeID).disabled = true;
                clonedOption.disabled = true;
            }
        }
       
        destinationSelect.appendChild(clonedOption);
    }

    count++;

}

function XoaDong(SoDong) {
    
    var t = 'RowTenKH' + SoDong;
    console.log(t);
    const element1 = document.getElementById(t);
    console.log(element1);
    element1.remove();

    var e = 'SoDong' + SoDong;
    const element = document.getElementById(e);
    element.remove();
    count--;
}


function selectedRow() {
    var index;
    var hdDieuChinh;
    table = document.getElementById("datatable1");

    for (var i = 1; i < table.rows.length; i++) {
        table.rows[i].onclick = function () {
            // remove the background from the previous selected row
            if (typeof index !== "undefined") {
                table.rows[index].classList.toggle("selected");
            }
            // get the selected row index
            index = this.rowIndex;
            // add class selected to the row
            this.classList.toggle("selected");
            console.log(typeof index);
            var maKH = table.rows[index].cells.item(1).innerHTML;
            var daiLy = table.rows[index].cells.item(2).innerHTML;
            var email = table.rows[index].cells.item(3).innerHTML;
            document.getElementById('MAKH').value = maKH;
            document.getElementById('DAILY').value = daiLy;
            document.getElementById('MAIL').value = email;
        };
    }

    var index2;
    var hdDieuChinh;
    table2 = document.getElementById("datatable2");

    for (var i = 1; i < table2.rows.length; i++) {
        table2.rows[i].onclick = function () {
            // remove the background from the previous selected row
            if (typeof index2 !== "undefined") {
                table2.rows[index].classList.toggle("selected");
            }

            // get the selected row index
            index2 = this.rowIndex;
            // add class selected to the row
            this.classList.toggle("selected");
            console.log(typeof index);
            var maKH = table2.rows[index2].cells.item(1).innerHTML;
            var daiLy = table2.rows[index2].cells.item(2).innerHTML;
            document.getElementById('MAKH_2').value = maKH;
            document.getElementById('DAILY_2').value = daiLy;
        };
    }


}

function formatCurrenyGuiMail(number, id) {
    number = number.replaceAll(",", "");
    x = number.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    document.getElementById(id).value = x1 + x2;
    //document.getElementById(id).value = number;

    return x1 + x2;
}

function FormatPhiDichVu() {
    var TongphiDV = document.getElementById("PHIDICHVU").value.replace(/\$|\,/g, '');
    document.getElementById("PHIDICHVU").value = formatCurrenyGuiMail(Number(TongphiDV));
}

function CheckMaKH(MaKH, TenKH) {
    console.log(MaKH);
    console.log(AreaNameScript.AREA_PhongVe);
    var makh = document.getElementById(MaKH).value;
    if (makh == '') {
        alert("Mã KH không dc để trống");
        return;
    }
    $.ajax({
        type: "POST",
        url: `/${AreaNameScript.AREA_PhongVe}/PhongVe/CheckMaKH`,
        data: {
            MaKH: makh,
        },
        success: function (response) {
            document.getElementById(TenKH).innerHTML =  response;          
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function LuuBaoCao() {
    var ChiTietVes = new Array();
    var MAHHK = document.getElementById("MAHHK").value.toUpperCase();
    var MAKH = document.getElementById("MAKH_NEW").value.toUpperCase();
    var PNR = document.getElementById("PNR_NEW").value.toUpperCase();
    var SOVE = document.getElementById("SoVe").value.toUpperCase();
    var GIAMUA = document.getElementById("GiaMua").value.replace(/\$|\,/g, '');
    var PHIDVMUA = document.getElementById("PhiDVMua").value.replace(/\$|\,/g, '');
    var PHIDVBAN = document.getElementById("PhiDVBan").value.replace(/\$|\,/g, '');
    var PHIHOAN = document.getElementById("PhiHoan").value.replace(/\$|\,/g, '');
    var CHIETKHAU = document.getElementById("ChietKhau").value.replace(/\$|\,/g, '');
    var GHICHU = document.getElementById("GhiChu").value;
    var MAGIOITHIEU = document.getElementById("MaGioiThieu").value;
    var NGUOIGIOITHIEU = document.getElementById("NguoiGioiThieu").value;

    var MAHHK_0 = document.getElementById("MAHHK").value.toUpperCase();
    var SOVE_0 = document.getElementById("SoVe").value.toUpperCase();
    var PNR_0 = document.getElementById("PNR_NEW").value.toUpperCase();
    if (GIAMUA == null || GIAMUA == "") {
        GIAMUA = 0;
    }
    if (PHIDVMUA == null || PHIDVMUA == "") {
        PHIDVMUA = 0;
    }
    if (PHIDVBAN == null || PHIDVBAN == "") {
        PHIDVBAN = 0;
    }
    if (PHIHOAN == null || PHIHOAN == "") {
        PHIHOAN = 0;
    }
    if (CHIETKHAU == null || CHIETKHAU == "") {
        CHIETKHAU = 0;
    }

    var ChiTietVe = {};
    ChiTietVe.MAHHK = MAHHK;
    ChiTietVe.MAKH = MAKH;
    ChiTietVe.PNR = PNR;
    ChiTietVe.SOVE = SOVE;
    ChiTietVe.GIAMUA = GIAMUA;
    ChiTietVe.PHIDVMUA = PHIDVMUA;
    ChiTietVe.PHIDVBAN = PHIDVBAN;
    ChiTietVe.PHIHOAN = PHIHOAN;
    ChiTietVe.CHIETKHAU = CHIETKHAU;
    ChiTietVe.GHICHU = GHICHU;
    ChiTietVe.MAGIOITHIEU = MAGIOITHIEU;
    ChiTietVe.NGUOIGIOITHIEU = NGUOIGIOITHIEU;
    ChiTietVes.push(ChiTietVe);

    if (SOVE == "") {
        alert("Bạn chưa nhập số vé");
        return;
    }
    if (PNR == "") {
        alert("Bạn chưa nhập PNR");
        return;
    }
    if (MAHHK == "") {
        alert("Bạn chưa nhập hãng");
        return;
    }
    if (MAKH == "") {
        alert("Bạn chưa nhập mã KH");
        return;
    }

  
    for (var i = 1; i < count; i++) {
        var soVe = "SoVe" + i;
        var maHHK = "MAHHK" + i;
        var giaMua = "GiaMua" + i;
        var phiDVMua = "PhiDVMua" + i;
        var phiDVBan = "PhiDVBan" + i;
        var phiHoan = "PhiHoan" + i;
        var chietKhau = "ChietKhau" + i;
        var maKH_New = "MAKH_NEW" + i;
        var PNR_New = "PNR_NEW" + i;
        var ghiChu = "GhiChu" + i;
        var maGioiThieu = "MaGioiThieu" + i;
        var nguoiGioiThieu = "NguoiGioiThieu" + i;
        var isDuLieu = document.getElementById(soVe);
        if (isDuLieu) {
            var MAHHK = document.getElementById(maHHK).value.toUpperCase();
            if (MAHHK == "") {
                alert("Bạn chưa nhập hãng");
                return;
            }
            var SOVE = document.getElementById(soVe).value.toUpperCase();
            if (SOVE == "") {
                alert("Bạn chưa nhập số vé");
                return;
            }
            var GIAMUA = document.getElementById(giaMua).value.replace(/\$|\,/g, '');
            if (GIAMUA == "" || GIAMUA == null) {
                GIAMUA = 0;
            }
            var PHIDVMUA = document.getElementById(phiDVMua).value.replace(/\$|\,/g, '');
            if (PHIDVMUA == "" || PHIDVMUA == null) {
                PHIDVMUA = 0;
            }
            var PHIDVBAN = document.getElementById(phiDVBan).value.replace(/\$|\,/g, '');
            if (PHIDVBAN == "" || PHIDVBAN == null) {
                PHIDVBAN = 0;
            }
            var PHIHOAN = document.getElementById(phiHoan).value.replace(/\$|\,/g, '');
            if (PHIHOAN == "" || PHIHOAN == null) {
                PHIHOAN = 0;
            }
            var CHIETKHAU = document.getElementById(chietKhau).value.replace(/\$|\,/g, '');
            if (CHIETKHAU == "" || CHIETKHAU == null) {
                CHIETKHAU = 0;
            }
            var MAKH_NEW = document.getElementById(maKH_New).value.toUpperCase();
            if (MAKH_NEW == "") {
                alert("Bạn chưa nhập mã KH");
                return;
            }
            var PNR_NEW = document.getElementById(PNR_New).value.toUpperCase();
            if (PNR_NEW == "") {
                alert("Bạn chưa nhập PNR");
                return;
            }
            var GHICHU = document.getElementById(ghiChu).value.toUpperCase();
            var MAGIOITHIEU = document.getElementById(maGioiThieu).value.toUpperCase();
            var NGUOIGIOITHIEU = document.getElementById(nguoiGioiThieu).value.toUpperCase();
            var ChiTietVe = {};
            ChiTietVe.MAHHK = MAHHK;
            ChiTietVe.SOVE = SOVE;
            ChiTietVe.GIAMUA = GIAMUA;
            ChiTietVe.PHIDVMUA = PHIDVMUA;
            ChiTietVe.PHIDVBAN = PHIDVBAN;
            ChiTietVe.CHIETKHAU = CHIETKHAU;
            ChiTietVe.PHIHOAN = PHIHOAN;
            ChiTietVe.MAKH = MAKH_NEW;
            ChiTietVe.PNR = PNR_NEW;
            ChiTietVe.GHICHU = GHICHU;
            ChiTietVe.MAGIOITHIEU = MAGIOITHIEU;
            ChiTietVe.NGUOIGIOITHIEU = NGUOIGIOITHIEU;

            if (SOVE_0 == SOVE && PNR_0 == PNR_NEW && MAHHK_0 == MAHHK) {
                alert("Số vé " + SOVE +" bị trùng xin vui lòng kiểm tra lại 1");
                return;
            }

            for (var y = 1; y <= i; y++) {
                var soVe_old = "SoVe" + y;
                var isDuLieu1 = document.getElementById(soVe_old);
                if (isDuLieu1) {
                    var PNR_old = "PNR_NEW" + y;
                    var SOVE_OLD = document.getElementById(soVe_old).value.toUpperCase();
                    var PNR_OLD = document.getElementById(PNR_old).value.toUpperCase();
                    var maHHK_old = "MAHHK" + y;
                    var MAHHK_OLD = document.getElementById(maHHK_old).value.toUpperCase();
                   
                    if (i != y) {
                        if (SOVE_OLD == SOVE && PNR_OLD == PNR_NEW && MAHHK_OLD == MAHHK) {
                            alert("Số vé " + SOVE + " bị trùng xin vui lòng kiểm tra lại 2");
                            return;
                        }
                    }
                   
                }
              
              }
            ChiTietVes.push(ChiTietVe);
        }
    }
   
    $.ajax({
        type: "POST",
        url: `/${AreaNameScript.AREA_BaoCaoVe}/BaoCaoVe/SaveBaoCao`,

        data: { ListChiTietVe: ChiTietVes},
        dataType: "json",
        beforeSend: function () {
            //show loading
            $("#loader").show();
        },
        complete: function () {
            //hide loading
            $("#loader").hide();
        },
        success: function (resultData) {
            if (resultData == "") {
                alert('Lưu thành công');
                location.reload();
            }
            else {
                alert(resultData);
            }
            $("#loader").hide();
        },
        error: function (xhr, status, p3, p4) {
            $("#loader").hide();
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText)
                err = xhr.responseText;
            alert(err);
        }
    })
}

function LuuBaoCaoVoQuy() {
    var ChiTietVes = new Array();
    var MAHHK = document.getElementById("MAHHK").value.toUpperCase();
    var MAKH = document.getElementById("MAKH_NEW").value.toUpperCase();
    var PNR = document.getElementById("PNR_NEW").value.toUpperCase();
    var SOVE = document.getElementById("SoVe").value.toUpperCase();
    var SOLUONG = document.getElementById("SoLuong").value;
    var GIAMUA = document.getElementById("GiaMua").value.replace(/\$|\,/g, '');
    var PHIDVMUA = document.getElementById("PhiDVMua").value.replace(/\$|\,/g, '');
    var LOAIPHI = document.getElementById("KindName").selectedOptions[0].textContent;
    var PHIXUATVE = document.getElementById("PhiXuatVe").value.replace(/\$|\,/g, '');
    var PHIDVBAN = document.getElementById("PhiDVBan").value.replace(/\$|\,/g, '');
    var PHIHOAN = document.getElementById("PhiHoan").value.replace(/\$|\,/g, '');
    var CHIETKHAU = document.getElementById("ChietKhau").value.replace(/\$|\,/g, '');
    var GHICHU = document.getElementById("GhiChu").value;
    var MAGIOITHIEU = document.getElementById("MaGioiThieu").value;
    var NGUOIGIOITHIEU = document.getElementById("NguoiGioiThieu").value;
   

    var MAHHK_0 = document.getElementById("MAHHK").value.toUpperCase();
    var SOVE_0 = document.getElementById("SoVe").value.toUpperCase();
    var PNR_0 = document.getElementById("PNR_NEW").value.toUpperCase();
    if (GIAMUA == null || GIAMUA == "" || GIAMUA == 0) {
        alert("Bạn phải nhập giá mua");
        return;
    }
    if (SOLUONG == null || SOLUONG == "") {
        SOLUONG = 1;
    }
    if (PHIXUATVE == null || PHIXUATVE == "") {
        PHIXUATVE = 0;
    }
    if (PHIDVMUA == null || PHIDVMUA == "") {
        PHIDVMUA = 0;
    }
    if (PHIDVBAN == null || PHIDVBAN == "") {
        PHIDVBAN = 0;
    }
    if (PHIHOAN == null || PHIHOAN == "") {
        PHIHOAN = 0;
    }
    if (CHIETKHAU == null || CHIETKHAU == "") {
        CHIETKHAU = 0;
    }

    var ChiTietVe = {};
    ChiTietVe.MAHHK = MAHHK;
    ChiTietVe.MAKH = MAKH;
    ChiTietVe.PNR = PNR;
    ChiTietVe.SOVE = SOVE;
    ChiTietVe.SOLUONG = SOLUONG;
    ChiTietVe.GIAMUA = GIAMUA;
    ChiTietVe.PHIDVMUA = PHIDVMUA;
    ChiTietVe.LOAIPHI = LOAIPHI;
    ChiTietVe.PHIXUATVE = PHIXUATVE;
    ChiTietVe.PHIDVBAN = PHIDVBAN;
    ChiTietVe.PHIHOAN = PHIHOAN;
    ChiTietVe.CHIETKHAU = CHIETKHAU;
    ChiTietVe.GHICHU = GHICHU;
    ChiTietVe.MAGIOITHIEU = MAGIOITHIEU;
    ChiTietVe.NGUOIGIOITHIEU = NGUOIGIOITHIEU;
    ChiTietVes.push(ChiTietVe);

    if (SOLUONG == "" || SOLUONG == "0") {
        alert("Số lượng phải lớn hơn 0");
        return;
    }

    if (SOVE == "") {
        alert("Bạn chưa nhập số vé");
        return;
    }
    if (PNR == "") {
        alert("Bạn chưa nhập PNR");
        return;
    }
    if (MAHHK == "") {
        alert("Bạn chưa nhập hãng");
        return;
    }
    if (MAKH == "") {
        alert("Bạn chưa nhập mã KH");
        return;
    }

    //if (count > 50) {
    //    alert("Chỉ được tối đa 50 vé");
    //    return;
    //}

    for (var i = 1; i < count; i++) {
        var soVe = "SoVe" + i;
        var soLuong = "SoLuong" + i;
        var maHHK = "MAHHK" + i;
        var giaMua = "GiaMua" + i;
        var phiDVMua = "PhiDVMua" + i;
        var phiXuatVe = "PhiXuatVe" + i;
        var loaiPhi = "KindName" + i;
        var phiDVBan = "PhiDVBan" + i;
        var phiHoan = "PhiHoan" + i;
        var chietKhau = "ChietKhau" + i;
        var maKH_New = "MAKH_NEW" + i;
        var PNR_New = "PNR_NEW" + i;
        var ghiChu = "GhiChu" + i;
        var maGioiThieu = "MaGioiThieu" + i;
        var nguoiGioiThieu = "NguoiGioiThieu" + i;
        var isDuLieu = document.getElementById(soVe);
        if (isDuLieu) {
            var MAHHK = document.getElementById(maHHK).value.toUpperCase();
            if (MAHHK == "") {
                alert("Bạn chưa nhập hãng");
                return;
            }
            var SOVE = document.getElementById(soVe).value.toUpperCase();
            if (SOVE == "") {
                alert("Bạn chưa nhập số vé");
                return;
            }
            var SOLUONG = document.getElementById(soLuong).value.replace(/\$|\,/g, '');
            if (SOLUONG == "" || SOLUONG == null || SOLUONG == 0) {
                alert("Bạn phải nhập số lượng");
                return;
            }
            var GIAMUA = document.getElementById(giaMua).value.replace(/\$|\,/g, '');
            if (GIAMUA == "" || GIAMUA == null || GIAMUA == 0) {
                alert("Bạn phải nhập giá mua");
                return;
            }
            var PHIDVMUA = document.getElementById(phiDVMua).value.replace(/\$|\,/g, '');
            if (PHIDVMUA == "" || PHIDVMUA == null) {
                PHIDVMUA = 0;
            }
            var PHIXUATVE = document.getElementById(phiXuatVe).value.replace(/\$|\,/g, '');
            if (PHIXUATVE == "" || PHIXUATVE == null) {
                PHIXUATVE = 0;
            }
            var PHIDVBAN = document.getElementById(phiDVBan).value.replace(/\$|\,/g, '');
            if (PHIDVBAN == "" || PHIDVBAN == null) {
                PHIDVBAN = 0;
            }
            var PHIHOAN = document.getElementById(phiHoan).value.replace(/\$|\,/g, '');
            if (PHIHOAN == "" || PHIHOAN == null) {
                PHIHOAN = 0;
            }
            var CHIETKHAU = document.getElementById(chietKhau).value.replace(/\$|\,/g, '');
            if (CHIETKHAU == "" || CHIETKHAU == null) {
                CHIETKHAU = 0;
            }
            var MAKH_NEW = document.getElementById(maKH_New).value.toUpperCase();
            if (MAKH_NEW == "") {
                alert("Bạn chưa nhập mã KH");
                return;
            }
            var PNR_NEW = document.getElementById(PNR_New).value.toUpperCase();
            if (PNR_NEW == "") {
                alert("Bạn chưa nhập PNR");
                return;
            }
            var GHICHU = document.getElementById(ghiChu).value.toUpperCase();
            var MAGIOITHIEU = document.getElementById(maGioiThieu).value.toUpperCase();
            var NGUOIGIOITHIEU = document.getElementById(nguoiGioiThieu).value.toUpperCase();
            var LOAIPHI = document.getElementById(loaiPhi).selectedOptions[0].textContent;
            var ChiTietVe = {};
            ChiTietVe.MAHHK = MAHHK;
            ChiTietVe.SOVE = SOVE;
            ChiTietVe.SOLUONG = SOLUONG;
            ChiTietVe.GIAMUA = GIAMUA;
            ChiTietVe.PHIDVMUA = PHIDVMUA;
            ChiTietVe.LOAIPHI = LOAIPHI;
            ChiTietVe.PHIXUATVE = PHIXUATVE;
            ChiTietVe.PHIDVBAN = PHIDVBAN;
            ChiTietVe.CHIETKHAU = CHIETKHAU;
            ChiTietVe.PHIHOAN = PHIHOAN;
            ChiTietVe.MAKH = MAKH_NEW;
            ChiTietVe.PNR = PNR_NEW;
            ChiTietVe.GHICHU = GHICHU;
            ChiTietVe.MAGIOITHIEU = MAGIOITHIEU;
            ChiTietVe.NGUOIGIOITHIEU = NGUOIGIOITHIEU;

            if (SOVE_0 == SOVE && PNR_0 == PNR_NEW && MAHHK_0 == MAHHK) {
                alert("Số vé " + SOVE + " bị trùng xin vui lòng kiểm tra lại 1");
                return;
            }

            for (var y = 1; y <= i; y++) {
                var soVe_old = "SoVe" + y;
                var isDuLieu1 = document.getElementById(soVe_old);
                if (isDuLieu1) {
                    var PNR_old = "PNR_NEW" + y;
                    var SOVE_OLD = document.getElementById(soVe_old).value.toUpperCase();
                    var PNR_OLD = document.getElementById(PNR_old).value.toUpperCase();
                    var maHHK_old = "MAHHK" + y;
                    var MAHHK_OLD = document.getElementById(maHHK_old).value.toUpperCase();

                    if (i != y) {
                        if (SOVE_OLD == SOVE && PNR_OLD == PNR_NEW && MAHHK_OLD == MAHHK) {
                            alert("Số vé " + SOVE + " bị trùng xin vui lòng kiểm tra lại 2");
                            return;
                        }
                    }

                }

            }
            ChiTietVes.push(ChiTietVe);
        }
    }
    let DataJson = JSON.stringify(ChiTietVes);

    $.ajax({
        type: "POST",
        url: `/${AreaNameScript.AREA_BaoCaoVe}/BaoCaoVe/SaveBaoCaoVoQuy`,

        data: { data: DataJson },
        dataType: "json",
        beforeSend: function () {
            //show loading
            $("#loader").show();
        },
        complete: function () {
            //hide loading
            $("#loader").hide();
        },
        success: function (resultData) {
            if (resultData == "") {
                alert('Lưu thành công');
                location.reload();
            }
            else {
                alert(resultData);
            }
            $("#loader").hide();
        },
        error: function (xhr, status, p3, p4) {
            $("#loader").hide();
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText)
                err = xhr.responseText;
            alert(err);
        }
    })
}

function ShowPhiXuatVe(PhiXuatVeId, Id, SoLuong) {
    var SL = document.getElementById(SoLuong).value;
    var Name = document.getElementById(Id).selectedOptions[0].textContent;
    if (Name == "WebBSP" || Name == "HoaHong") {
        document.getElementById(PhiXuatVeId).value = 0;
        document.getElementById(PhiXuatVeId).disabled = false;
    }
    else {
        var phiXuatVe = SL * document.getElementById(Id).value;
        if (phiXuatVe == -1) {
            phiXuatVe = 0;
        }
        document.getElementById(PhiXuatVeId).value = phiXuatVe;
        document.getElementById(PhiXuatVeId).disabled = true;
    }
  
 
}
selectedRow();