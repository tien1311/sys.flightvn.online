let pageSize = 10;
//#region Phân trang
function loadPage(page) {
    let agentCodes = document.getElementById('agentCodes').value;
    let phoneNumbers = document.getElementById('phoneNumbers').value;
    let secondarySerials = document.getElementById('secondarySerials').value;
    loadOrders(agentCodes, phoneNumbers, secondarySerials, pageSize, page)
}


function loadOrders(agentCodes, phoneNumbers, secondarySerials, pageSize, page = 1) {
    $.post(
        '../KeToan/GetAllBankAccountDetailByFilter',
        {
            agentCodes: agentCodes,
            phoneNumbers: phoneNumbers,
            secondarySerials: secondarySerials,
            pageSize: pageSize,
            page: page
        },
        (data) => {
            $("#table_Pagination").html(data);
            $("html, body").animate({ scrollTop: 0 }, "slow"); // Kéo page lên đầu
        }
    );
}

// Chọn số lượng sản phẩm hiển thị
$(document).on('change', '#Page_Size', function (e) {
    let agentCodes = document.getElementById('agentCodes').value;
    let phoneNumbers = document.getElementById('phoneNumbers').value;
    let secondarySerials = document.getElementById('secondarySerials').value;
    pageSize = e.target.value;
    loadOrders(agentCodes, phoneNumbers, secondarySerials, pageSize)
});

$(document).on('input', '#agentCodes, #phoneNumbers, #secondarySerials', function (e) {
    e.preventDefault();
    let agentCodes = document.getElementById('agentCodes').value;
    let phoneNumbers = document.getElementById('phoneNumbers').value;
    let secondarySerials = document.getElementById('secondarySerials').value;
    loadOrders(agentCodes, phoneNumbers, secondarySerials, pageSize);
});

$(document).on('click', '#btnUseExcelFileExample', function (e) {
    //// URL đến file Excel mẫu trên server
    //const fileUrl = '/UploadFile/Bank_Account_Detail.xlsx';

    //// Tạo liên kết để tải xuống
    //const link = document.createElement('a');
    //link.href = fileUrl;
    //link.download = 'Bank_Account_Detail.xlsx'; // Tên file tải về
    //document.body.appendChild(link);
    //link.click();
    //document.body.removeChild(link);

    e.preventDefault();

    // URL đến file Excel mẫu trên server
    const fileUrl = '/UploadFile/Bank_Account_Detail.xlsx';
    let agentCodes = document.getElementById('agentCodes').value;
    let phoneNumbers = document.getElementById('phoneNumbers').value;
    let secondarySerials = document.getElementById('secondarySerials').value;


    // Tải file từ server
    fetch(fileUrl)
        .then(response => response.blob())
        .then(blob => {
            // Tạo đối tượng FormData
            let formData = new FormData();
            formData.append('agentCodes', agentCodes);
            formData.append('phoneNumbers', phoneNumbers);
            formData.append('secondarySerials', secondarySerials);
            formData.append('excelFile', blob, 'Bank_Account_Detail.xlsx');

            // Thực hiện yêu cầu AJAX gửi file mẫu tới server
            $.ajax({
                url: '../KeToan/ExportAfterImportExcel',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                xhrFields: {
                    responseType: 'blob' // Đảm bảo phản hồi là blob
                },
                success: function (response) {
                    if (response.success === false) {
                        CustomSweetAlert_Error(response.message);
                    } else {
                        // Tạo URL và tải file kết quả về
                        let url = window.URL.createObjectURL(new Blob([response]));
                        let a = document.createElement('a');
                        a.href = url;
                        a.download = 'Export.xlsx';
                        document.body.appendChild(a);
                        a.click();
                        a.remove();
                    }
                },
                error: function (xhr) {
                    alert('Error: ' + xhr.responseText);
                }
            });
        })
        .catch(error => console.error('Error fetching the file:', error));
});


