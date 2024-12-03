$(document).ready(function () {
    loadRoleTable();
});
function loadRoleTable() {
    dataTable = $("#roleTable").DataTable({
        "ajax": {
            "url": "/Admin/Role/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "40%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveRole("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveRole("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditRole("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteRole("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "30%"
            }
        ]
    });
}

function ActiveInActiveRole(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/Role/ActiveInActive",
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

$("form#roleForm").validate({
    rules: {
        Name: {
            required: true,
        },
    },
    messages: {
        Name: {
            required: "Please Enter the Role Name",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/Role/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#roleModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#roleForm')[0].reset();
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

function EditRole(id) {
    $("#editRoleModal").modal('show');
    $.ajax({
        url: "/Admin/Role/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var name = res.obj.name;
                var status = res.obj.status;
                if (name != "") {
                    $("#name").val(name);
                }
                if (status == true) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#status").prop('checked', false);
                }
                $("#roleId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editRoleForm").validate({
    rules: {
        Name: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please Enter the Role Name",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/Role/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editRoleModal').modal('hide');
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

function DeleteRole(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/Role/Delete",
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