$(document).ready(function () {
    loadAboutTable();
});
function loadAboutTable() {
    dataTable = $("#aboutTable").DataTable({
        "ajax": {
            "url": '/Admin/AboutUs/GetAll/'
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "heading", "width": "20%" },
            { "data": "discription", "width": "40%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveAbout("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveAbout("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditAbout("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteAbout("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}


function ActiveInActiveAbout(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url:'/Admin/AboutUs/ActiveInActive/',
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
    $("#editpreviewAboutImg").attr('src', "");
    previewAboutImg.src = URL.createObjectURL(event.target.files[0]);
}

function editPreview() {
    $("#previewAboutImg").attr('src', "");
    editpreviewAboutImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#aboutForm").validate({
    rules: {
        heading: {
            required: true,
        },
        discription: {
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
        discription: {
            required: "Please enter Discription Header",
        },
        files: {
            required: "Please Select Image",
        }
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: '/Admin/AboutUs/UpsertPost/',
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#aboutModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#aboutForm')[0].reset();
                    $("#previewAboutImg").attr('src', "");
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

function EditAbout(id) {
    $("#editAboutModal").modal('show');
    $.ajax({
        url: '/Admin/AboutUs/Edit/',
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var heading = res.obj.heading;
                var discription = res.obj.discription;
                var imageUrl = res.obj.imageUrl;
                var status = res.obj.status;
                if (heading != "") {
                    $("#editheading").val(heading);
                }
                if (discription != "") {
                    $("#editdiscription").val(discription);
                }
                if (imageUrl != "") {
                    console.log(imageUrl);
                    $("#editimageUrl").val("");
                    $("#editpreviewAboutImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#editaboutStatus").prop('checked', true);
                }
                else {
                    $("#editaboutStatus").prop('checked', false);
                }
                $("#aboutId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editaboutForm").validate({
    rules: {
        heading: {
            required: true,
        },
        discription: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please enter Heading",
        },
        discription: {
            required: "Please enter Discription",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: '/Admin/AboutUs/UpsertPost/',
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editAboutModal').modal('hide');
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

function DeleteAbout(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: '/Admin/AboutUs/Delete/',
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