$("#gridTable .Chitiet").click(function () {

    /*var index = $('#gridVeHoan tr').index($(this).closest('tr'));*/
    var id = String($(this).closest('tr').attr('id'));

    $.ajax({
        type: "POST",
        url: "../Daily/ChiTietMember",
        data: { khoachinh: id },
        success: function (response) {
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

var count = 1;
console.log(document.getElementById("MemberChildsCount").value);
if (Number(document.getElementById("MemberChildsCount").value) >= 1) {
    count = Number(document.getElementById("MemberChildsCount").value) + 1;
}
function ThemDong(button) {
    var newRow = `
        <div class="col-xs-12 form-group member-child">
            <div class="row">
                <label for="" class="col-sm-4 control-label"></label>
                <div class="col-sm-5">
                    <div class="control-group">
                        <div class="controls">
                            <div class="xdisplay_inputx form-group has-feedback">
                                <input type="text" placeholder="Mã Phụ ${count}" class="form-control" id="member_child_${count}" name="member_child_${count}" value="">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="item form-group col-sm-3">
                    <input class="btn btn-danger" onclick="XoaDong(this);" type="button" value="-">
                </div>
            </div>
        </div>
    `;

    // Thêm dòng mới vào cuối của container
    var container = document.getElementById('MemberChildDetails');
    container.insertAdjacentHTML('beforeend', newRow);
    count++;
}

function XoaDong(button) {
    var currentRow = button.closest('.member-child');
    // Kiểm tra nếu có nhiều hơn một dòng thì mới cho phép xóa
    var container = document.getElementById('MemberChildDetails');
    if (container.children.length > 1) {
        container.removeChild(currentRow);
    } else {
        alert("Không thể xóa, phải có ít nhất một dòng.");
    }
}

$("#UpdatePassword").click(function () {
    var memberid = document.getElementById("memberid").value;
    var password = document.getElementById("password").value;

    $.ajax({
        type: "POST",
        url: "../Daily/UpdatePassword",
        data: { memberid: memberid, password: password },
        success: function (response) {


            if (response == true) {
                alert("Đổi mật khẩu thành công");
            } else {
                alert("Đổi mật khẩu thất bại");
            }

        },
        failure: function (response) {
            console.log(response);
            alert(response);
        },
        error: function (response) {
            console.log(response);
            alert(response);
        }
    });
});

$("#UpdateMember").click(function () {
    var memberid = document.getElementById("memberid").value;
    var khuvuc = document.getElementById("khuvuc").value;
    var company = document.getElementById("company").value;
    var name = document.getElementById("name").value;
    var makh = document.getElementById("makh").value;
    var code = document.getElementById("code").value;
    var email = document.getElementById("email").value;
    var address = document.getElementById("address").value
    var phone = document.getElementById("phone").value;
    var fax = document.getElementById("fax").value;
    var isactive = document.getElementById("isactive").value;
    var isshow = document.getElementById("isshow").value;
    var kinhdoanh = document.getElementById("kinhdoanh").value;
    var ketoan = document.getElementById("ketoan").value;
    var dulich = document.getElementById("dulich").checked;

    var member_childs = [];
    for (var i = 1; i <= count; i++) {
        var input = document.getElementById(`member_child_${i}`);
        if (input) {
            member_childs.push(input.value);
        }
    }

    $.ajax({
        type: "POST",
        url: "../Daily/UpdateMember",
        data: { memberid: memberid, khuvuc: khuvuc, company: company, name: name, makh: makh, code: code, email: email, address: address, phone: phone, fax: fax, isactive: isactive, isshow: isshow, kinhdoanh: kinhdoanh, ketoan: ketoan, dulich: dulich, member_childs: member_childs },
        success: function (response) {
            if (response == true) {
                alert("Lưu thành công");
            } else {
                alert("Lưu thất bại");
            }

        },
        failure: function (response) {
            console.log(response);
            alert(response);
        },
        error: function (response) {
            console.log(response);
            alert(response);
        }
    });
});

function CheckActiveMember(obj) {
    var table = document.getElementById("gridTable");
    var index = obj.parentNode.parentNode.rowIndex;
    var active = "";
    var n = table.rows[index].cells[6].getElementsByTagName("input")[0].checked;
    var z = table.getElementsByTagName('tr')[index].id;


    if (n == true) {
        active = 1;
    }
    else {
        active = 0;
    }
    $.ajax({
        type: "POST",
        url: "../Daily/ActiveMember",
        data: {
            Active: active,
            RowID: z
        },
        success: function (response) {
            if (response == true) {
                document.getElementById("ActiveUser").setAttribute("checked", "checked");
                alert("Ngừng kích hoạt thành công");
                return;
            } else {
                document.getElementById("ActiveUser").removeAttribute("checked");
                alert("Kích hoạt thành công");
                return;
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
function Resetpass(obj) {
    var table = document.getElementById("gridTable");
    var index = obj.parentNode.parentNode.rowIndex;
    var z = table.getElementsByTagName('tr')[index].id;

    $.ajax({
        type: "POST",
        url: "../Daily/ResetPass",
        data: {
            RowID: z
        },
        success: function (response) {
            if (response == true) {
                alert("Khôi phục thành công");
                return;
            } else {
                alert("Khôi phục thất bại ");
                return;
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