$(document).ready(function () {
    loadMediaCategoryTable();
});
function loadMediaCategoryTable() {
    dataTable = $("#mediaCategoryTable").DataTable({
        "ajax": {
            "url": "/Admin/MediaCategory/GetAll"
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
                                       <button class="btn btn-primary" onclick=ActiveInActiveMediaCategory("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveMediaCategory("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditMediaCategory("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteMediaCategory("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "30%"
            }
        ]
    });
}

function ActiveInActiveMediaCategory(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaCategory/ActiveInActive",
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

$("form#mediaCategoryForm").validate({
    rules: {
        name: {
            required: true,
        },
    },
    messages: {
        name: {
            required: "Please Enter the Name",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/MediaCategory/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#mediaCategoryModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#mediaCategoryForm')[0].reset();
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

function EditMediaCategory(id) {
    $("#editMediaCategoryModal").modal('show');
    $.ajax({
        url: "/Admin/MediaCategory/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var name = res.obj.name;
                var status = res.obj.status;
                if (name != "") {
                    $("#mediaCategoryName").val(name);
                }
                if (status == true) {
                    $("#mediaCategoryStatus").prop('checked', true);
                }
                else {
                    $("#mediaCategoryStatus").prop('checked', false);
                }
                $("#mediaCategoryId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editMediaCategoryForm").validate({
    rules: {
        name: {
            required: true,
        },
    },
    messages: {
        name: {
            required: "Please Enter the Name",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/MediaCategory/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editMediaCategoryModal').modal('hide');
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

function DeleteMediaCategory(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaCategory/Delete",
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