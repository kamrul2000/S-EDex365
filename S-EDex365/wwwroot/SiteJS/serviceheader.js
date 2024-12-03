$(document).ready(function () {
    loadServiceHeaderTable();
});
function loadServiceHeaderTable() {
    dataTable = $("#serviceHeaderTable").DataTable({
        "ajax": {
            "url": "/Admin/ServiceHeader/GetAll"
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
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveServceHeader("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveServceHeader("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditServiceHeader("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteServiceHeader("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveServceHeader(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/ServiceHeader/ActiveInActive",
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

$("form#serviceHeaderForm").validate({
    rules: {
        heading: {
            required: true,
        },
        description: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please Enter the Heading",
        },
        description: {
            required: "Please Enter the Discription",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/ServiceHeader/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#serviceHeaderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#serviceHeaderForm')[0].reset();
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

function EditServiceHeader(id) {
    $("#editServiceHeaderModal").modal('show');
    $.ajax({
        url: "/Admin/ServiceHeader/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var heading = res.obj.heading;
                var description = res.obj.description;
                var status = res.obj.status;
                if (heading != "") {
                    $("#editServiceHeaderHeading").val(heading);
                }
                if (description != "") {
                    $("#editServiceHeaderDiscription").val(description);
                }
                if (status == true) {
                    $("#editServiceHeaderStatus").prop('checked', true);
                }
                else {
                    $("#editServiceHeaderStatus").prop('checked', false);
                }
                $("#serviceHeaderId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editServiceHeaderForm").validate({
    rules: {
        heading: {
            required: true,
        },
        description: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please Enter the Heading",
        },
        description: {
            required: "Please Enter the Discription",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/ServiceHeader/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editServiceHeaderModal').modal('hide');
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

function DeleteServiceHeader(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/ServiceHeader/Delete",
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
