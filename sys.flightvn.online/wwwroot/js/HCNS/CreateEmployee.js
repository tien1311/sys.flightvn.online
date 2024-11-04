function previewAvatar() {
    const avatarInput = document.getElementById('avatar');
    const avatarPreview = document.getElementById('avatarPreview');

    // Kiểm tra nếu người dùng đã chọn một tệp
    if (avatarInput.files && avatarInput.files[0]) {
        const file = avatarInput.files[0];

        // Giới hạn kích thước ảnh (ở đây là 2MB)
        const maxSize = 2 * 1024 * 1024; // 2MB in bytes

        // Kiểm tra nếu kích thước ảnh lớn hơn giới hạn
        if (file.size > maxSize) {
            alert("Kích thước ảnh không được vượt quá 2MB!");
            avatarInput.value = "";  // Reset input nếu ảnh quá lớn
            avatarPreview.src = "https://placehold.co/200x200?text=Avatar"; // Đặt lại ảnh mặc định
            return;
        }

        const reader = new FileReader();

        // Đọc file ảnh
        reader.onload = function (e) {
            // Cập nhật src của img để hiển thị ảnh xem trước
            avatarPreview.src = e.target.result;
        };

        // Bắt đầu đọc file dưới dạng Data URL
        reader.readAsDataURL(file);
    }
}

class Employee {
    constructor({
        employeeCode, lastName, firstName, gender, birthDate, personalPhone,
        permanentAddress, temporaryAddress, cccd, issuedBy,
        issueDate, department, division, username, password, email,
        accountantCode, workRegime, workStatus, jobPermissions, avatar
    }) {
        this.employeeCode = employeeCode;
        this.lastName = lastName;
        this.firstName = firstName;
        this.gender = gender;
        this.birthDate = birthDate;
        this.personalPhone = personalPhone;
        this.permanentAddress = permanentAddress;
        this.temporaryAddress = temporaryAddress;
        this.cccd = cccd;
        this.issuedBy = issuedBy;
        this.issueDate = issueDate;
        this.department = department;
        this.division = division;
        this.username = username;
        this.password = password;
        this.email = email;
        this.accountantCode = accountantCode;
        this.workRegime = workRegime;
        this.workStatus = workStatus;
        this.jobPermissions = jobPermissions;
        this.avatar = avatar.files.length;
    }

    validate() {
        var errors = [];
       

        if (!this.lastName) {
            document.getElementById("lastNameError").innerText = "Họ lót là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("lastNameError").innerText = "";
        }
        if (!this.firstName) {
            document.getElementById("firstNameError").innerText = "Tên là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("firstNameError").innerText = "";
        }
        if (this.gender === "0") {
            document.getElementById("genderError").innerText = "Giới tính là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("genderError").innerText = "";
        }
        if (!this.birthDate) {
            document.getElementById("birthDateError").innerText = "Ngày sinh là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("birthDateError").innerText = "";
        }
        if (!this.personalPhone) {
            document.getElementById("personalPhoneError").innerText = "SĐT cá nhân là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("personalPhoneError").innerText = ""
        }
        if (!this.permanentAddress) {
            document.getElementById("permanentAddressError").innerText = "Địa chỉ thường trú là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("permanentAddressError").innerText = ""
        }
        if (!this.temporaryAddress) {
            document.getElementById("temporaryAddressError").innerText = "Địa chỉ tạm trú là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("temporaryAddressError").innerText = "";
        }
        if (!this.cccd) {
            document.getElementById("cccdError").innerText = "CCCD là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("cccdError").innerText = "";
        }
        if (!this.issuedBy) {
            document.getElementById("issuedByError").innerText = "Nơi cấp là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("issuedByError").innerText = "";
        }
        if (!this.issueDate) {
            document.getElementById("issueDateError").innerText = "Ngày cấp là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("issueDateError").innerText = "";
        }
        if (!this.username) {
            document.getElementById("usernameError").innerText = "Tên đăng nhập là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("usernameError").innerText = "";
        }
        if (!this.password) {
            document.getElementById("passwordError").innerText = "Mật khẩu là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("passwordError").innerText = "";
        }
        if (!this.email) {
            document.getElementById("emailError").innerText = "Email là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("emailError").innerText = "";
        }
        if (!this.accountantCode) {
            document.getElementById("accountantCodeError").innerText = "Mã kế toán là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("accountantCodeError").innerText = "";
        }
        if (!this.workRegime) {
            document.getElementById("workRegimeError").innerText = "Chế độ làm việc là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("workRegimeError").innerText = "";
        }
        if (!this.workStatus) {
            document.getElementById("workStatusError").innerText = "Trạng thái làm việc là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("workStatusError").innerText = "";
        }
        if (!this.jobPermissions) {
            document.getElementById("jobPermissionsError").innerText = "Quyền hạn công việc là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("jobPermissionsError").innerText = "";
        }
        if (this.avatar === 0) {
            document.getElementById("avatarPreviewError").innerText = "Upload avatar là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("avatarPreviewError").innerText = "";
        }
        
        if (this.department == 0) {
           
            document.getElementById("departmentError").innerText = "Phòng ban là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("departmentError").innerText = "";
        }
       
        if (this.division == 0) {
            document.getElementById("divisionError").innerText = "Bộ phận là bắt buộc."; errors.push("++");
        }
        else {
            document.getElementById("divisionError").innerText = "";
        }

        return errors;
    }
}


$('body').on('submit', '#employeeForm', function (e) {
    e.preventDefault();
    const employee = new Employee({
        employeeCode: document.getElementById('employeeCode').value,
        lastName: document.getElementById('lastName').value,
        firstName: document.getElementById('firstName').value,
        gender: document.getElementById('gender').value,
        birthDate: document.getElementById('birthDate').value,
        personalPhone: document.getElementById('personalPhone').value,
        permanentAddress: document.getElementById('permanentAddress').value,
        temporaryAddress: document.getElementById('temporaryAddress').value,
        cccd: document.getElementById('cccd').value,
        issuedBy: document.getElementById('issuedBy').value,
        issueDate: document.getElementById('issueDate').value,
        department: document.getElementById('department').value,
        division: document.getElementById('division').value,
        username: document.getElementById('username').value,
        password: document.getElementById('password').value,
        email: document.getElementById('email').value,
        accountantCode: document.getElementById('accountantCode').value,
        workRegime: document.getElementById('workRegime').value,
        workStatus: document.getElementById('workStatus').value,
        jobPermissions: document.getElementById('jobPermissions').value,
        avatar: document.getElementById('avatar')
    });

    // Kiểm tra tính hợp lệ
    const errors = employee.validate();
   
    if (errors.length > 0) {
        CustomSweetAlert_Error("Xin nhập đúng giá trị."); // Example error handling
        return; // Exit the function early if there are errors
    }

    $('#loadingOverlay').css('display', 'flex');
    var submitButton = $('#employeeForm').find('button[type="submit"]');
    submitButton.prop('disabled', true); // Disable the submit button

    var formData = new FormData(this);

    $.ajax({
        type: 'POST',
        url: '../Employee/CreateEmployee',
        data: formData,
        processData: false, // Prevent jQuery from processing the data
        contentType: false, // Prevent jQuery from setting content type
        success: function (response) {
            $('#loadingOverlay').css('display', 'none');
            submitButton.prop('disabled', false);
            console.log(response);
            if (response.success) {
                CustomSweetAlert_Success_ReloadPage(response.message);
            }
            else {
                CustomSweetAlert_Error(response.message);
            }
        }
    })
})
