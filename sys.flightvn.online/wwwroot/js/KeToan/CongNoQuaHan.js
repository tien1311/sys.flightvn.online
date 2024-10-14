
function selectedRow() {
    var index;
    table = document.getElementById("gridCNNVQH");

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

            var ID = table.getElementsByTagName('tr')[index].id;
            $.ajax({
                type: "GET",
                url: "../KeToan/GetCongNoNVQuaHan",
                data: { id: ID },
                dataType: "json",
                success: function (resultData) {
                    document.getElementById("ID").value = ID;
                    document.getElementById("TieuDe").value = resultData.tieuDe;
                    document.getElementById("Thang").value = resultData.thang;
                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText)
                        err = xhr.responseText;
                    alert(err);
                    console.log(err);
                }
            })
        };
    }


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
    for (var i = 0; i < excelRows.length; i++) {
        if (excelRows[i].MaNV == null || excelRows[i].MaNV == "") {
            excelRows[i].MaNV = "";
        }
        if (excelRows[i].TenNV == null || excelRows[i].TenNV == "") {
            excelRows[i].TenNV = "";
        }
        if (excelRows[i].SoTienNo == null || excelRows[i].SoTienNo == "") {
            excelRows[i].SoTienNo = 0;
        }
        if (excelRows[i].ThoiGianXuat == null || excelRows[i].ThoiGianXuat == "") {
            excelRows[i].ThoiGianXuat = "";
        }
        if (excelRows[i].DuNoHienTai == null || excelRows[i].DuNoHienTai == "") {
            excelRows[i].DuNoHienTai = 0;
        }
        if (excelRows[i].TinhTrang == null || excelRows[i].TinhTrang == "") {

            excelRows[i].TinhTrang = "";
        }
        if (excelRows[i].GhiChu == null || excelRows[i].GhiChu == "") {

            excelRows[i].GhiChu = "";
        }
        if (i == 0) {
            document.getElementById("MaNV").value = excelRows[i].MaNV;
            document.getElementById("TenNV").value = excelRows[i].TenNV;
            document.getElementById("SoTienNo").value = excelRows[i].SoTienNo;
            document.getElementById("ThoiGianXuatVe").value = excelRows[i].ThoiGianXuat;
            document.getElementById("DuNo").value = excelRows[i].DuNoHienTai;
            document.getElementById("GhiChu").value = excelRows[i].GhiChu;
            document.getElementById("TinhTrang").value = excelRows[i].TinhTrang;
        }
        else {
            var TT = "";
            if (excelRows[i].TinhTrang == "Normal") {
                TT = `
                    <option value="Normal" selected>Normal</option>
                    <option value="High">High</option>
                `;
            }
            else {
                TT = `
                    <option value="Normal" >Normal</option>
                    <option value="High" selected>High</option>
                `;
            }
            document.querySelector('#addRows').insertAdjacentHTML(
                'beforebegin',
                `<div class="row" id="SoDong` + count + `">
        <div class="col-md-1 col-xs-4">
            <div class="item form-group">
                <input type="text" placeholder="MaNV" id="MaNV` + count + `" asp-for="MaNV` + count + `" name="MaNV` + count + `" value="` + excelRows[i].MaNV + `" class="form-control ">
                        </div>
            </div>
            <div class="col-md-2 col-xs-4">
                <div class="item form-group">
                    <input type="text" placeholder="Tên NV" id="TenNV` + count + `" asp-for="TenNV` + count + `" name="TenNV` + count + `" value="` + excelRows[i].TenNV + `" class="form-control ">
                        </div>
                </div>
                <div class="col-md-2 col-xs-4">
                    <div class="item form-group">
                        <input type="text" placeholder="Số tiền nợ" id="SoTienNo` + count + `" asp-for="SoTienNo` + count + `" name="SoTienNo` + count + `" value="` + excelRows[i].SoTienNo + `" onblur="formatNumberCNQH(this.value, this.id)" class="form-control ">
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-4">
                        <div class="item form-group">
                            <input type="text" placeholder="Thời gian xuất vé" id="ThoiGianXuatVe` + count + `" asp-for="ThoiGianXuatVe` + count + `" name="ThoiGianXuatVe` + count + `" value="` + excelRows[i].ThoiGianXuat + `" class="form-control ">
                        </div>
                        </div>
                        <div class="col-md-2 col-xs-4">
                            <div class="item form-group">
                                <input type="text" placeholder="Dư nợ hiện tại" id="DuNo` + count + `" asp-for="DuNo` + count + `" name="DuNo` + count + `" value="` + excelRows[i].DuNoHienTai + `" onblur="formatNumberCNQH(this.value, this.id)" class="form-control ">
                        </div>
                            </div>
                            <div class="col-md-1 col-xs-4">
                                <div class="item form-group">
                                    <div class="control-group">
                                        <div class="controls">
                                            <div class=" xdisplay_inputx form-group has-feedback">
                                                <select id="TinhTrang` + count + `" name="TinhTrang` + count + `" asp-for="TinhTrang` + count + `" class="form-control" style=" padding-right: 0px;">
                                                    `+ TT + `
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-4">
                                <div class="item form-group">
                                    <input type="text" placeholder="Ghi chú" id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" value="` + excelRows[i].GhiChu + `" class="form-control ">
                        </div>
                                </div>
                                <div class="col-md-1 col-xs-6">
                                    <div class="item form-group">
                                        <input class="btn btn-danger" onclick="XoaDong(` + count + `)" type="button" value="-" />
                                    </div>
                                </div>
                            </div>` );
            count++;
        }
    }
};
function ThemDong() {
    var MaNV = document.getElementById("MaNV").value;
    var TenNV = document.getElementById("TenNV").value;
    var SoTienNo = document.getElementById("SoTienNo").value;
    var ThoiGianXuatVe = document.getElementById("ThoiGianXuatVe").value;
    var DuNo = document.getElementById("DuNo").value;
    var GhiChu = document.getElementById("GhiChu").value;

    if (SoTienNo == null || SoTienNo == "") {
        SoTienNo = 0;
    }
    if (DuNo == null || DuNo == "") {
        DuNo = 0;
    }

    document.querySelector('#addRows').insertAdjacentHTML(
        'beforebegin',
        `<div class="row" id="SoDong` + count + `">
                                <div class="col-md-1 col-xs-4">
                                    <div class="item form-group">
                                        <input type="text" placeholder="MaNV" id="MaNV` + count + `" asp-for="MaNV` + count + `" name="MaNV` + count + `" class="form-control ">
                        </div>
                                    </div>
                                    <div class="col-md-2 col-xs-4">
                                        <div class="item form-group">
                                            <input type="text" placeholder="Tên NV" id="TenNV` + count + `" asp-for="TenNV` + count + `" name="TenNV` + count + `" class="form-control ">
                        </div>
                                        </div>
                                        <div class="col-md-2 col-xs-4">
                                            <div class="item form-group">
                                                <input type="text" placeholder="Số tiền nợ" id="SoTienNo` + count + `" asp-for="SoTienNo` + count + `" name="SoTienNo` + count + `" onblur="formatNumberCNQH(this.value, this.id)" class="form-control ">
                        </div>
                                            </div>
                                            <div class="col-md-1 col-xs-4">
                                                <div class="item form-group">
                                                    <input type="text" placeholder="Thời gian xuất vé" id="ThoiGianXuatVe` + count + `" asp-for="ThoiGianXuatVe` + count + `" name="ThoiGianXuatVe` + count + `" class="form-control ">
                        </div>
                                                </div>
                                                <div class="col-md-2 col-xs-4">
                                                    <div class="item form-group">
                                                        <input type="text" placeholder="Dư nợ hiện tại" id="DuNo` + count + `" asp-for="DuNo` + count + `" name="DuNo` + count + `" onblur="formatNumberCNQH(this.value, this.id)" class="form-control ">
                        </div>
                                                    </div>
                                                    <div class="col-md-1 col-xs-4">
                                                        <div class="item form-group">
                                                            <div class="control-group">
                                                                <div class="controls">
                                                                    <div class=" xdisplay_inputx form-group has-feedback">
                                                                        <select id="TinhTrang` + count + `" name="TinhTrang` + count + `" asp-for="TinhTrang` + count + `" class="form-control" style=" padding-right: 0px;">
                                                                            <option value="Normal">Normal</option>
                                                                            <option value="High">High</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 col-xs-4">
                                                        <div class="item form-group">
                                                            <input type="text" placeholder="Ghi chú" id="GhiChu` + count + `" asp-for="GhiChu` + count + `" name="GhiChu` + count + `" class="form-control ">
                        </div>
                                                        </div>
                                                        <div class="col-md-1 col-xs-6">
                                                            <div class="item form-group">
                                                                <input class="btn btn-danger" onclick="XoaDong(` + count + `)" type="button" value="-" />
                                                            </div>
                                                        </div>
                                                    </div>` );
    count++;
}

