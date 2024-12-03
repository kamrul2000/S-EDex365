$("form#passForm").validate({
    rules: {
        OldPassword: {
            required: true,
            minlength: 6,
        },
        NewPassword: {
            required: true,
            minlength: 6,
        },
        ConfirmNewPassword: {
            required: true,
            minlength: 6,
            equalTo: "#newPassword"
        },
    },
    messages: {
        OldPassword: {
            required: "Please Enter the Password",
            minlength: "Please Enter the Password Minimum 6 Length",
        },
        NewPassword: {
            required: "Please Enter the Password",
            minlength: "Please Enter the Password Minimum 6 Length",
        },
        ConfirmNewPassword: {
            equired: "Please Enter the  Password",
            minlength: "Please Enter the Password Minimum 6 Length",
            equalTo: "Password and Confirm Password Must be Same",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Account/ChangePassword",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#exampleModalCenter').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                }
                else {
                    toastr.error(res.message);
                }
                $('#passForm')[0].reset();
            },
        }).fail(function (xhr) {
            toastr.error("Something went error");
        });;
    }
});

var message = $("#loginBox").text();
if (message != "") {
    toastr.success(message);
}