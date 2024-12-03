$(document).ready(function () {
    loadLocationTable();
});
function loadLocationTable() {
    dataTable = $("#locationTable").DataTable({
        "ajax": {
            "url": "/Admin/Location/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "description", "width": "55%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveLocation("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveLocation("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditLocation("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteLocation("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveLocation(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/Location/ActiveInActive",
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

$("form#locationForm").validate({
    rules: {
        description: {
            required: true,
        },
        imageUrl: {
            required: true,
        },
    },
    messages: {
        description: {
            required: "Please Enter the Discription",
        },
        imageUrl: {
            required: "Please Enter the Map Location",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/Location/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#locationModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#locationForm')[0].reset();
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

function EditLocation(id) {
    $("#editLocationModal").modal('show');
    $.ajax({
        url: "/Admin/Location/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var description = res.obj.description;
                var imageUrl = res.obj.imageUrl;
                var status = res.obj.status;
                if (description != "") {
                    $("#description").val(description);
                }
                if (imageUrl != "") {
                    $("#imageUrl").val(imageUrl);
                }
                if (status == true) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#status").prop('checked', false);
                }
                $("#locationId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editLocationForm").validate({
    rules: {
        description: {
            required: true,
        },
        imageUrl: {
            required: true,
        },
    },
    messages: {
        description: {
            required: "Please Enter the Discription",
        },
        imageUrl: {
            required: "Please Enter the Map Location",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/Location/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editLocationModal').modal('hide');
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

function DeleteLocation(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/Location/Delete",
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