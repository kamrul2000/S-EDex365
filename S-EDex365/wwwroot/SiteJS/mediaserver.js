$(document).ready(function () {
    loadMediaServerTable();
});
function loadMediaServerTable() {
    dataTable = $("#mediaServerTable").DataTable({
        "ajax": {
            "url": "/Admin/MediaServer/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            { "data": "mediaCategory.name", "width": "15%" },
            { "data": "linkUrl", "width": "25%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveMediaServer("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveMediaServer("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditMediaServer("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteMediaServer("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}


function ActiveInActiveMediaServer(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaServer/ActiveInActive",
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
    previewMediaServerImg.src = URL.createObjectURL(event.target.files[0]);
}
function editpreview() {
    editpreviewMediaServerImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#mediaServerForm").validate({
    rules: {
        name: {
            required: true,
        },
        mediaCategoryId: {
            required: true,
        },
        linkUrl: {
            required: true,
        },
        files: {
            required: true,
        },
    },
    messages: {
        name: {
            required: "Please enter Name",
        },
        mediaCategoryId: {
            required: "Please enter Media Category",
        },
        linkUrl: {
            required: "Please enter Media Server Url",
        },
        files: {
            required: "Please Select Image",
        }
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/MediaServer/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#mediaServerModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#mediaServerForm')[0].reset();
                    $("#previewMediaServerImg").attr('src', "");
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
function EditMediaServer(id) {
    $("#editMediaServerModal").modal('show');
    $.ajax({
        url: "/Admin/MediaServer/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var name = res.obj.name;
                var linkurl = res.obj.linkUrl;
                var imageUrl = res.obj.imageUrl;
                var mediaCategoryId = res.obj.mediaCategoryId;
                var status = res.obj.status;
                if (name != "") {
                    $("#name").val(name);
                }
                if (linkurl != "") {
                    $("#linkurl").val(linkurl);
                }
                if (mediaCategoryId != "") {
                    $(`#categoryId option[value='${mediaCategoryId}']`).prop('selected', true);
                }
                if (imageUrl != "") {
                    $("#editImageUrl").val("");
                    $("#editpreviewMediaServerImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#mediaServerStatus").prop('checked', true);
                }
                else {
                    $("#mediaServerStatus").prop('checked', false);
                }
                $("#mediaServerId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editMediaServerForm").validate({
    rules: {
        name: {
            required: true,
        },
        mediaCategoryId: {
            required: true,
        },
        linkUrl: {
            required: true,
        },
    },
    messages: {
        name: {
            required: "Please enter Name",
        },
        mediaCategoryId: {
            required: "Please enter Media Category",
        },
        linkUrl: {
            required: "Please enter Media Server Url",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/MediaServer/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editMediaServerModal').modal('hide');
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

function DeleteMediaServer(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/MediaServer/Delete",
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