$(document).on('click', '#btnImportExcel', function (e) {
    e.preventDefault();
    let agentCodes = document.getElementById('agentCodes').value;
    let phoneNumbers = document.getElementById('phoneNumbers').value;
    let secondarySerials = document.getElementById('secondarySerials').value;
    let excelFile = document.getElementById('excelFile').files[0];
    if (!excelFile) {
        alert("Vui lòng chọn file excel");
        return;
    }
    let formData = new FormData();
    formData.append('agentCodes', agentCodes);
    formData.append('phoneNumbers', phoneNumbers);
    formData.append('secondarySerials', secondarySerials);
    formData.append('excelFile', excelFile);

    $.ajax({
        url: '../KeToan/ExportAfterImportExcel',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        xhrFields: {
            responseType: 'blob' // Ensure response is treated as a blob
        },
        success: function (response) {

            if (response.success == false) {
                CustomSweetAlert_Error(response.message);
            }
            else {
                let url = window.URL.createObjectURL(new Blob([response]));
                let a = document.createElement('a');
                a.href = url;
                a.download = 'Export.xlsx';
                document.body.appendChild(a);
                a.click();
                a.remove();
            }
        },
        error: function (xhr) {
            alert('Error: ' + xhr.responseText);
        }
    });
})


$(document).ready(function () {
    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data("id");

        CustomSweetAlert_Confirm('Bạn thực sự muốn xoá?', 'Sau khi xoá thì chỉ có thể hoàn tác trong vòng 5s').then((result) => {
            if (result.isConfirmed) {
                $('#loadingOverlay').css('display', 'flex');
                $.ajax({
                    url: '../KeToan/DeleteBankAccountDetail',
                    type: 'POST',
                    data: { id: id },
                    success: function (rs) {
                        $('#loadingOverlay').css('display', 'none');
                        if (rs.success) {
                            CustomSweetAlert_Success_ReloadPage(rs.message)
                        } else {
                            CustomSweetAlert_Error(rs.message)
                        }
                    },
                    error: function () {
                        Swal.fire({
                            imageUrl: "/images/fail.png",
                            imageWidth: 100,
                            imageHeight: 100,
                            title: "Thao tác thất bại",
                            text: rs.message,
                            button: "Đóng",
                        });
                    }
                });
            }
        });
    });

    $("#BtnCreate").click(function () {
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            type: "POST",
            url: "../KeToan/CreateBankAccountDetail",
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
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
    });

    $('body').on('click', '.Edit', function () {
        $('#loadingOverlay').css('display', 'flex');
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "../KeToan/EditBankAccountDetail",
            data: {
                id: id
            },
            success: function (response) {
                $('#loadingOverlay').css('display', 'none');
                $('#openPopup').html(response);
                $('#openPopup').modal({
                    backdrop: 'static',
                    keyboard: false,
                    show: true
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
    });

    $('body').on('click', '#btnDeleteSelected', function (e) {
        e.preventDefault();
        var str = "";
        var checkbox = $('#gridItem').find('input:checkbox.cbkItem:checked'); // Lấy tất cả các checkbox đã được chọn trong bảng với id là 'gridItem'
        var i = 0;
        var deletedIds = [];
        checkbox.each(function () {
            debugger;
            var _id = $(this).val(); // Lấy giá trị của checkbox (ID của hàng)
            deletedIds.push(_id); // Thêm ID vào mảng
            if (i === 0) {
                str += _id;
            } else {
                str += "," + _id;
            }
            i++;
        });
        if (str.length > 0) {
            // Sử dụng SweetAlert 2 để hiển thị cửa sổ xác nhận thay vì confirm

            CustomSweetAlert_Confirm('Bạn có chắc là muốn xoá các nội dung này?', 'Sau khi xoá thì chỉ có thể hoàn tác trong vòng 5s').then((result) => {
                if (result.isConfirmed) {
                    $('#loadingOverlay').css('display', 'flex');
                    $.ajax({
                        url: '../KeToan/DeleteBankAccountDetailSelected',
                        type: 'POST',
                        data: { ids: str },
                        success: function (rs) {
                            $('#loadingOverlay').css('display', 'none');
                            if (rs.success) {
                                CustomSweetAlert_Success_ReloadPage(rs.message)
                            }
                        }
                    });
                }
            })
        }
        else {
            CustomSweetAlert_Info("Vui lòng chọn ít nhất 1 dòng")
        }
    });


    $('body').on('click', '#btnUndo', function (e) {
        e.preventDefault();
        $('#loadingOverlay').css('display', 'flex');
        $.ajax({
            url: '../KeToan/UndoDeleteBankAccountDetail', // Địa chỉ URL của action Undo trong Controller
            type: 'POST',
            success: function (rs) {
                $('#loadingOverlay').css('display', 'none');
                if (rs.success) {
                    CustomSweetAlert_Success_ReloadPage(rs.message);
                } else {
                    CustomSweetAlert_Error(rs.message);
                }
            }
        });
    });

})