
function selectedRow(){
                
                var index;
                table = document.getElementById("datatable1");
             
       
                for(var i = 1; i < table.rows.length; i++)
                {
                    table.rows[i].onclick = function()
                    {
                         // remove the background from the previous selected row
                        if(typeof index !== "undefined"){
                           table.rows[index].classList.toggle("selected");
                        }
                        console.log(typeof index);
                        // get the selected row index
                        index = this.rowIndex;
                        // add class selected to the row
                        this.classList.toggle("selected");
                        console.log(typeof index);
                        var kyhieu_HD = table.rows[index].cells.item(1).innerHTML;
                        var ikey = table.rows[index].cells.item(3).innerHTML;
                        var mauso = table.rows[index].cells.item(2).innerHTML;
                        var MaKH = table.rows[index].cells.item(20).innerHTML;
                    
                        $("#Serial").val(kyhieu_HD);
                        $("#SoHD").val(ikey);
                        $("#Pattern").val(mauso);
                        $("#makh").val(mauso);
                                 $.ajax({
                                type: "GET",
                                url: "../Ketoan/LayDanhSachSoVeHDDT",
                                        data: { ikey: ikey, serial: kyhieu_HD, pattern: mauso, maKH: MaKH },
                             
                                        
                                dataType: "json",
                                beforeSend: function () {
                                    //show loading
                                },
                                complete: function () {
                                    //hide loading
                                },
                                success: function (resultData) {
                                 
                                        //Clear datatable2
                                        var table2 = document.getElementById("datatable2");
                                         var tableHeaderRowCount = 1;
                                         var rowCount = table2.rows.length;
                                        for (var i = tableHeaderRowCount; i < rowCount; i++) {
                                            table2.deleteRow(tableHeaderRowCount);
                                        }
                                        //Add data datatable2
                                      
                                        for(var i =0;i < resultData.length;i++)
                                        {

                                            var item = resultData[i];
                                            var row = table2.insertRow(i+1);
                                            var cell0 = row.insertCell(0);

                                            var cell1 = row.insertCell(1);
                                            var cell2 = row.insertCell(2);
                                            var cell3 = row.insertCell(3);
                                            var cell4 = row.insertCell(4);
                                            var cell5 = row.insertCell(5);
                                            var cell6 = row.insertCell(6);
                                            var cell7 = row.insertCell(7);
                                            var cell8 = row.insertCell(8);
                                            var cell9 = row.insertCell(9);
                                            var cell10 = row.insertCell(10);


                                            cell0.innerHTML = Number(i + 1);
                                            cell0.style.textAlign = "center";
                                            cell1.innerHTML = item.soVe;
                                            cell1.style.textAlign = "center";
                                            cell2.innerHTML = item.hanhTrinh;
                                            cell2.style.textAlign = "center";
                                            cell3.innerHTML = formatNumber(item.soLuong);
                                            cell3.style.textAlign = "center";
                                            cell4.innerHTML = formatNumber(item.donGia);
                                            cell4.style.textAlign = "center";
                                            cell5.innerHTML = formatNumber(item.thanhTien);
                                            cell5.style.textAlign = "center";
                                            cell6.innerHTML = formatNumber(item.thue);
                                            cell6.style.textAlign = "center";
                                            cell7.innerHTML = formatNumber(item.phiKhac);
                                            cell7.style.textAlign = "center";
                                            cell8.innerHTML = formatNumber(item.phiDoi);
                                            cell8.style.textAlign = "center";
                                            cell9.innerHTML = formatNumber(item.phiHoan);
                                            cell9.style.textAlign = "center"
                                            cell10.innerHTML = formatNumber(item.tongCong);
                                            cell10.style.textAlign = "center"
                                            
                                        }
                
                     
        
                 
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

selectedRow();

 function formatNumber(number) {
        number = number.toFixed(0) + '';
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
}


function XemHD(obj) {


    var Ikey = document.getElementById("SoHD").value;
    var Serial = document.getElementById("Serial").value;
    var Pattern = document.getElementById("Pattern").value;

    if (Ikey == "") {
        alert("Ikey không được bỏ trống");
        return;
    }

    if (Pattern == "") {
        alert("Pattern không được bỏ trống");
        return;
    }

    var url = window.location.href;

    $.ajax({
        type: "POST",
        url: "../Ketoan/XemHoaDon",

        data: { ikey: Ikey, pattern: Pattern },
        dataType: "json",
        beforeSend: function () {
            $("#loader").show();
            //show loading

        },
        complete: function (response) {
            $("#loader").hide();
            //hide loading
        },
        success: function (resultData) {

            $("#loader").hide();
            var binary = atob(resultData.replace(/\s/g, ''));
            var len = binary.length;
            var buffer = new ArrayBuffer(len);
            var view = new Uint8Array(buffer);
            for (var i = 0; i < len; i++) {
                view[i] = binary.charCodeAt(i);
            }

            var blob = new Blob([view], { type: "application/pdf" });

            var url = URL.createObjectURL(blob);

            window.open(url);



        },
        error: function (xhr, status, p3, p4) {
            $("#loader").hide();

        }
    })

}

function CancelHD(obj) {



    var Ikey = document.getElementById("SoHD").value;
    var Serial = document.getElementById("Serial").value;
    var Pattern = document.getElementById("Pattern").value;

    if (Ikey == "") {
        alert("Ikey không được bỏ trống");
        return;
    }
  
    if (Pattern == "") {
        alert("Pattern không được bỏ trống");
        return;
    }

    var check = confirm("Bạn chắc chắn muốn hủy hóa đơn: " + Serial + ", ikey: " + Ikey + ", Mẫu số: " + Pattern + "");
    if (check == true) {

        $.ajax({
            type: "POST",
            url: "../Ketoan/CancelHoaDon",

            data: { ikey: Ikey, serial: Serial, pattern: Pattern },
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
                $("#loader").hide();
                if (resultData.status == "200") {
                    alert("Hủy hóa đơn thành công !");
                    location.reload();
                }
                else {
                    alert(resultData.message);
                 
                }

               

            },
            error: function (xhr, status, p3, p4) {
                $("#loader").hide();
                alert("Hủy hóa đơn thất bại !");

            }
        })
    }

}


           