function XoaDong(SoDong) {
    var e = 'SoDong' + SoDong;
    const element = document.getElementById(e);
    element.remove();
    count--;
}

function formatNumberCNQH(number, id) {
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

function LuuCongNoQuaHan() {
    var CongNoQuaHans = new Array();
    var TieuDe = document.getElementById("TieuDe").value;
    var Thang = document.getElementById("Thang").value;
    var MaNV = document.getElementById("MaNV").value;
    var TenNV = document.getElementById("TenNV").value;
    var SoTienNo = document.getElementById("SoTienNo").value.replace(/\$|\,/g, '');
    var ThoiGianXuatVe = document.getElementById("ThoiGianXuatVe").value;
    var DuNo = document.getElementById("DuNo").value.replace(/\$|\,/g, '');
    var GhiChu = document.getElementById("GhiChu").value;
    var TinhTrang = document.getElementById("TinhTrang").value;

    if (SoTienNo == null || SoTienNo == "") {
        SoTienNo = 0;
    }
    if (DuNo == null || DuNo == "") {
        DuNo = 0;
    }
    if (TieuDe == null || TieuDe == "") {
        alert("Bạn chưa nhập tiêu đề");
        return;
    }
    var CongNoQuaHan = {};
    CongNoQuaHan.MaNV = MaNV;
    CongNoQuaHan.TenNV = TenNV;
    CongNoQuaHan.SoTienNo = SoTienNo;
    CongNoQuaHan.ThoiGianXuatVe = ThoiGianXuatVe;
    CongNoQuaHan.DuNo = DuNo;
    CongNoQuaHan.GhiChu = GhiChu;
    CongNoQuaHan.TinhTrang = TinhTrang;
    CongNoQuaHans.push(CongNoQuaHan);

    for (var i = 1; i < count; i++) {
        var maNV = "MaNV" + i;
        var tenNV = "TenNV" + i;
        var soTienNo = "SoTienNo" + i;
        var thoiGianXuatVe = "ThoiGianXuatVe" + i;
        var duNo = "DuNo" + i;
        var ghiChu = "GhiChu" + i;
        var tinhTrang = "TinhTrang" + i;


        var MaNV = document.getElementById(maNV).value;
        
        var TenNV = document.getElementById(tenNV).value;
        var ThoiGianXuatVe = document.getElementById(thoiGianXuatVe).value;
        
        var SoTienNo = document.getElementById(soTienNo).value.replace(/\$|\,/g, '');
        if (SoTienNo == "" || SoTienNo == null) {
            SoTienNo = 0;
        }
        var DuNo = document.getElementById(duNo).value.replace(/\$|\,/g, '');
        if (DuNo == "" || DuNo == null) {
            DuNo = 0;
        }
        var GhiChu = document.getElementById(ghiChu).value;
        var TinhTrang = document.getElementById(tinhTrang).value;

        var CongNoQuaHan = {};
        CongNoQuaHan.MaNV = MaNV;
        CongNoQuaHan.TenNV = TenNV;
        CongNoQuaHan.SoTienNo = SoTienNo;
        CongNoQuaHan.ThoiGianXuatVe = ThoiGianXuatVe;
        CongNoQuaHan.DuNo = DuNo;
        CongNoQuaHan.GhiChu = GhiChu;
        CongNoQuaHan.TinhTrang = TinhTrang;
        CongNoQuaHans.push(CongNoQuaHan);
    }
    let DataJson = JSON.stringify(CongNoQuaHans);

    $.ajax({
        type: "POST",
        url: "../KeToan/SaveCongNoQuaHan",
        data: {
            data: DataJson,
            TieuDe: TieuDe,
            Thang: Thang
        },
        dataType: "json",
        success: function (resultData) {
            alert(resultData);
            window.location.href = "../KeToan/CongNoQuaHan?i=5";
        },
        error: function () {
        }
    })
}

function UpdateCongNoQuaHan() {
    var ID = document.getElementById("ID").value;
    var TieuDe = document.getElementById("TieuDe").value;
    var Thang = document.getElementById("Thang").value;
    if (TieuDe == null || TieuDe == "") {
        alert("Bạn chưa nhập tiêu đề");
        return;
    }
    $.ajax({
        type: "POST",
        url: "../KeToan/UpdateCongNoQuaHan",
        data: {
            ID: ID,
            TieuDe: TieuDe,
            Thang: Thang
        },
        dataType: "json",
        success: function (resultData) {
            alert(resultData);
            window.location.href = "../KeToan/CongNoQuaHan?i=5";
        },
        error: function () {
        }
    })
}

$("#gridCNNVQH .ImportData").click(function () {

    var ID = String($(this).closest('tr').attr('id'));
    $.ajax({
        type: "POST",
        url: "../KeToan/DS_CNNVQH_Detail",
        data: {
            id: ID
        },
        success: function (response) {
            $('#openPopup').html(response);
            $('#openPopup').modal('show');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
});
selectedRow();
