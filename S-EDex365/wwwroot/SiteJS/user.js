
$(document).ready(function () {
    loadUserTable();
});
function loadUserTable() {
    dataTable = $("#userTable").DataTable({
        "ajax": {
            "url": "/Admin/User/GetAllUser"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "15%" },
            { "data": "userName", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "roles", "width": "15%" },
            {
                "data": {
                    id: "id", lockOutEnd: "lockOutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();                 
                    var lockOut = new Date(data.lockOutEnd).getTime();
                    if (lockOut > today) {
                        return `<div class="text-center">
                                    <button class="btn btn-danger" onclick=ActiveInActiveUser("${data.id}") style="cursor:pointer;">InActive</button>
                            </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                    <button class="btn btn-primary" onclick=ActiveInActiveUser("${data.id}") style="cursor:pointer;">Active</button>                                  
                            </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditUser("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteUser("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveUser(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/User/ActiveInActive",
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.success == true) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

$("form#userForm").validate({
    rules: {
        Name: {
            required: true,
        },
        Email: {
            required: true,
            email: true,
        },
        Password: {
            required: true,
            minlength:6
        },
        ConfirmPassword: {
            required: true,
            minlength: 6,
            equalTo: "#pass"
        },
        MobileNo: {
            required: true,
            number:true,
        },
        RoleId: {
            required: true,
        },       
    },
    messages: {
        Name: {
            required: "Please Enter the Role Name",
        },
        Email: {
            required: "Please Enter the Role Email",
            email: "Please Enter a Valid Email Address",
        },
        Password: {
            required: "Please Enter the Role Email",
            minlength: "Please Enter the Password Minimum 6 Length",
        },
        ConfirmPassword: {
            required: "Please Enter the Role Email",
            minlength: "Please Enter the Password Minimum 6 Length",
            equalTo: "Password and Confirm Password Must be Same",
        },
        MobileNo: {
            required: "Please Enter the User Mobile No",
            number:"Please Enter a Valid Mobile No",
        },
        RoleId: {
            required: "Please Enter the User Role",
        },
    },
    submitHandler: function (form) {
        console.log(form.data);
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/User/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#userModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#userForm')[0].reset();
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(res.message);
                }
            },
        }).fail(function (xhr) {
            toastr.error("Something went error");
        });;
    }
});

function EditUser(id) {
    $("#editUserModal").modal('show');
    $.ajax({
        url: "/Admin/User/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var name = res.obj.name;
                var email = res.obj.email;
                var password = res.obj.password;
                var mobile = res.obj.mobileNo;
                var role = res.obj.roleId;
                if (name != "") {
                    $("#name").val(name);
                }
                if (email != "") {
                    $("#email").val(email);
                }
                if (password != "") {
                    $("#editPass").val(password);
                }
                if (password != "") {
                    $("#conPass").val(password);
                }
                if (mobile != "") {
                    $("#mobileNo").val(mobile);
                }
                if (role != "") {
                    $("#roleId option:selected").prop('selected', false);
                    if (role.length > 1) {
                        $(role).each(function (val, text) {
                            $(`#roleId option[value='${text}']`).prop('selected', true);
                        });
                    }
                    else {
                        $(`#roleId option[value='${role[0]}']`).prop('selected', true);
                    }
                }
                $("#userId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editUserForm").validate({
    rules: {
        Name: {
            required: true,
        },
        Email: {
            required: true,
            email: true,
        },
        Password: {
            required: true,
            minlength: 6
        },
        ConfirmPassword: {
            required: true,
            minlength: 6,
            equalTo: "#editPass"
        },
        MobileNo: {
            required: true,
            number: true,
        },
        RoleId: {
            required: true,
        },
    },
    messages: {
        Name: {
            required: "Please Enter the Role Name",
        },
        Email: {
            required: "Please Enter the Role Email",
            email: "Please Enter a Valid Email Address",
        },
        Password: {
            required: "Please Enter the Password",
            minlength: "Please Enter the Password Minimum 6 Length",
        },
        ConfirmPassword: {
            required: "Please Enter the  Password",
            minlength: "Please Enter the Password Minimum 6 Length",
            equalTo: "Password and Confirm Password Must be Same",
        },
        MobileNo: {
            required: "Please Enter the User Mobile No",
            number: "Please Enter a Valid Mobile No",
        },
        RoleId: {
            required: "Please Enter the User Role",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/User/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editUserModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(res.message);
                }
            },
        }).fail(function (xhr) {
            toastr.error("Something went error");
        });
    }
});

function DeleteUser(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/User/Delete",
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.success == true) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}