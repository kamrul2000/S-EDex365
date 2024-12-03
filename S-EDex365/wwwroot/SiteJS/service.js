$(document).ready(function () {
    loadServiceTable();
});
function loadServiceTable() {
    dataTable = $("#serviceTable").DataTable({
        "ajax": {
            "url": "/Admin/Service/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "heading", "width": "20%" },
            { "data": "description", "width": "40%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveService("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveService("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditService("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteService("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveService(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/Service/ActiveInActive",
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

function preview() {
    $("#editpreviewServiceImage").attr('src', "");
    previewServiceImg.src = URL.createObjectURL(event.target.files[0]);
    $("#editpreviewServiceImage").attr('src', previewServiceImg.src);
}
function editpreview() {
    editpreviewServiceImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#serviceForm").validate({
    rules: {
        heading: {
            required: true,
        },
        description: {
            required: true,
        },
        files: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please enter Main Heading",
        },
        description: {
            required: "Please enter Discription Header",
        },
        files: {
            required: "Please Select Image",
        }
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/Service/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#serviceModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#serviceForm')[0].reset();
                    $("#previewServiceImg").attr('src', "");
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

function EditService(id) {
    $("#editServiceModal").modal('show');
    $.ajax({
        url: "/Admin/Service/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var heading = res.obj.heading;
                var description = res.obj.description;
                var imageUrl = res.obj.imageUrl;
                var status = res.obj.status;
                if (heading != "") {
                    $("#editserviceheading").val(heading);
                }
                if (description != "") {
                    $("#editservicediscription").val(description);
                }
                if (imageUrl != "") {
                    $("#editimageUrl").val("");
                    $("#editpreviewServiceImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#editServiceStatus").prop('checked', true);
                }
                else {
                    $("#editServiceStatus").prop('checked', false);
                }
                $("#serviceId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editServiceForm").validate({
    rules: {
        heading: {
            required: true,
        },
        description: {
            required: true,
        }
    },
    messages: {
        heading: {
            required: "Please enter Main Heading",
        },
        description: {
            required: "Please enter Discription Header",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/Service/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editServiceModal').modal('hide');
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

function DeleteService(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/Service/Delete",
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