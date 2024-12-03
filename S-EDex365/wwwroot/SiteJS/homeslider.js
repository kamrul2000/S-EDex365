$(document).ready(function () {
    loadHomeSliderTable();
});
function loadHomeSliderTable() {
    dataTable = $("#homeSliderTable").DataTable({
        "ajax": {
            "url": "/Admin/HomeSlider/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "smallHeading", "width": "20%" },
            { "data": "biggerHeading", "width": "40%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveHomeSlider("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveHomeSlider("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditHomeSlider("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteHomeSlider("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveHomeSlider(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/HomeSlider/ActiveInActive",
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
    $("#editpreviewHomeSliderImage").attr('src', "");
    previewHomeSliderImg.src = URL.createObjectURL(event.target.files[0]);
}
function editpreview() {
    editpreviewHomeSliderImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#homeSliderForm").validate({
    rules: {
        smallHeading: {
            required: true,
        },
        biggerHeading: {
            required: true,
        },
        files: {
            required: true,        
        },
    },
    messages: {
        smallHeading: {
            required: "Please enter Small Heading",
        },
        biggerHeading: {
            required: "Please enter Bigger Heading",
        },
        files: {
            required: "Please Select Image",
        }
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/HomeSlider/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#homeSliderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#homeSliderForm')[0].reset();
                    $("#previewHomeSliderImg").attr('src', "");
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

function EditHomeSlider(id) {
    $("#editHomeSliderModal").modal('show');
    $.ajax({
        url: "/Admin/HomeSlider/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var smallHeading = res.obj.smallHeading;
                var biggerHeading = res.obj.biggerHeading;
                var imageUrl = res.obj.imageUrl;
                var status = res.obj.status;
                if (smallHeading != "") {
                    $("#smallHeading").val(smallHeading);
                }
                if (biggerHeading != "") {
                    $("#biggerHeading").val(biggerHeading);
                }
                if (imageUrl != "") {
                    $("#editimageUrl").val("");
                    $("#editpreviewHomeSliderImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#editServiceStatus").prop('checked', true);
                }
                else {
                    $("#editServiceStatus").prop('checked', false);
                }
                $("#homeSliderId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editHomeSliderForm").validate({
    rules: {
        smallHeading: {
            required: true,
        },
        biggerHeading: {
            required: true,
        },
    },
    messages: {
        smallHeading: {
            required: "Please enter Small Heading",
        },
        biggerHeading: {
            required: "Please enter Bigger Heading",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/HomeSlider/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editHomeSliderModal').modal('hide');
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

function DeleteHomeSlider(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/HomeSlider/Delete",
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