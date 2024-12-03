$(document).ready(function () {
    loadMediaHeaderTable();
});
function loadMediaHeaderTable() {
    dataTable = $("#mediaHeaderTable").DataTable({
        "ajax": {
            "url": "/Admin/MediaHeader/GetAll"
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
                                       <button class="btn btn-primary" onclick=ActiveInActiveMediaHeader("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveMediaHeader("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditMediaHeader("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteMediaHeader("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveMediaHeader(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaHeader/ActiveInActive",
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

$("form#mediaHeaderForm").validate({
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
            url: "/Admin/MediaHeader/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#mediaHeaderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#mediaHeaderForm')[0].reset();
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

function EditMediaHeader(id) {
    $("#editMediaHeaderModal").modal('show');
    $.ajax({
        url: "/Admin/MediaHeader/Edit",
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
                    $("#editMediaHeaderHeading").val(heading);
                }
                if (description != "") {
                    $("#editMediaHeaderDiscription").val(description);
                }
                if (status == true) {
                    $("#editMediaHeaderStatus").prop('checked', true);
                }
                else {
                    $("#editMediaHeaderStatus").prop('checked', false);
                }
                $("#mediaHeaderId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editMediaHeaderForm").validate({
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
            url: "/Admin/MediaHeader/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editMediaHeaderModal').modal('hide');
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

function DeleteMediaHeader(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaHeader/Delete",
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