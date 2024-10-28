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
        accountantCode, workRegime, workStatus, jobPermissions, avatarPreview
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
        this.avatarPreview = avatarPreview;
    }

    validate() {
        const errors = [];

        if (!this.employeeCode) document.getElementById("employeeCodeError").innerText = "Mã nhân viên là bắt buộc."; errors.push("++");
        if (!this.lastName) document.getElementById("lastNameError").innerText = "Họ lót là bắt buộc."; errors.push("++");
        if (!this.firstName) document.getElementById("firstNameError").innerText = "Tên là bắt buộc."; errors.push("++");
        if (this.gender === "0")  document.getElementById("genderError").innerText = "Giới tính là bắt buộc."; errors.push("++");
        if (!this.birthDate) document.getElementById("birthDateError").innerText = "Ngày sinh là bắt buộc."; errors.push("++");
        if (!this.personalPhone) document.getElementById("personalPhoneError").innerText = "SĐT cá nhân là bắt buộc."; errors.push("++");
        if (!this.permanentAddress) document.getElementById("permanentAddressError").innerText = "Địa chỉ thường trú là bắt buộc."; errors.push("++");
        if (!this.temporaryAddress) document.getElementById("temporaryAddressError").innerText = "Địa chỉ tạm trú là bắt buộc."; errors.push("++");
        if (!this.cccd) document.getElementById("cccdError").innerText = "CCCD là bắt buộc."; errors.push("++");
        if (!this.issuedBy) document.getElementById("issuedByError").innerText = "Nơi cấp là bắt buộc."; errors.push("++");
        if (!this.issueDate) document.getElementById("issueDateError").innerText = "Ngày cấp là bắt buộc."; errors.push("++");
        
        if (!this.username) document.getElementById("usernameError").innerText = "Tên đăng nhập là bắt buộc."; errors.push("++");
        if (!this.password) document.getElementById("passwordError").innerText = "Mật khẩu là bắt buộc."; errors.push("++");
        if (!this.email) document.getElementById("emailError").innerText = "Email là bắt buộc."; errors.push("++");
        if (!this.accountantCode) document.getElementById("accountantCodeError").innerText = "Mã kế toán là bắt buộc."; errors.push("++");
        if (!this.workRegime) document.getElementById("workRegimeError").innerText = "Chế độ làm việc là bắt buộc."; errors.push("++");
        if (!this.workStatus) document.getElementById("workStatusError").innerText = "Trạng thái làm việc là bắt buộc."; errors.push("++");
        if (!this.jobPermissions) document.getElementById("jobPermissionsError").innerText = "Quyền hạn công việc là bắt buộc."; errors.push("++");
        if (!this.avatarPreview) document.getElementById("avatarPreviewError").innerText = "Upload avatar là bắt buộc."; errors.push("++");
        if (this.department === 0) document.getElementById("departmentError").innerText = "Phòng ban là bắt buộc."; errors.push("++");
        if (!this.division === 0) document.getElementById("divisionError").innerText = "Bộ phận là bắt buộc."; errors.push("++");

        return errors;
    }
}

function handleFormSubmit(event) {
    console.log(document.getElementById("gender").value);
    // Ngăn chặn gửi form
    event.preventDefault();
    // Lấy giá trị từ các input
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
        avatarPreview: document.getElementById('avatarPreview').value
    });

    // Kiểm tra tính hợp lệ
    const errors = employee.validate();
    if (errors.length <= 0) {
        document.getElementById("employeeForm").submit();
    }
}