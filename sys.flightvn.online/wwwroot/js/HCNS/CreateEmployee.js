function previewAvatar() {
    const avatarInput = document.getElementById('avatar');
    const avatarPreview = document.getElementById('avatarPreview');

    // Kiểm tra nếu người dùng đã chọn một tệp
    if (avatarInput.files && avatarInput.files[0]) {
        const file = avatarInput.files[0];
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
        permanentAddress, temporaryAddress, identityCard, placeOfIssue,
        dateOfIssue, department, division, username, password, email,
        accountantCode, workingMode, workingStatus, workPermissions, avatarPreview
    }) {
        this.employeeCode = employeeCode;
        this.lastName = lastName;
        this.firstName = firstName;
        this.gender = gender;
        this.birthDate = birthDate;
        this.personalPhone = personalPhone;
        this.permanentAddress = permanentAddress;
        this.temporaryAddress = temporaryAddress;
        this.identityCard = identityCard;
        this.placeOfIssue = placeOfIssue;
        this.dateOfIssue = dateOfIssue;
        this.department = department;
        this.division = division;
        this.username = username;
        this.password = password;
        this.email = email;
        this.accountantCode = accountantCode;
        this.workingMode = workingMode;
        this.workingStatus = workingStatus;
        this.workPermissions = workPermissions;
        this.avatarPreview = avatarPreview;
    }

    validate() {
        const errors = [];
        if (!this.employeeCode) errors.push("Mã nhân viên là bắt buộc.");
        if (!this.lastName) errors.push("Họ lót là bắt buộc.");
        if (!this.firstName) errors.push("Tên là bắt buộc.");
        if (!this.gender) errors.push("Giới tính là bắt buộc.");
        if (!this.birthDate) errors.push("Ngày sinh là bắt buộc.");
        if (!this.personalPhone) errors.push("SĐT cá nhân là bắt buộc.");
        if (!this.permanentAddress) errors.push("Địa chỉ thường trú là bắt buộc.");
        if (!this.temporaryAddress) errors.push("Địa chỉ tạm trú là bắt buộc.");
        if (!this.identityCard) errors.push("CCCD là bắt buộc.");
        if (!this.placeOfIssue) errors.push("Nơi cấp là bắt buộc.");
        if (!this.dateOfIssue) errors.push("Ngày cấp là bắt buộc.");
        if (!this.department) errors.push("Phòng ban là bắt buộc.");
        if (!this.division) errors.push("Bộ phận là bắt buộc.");
        if (!this.username) errors.push("Tên đăng nhập là bắt buộc.");
        if (!this.password) errors.push("Mật khẩu là bắt buộc.");
        if (!this.email) errors.push("Email là bắt buộc.");
        if (!this.accountantCode) errors.push("Mã kế toán là bắt buộc.");
        if (!this.workingMode) errors.push("Chế độ làm việc là bắt buộc.");
        if (!this.workingStatus) errors.push("Trạng thái làm việc là bắt buộc.");
        if (!this.workPermissions) errors.push("Quyền hạn công việc là bắt buộc.");
        if (!this.avatarPreview) errors.push("Upload avatar là bắt buộc.");
        return errors;
    }
}

function handleFormSubmit() {
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
        identityCard: document.getElementById('identityCard').value,
        placeOfIssue: document.getElementById('placeOfIssue').value,
        dateOfIssue: document.getElementById('dateOfIssue').value,
        department: document.getElementById('department').value,
        division: document.getElementById('division').value,
        username: document.getElementById('username').value,
        password: document.getElementById('password').value,
        email: document.getElementById('email').value,
        accountantCode: document.getElementById('accountantCode').value,
        workingMode: document.getElementById('workingMode').value,
        workingStatus: document.getElementById('workingStatus').value,
        workPermissions: document.getElementById('workPermissions').value,
        avatarPreview: document.getElementById('avatarPreview').value
    });

    // Kiểm tra tính hợp lệ
    const errors = employee.validate();

    if (errors.length > 0) {
        alert("Có lỗi:\n" + errors.join("\n"));
    } else {
        alert("Thông tin nhân viên hợp lệ!");
    }
}