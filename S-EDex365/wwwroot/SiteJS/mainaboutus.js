$(document).ready(function () {
    loadMainTable();
});
function loadMainTable() {
    dataTable = $("#mainTable").DataTable({
        "ajax": {
            "url": "/Admin/MainAboutUs/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "mainHeading", "width": "15%" },
            { "data": "discriptionHeader", "width": "20%" },
            { "data": "seconderyDescription", "width": "20%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActive("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActive("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditAboutUs("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteAboutUs("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActive(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/MainAboutUs/ActiveInActive",
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
    $("#editpreviewImg").attr('src', "");
    previewImg.src = URL.createObjectURL(event.target.files[0]);
}
function editPreview() {
    $("#previewImg").attr('src', "");
    editpreviewImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#headerForm").validate({
    rules: {
        mainHeading: {
            required: true,
        },
        discriptionHeader: {
            required: true,
        },
        seconderyDescription: {
            required: true,
        },
        files: {
            required: true,
        },
    },
    messages: {
        mainHeading: {
            required: "Please enter Main Heading",
        },
        discriptionHeader: {
            required: "Please enter Discription Header",
        },
        seconderyDescription: {
            required: "Please Select Secondery Description",
        },
        files: {
            required: "Please Select Image",
        }
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/MainAboutUs/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#aboutHeaderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#headerForm')[0].reset();
                    $("#previewImg").attr('src', "");
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(res.message);
                }
            },
        }).fail(function (xhr) {
            toastr.error("Something went error");
        });
        $("#submitAboutheader").removeAttr("data-dismiss");
    }
});

function EditAboutUs(id) {
    $("#editAboutHeaderModal").modal('show');
    $.ajax({
        url: "/Admin/MainAboutUs/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var mainHeading = res.obj.mainHeading;
                var discriptionHeader = res.obj.discriptionHeader;
                var seconderyDescription = res.obj.seconderyDescription;
                var imageUrl = res.obj.imageUrl;
                var status = res.obj.status;
                if (mainHeading != "") {
                    $("#editmainHeading").val(mainHeading);
                }
                if (discriptionHeader != "") {
                    $("#editdiscriptionHeader").val(discriptionHeader);
                }
                if (seconderyDescription != "") {
                    $("#editseconderyDescription").val(seconderyDescription);
                }
                if (imageUrl != "") {                                      
                    $("#editpreviewImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#editaboutHeaderStatus").prop('checked', true);
                }
                else {
                    $("#editaboutHeaderStatus").prop('checked', false);
                }
                $("#aboutUsId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editheaderForm").validate({
    rules: {
        mainHeading: {
            required: true,
        },
        discriptionHeader: {
            required: true,
        },
        seconderyDescription: {
            required: true,
        },
    },
    messages: {
        mainHeading: {
            required: "Please enter Main Heading",
        },
        discriptionHeader: {
            required: "Please enter Discription Header",
        },
        seconderyDescription: {
            required: "Please Select Secondery Description",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/MainAboutUs/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editAboutHeaderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#headerForm')[0].reset();
                    $("#previewImg").attr('src', "");
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

function DeleteAboutUs(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/MainAboutUs/Delete",
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