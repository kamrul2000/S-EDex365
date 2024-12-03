$(document).ready(function () {
    loadContactHeaderTable();
});
function loadContactHeaderTable() {
    dataTable = $("#contactHeaderTable").DataTable({
        "ajax": {
            "url": "/Admin/ContactHeader/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "heading", "width": "20%" },
            { "data": "paragraph", "width": "20%" },
            { "data": "btnTxt", "width": "20%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveContactHeader("${data.id}") style="cursor:pointer;">Active</button>                                                                         
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveContactHeader("${data.id}") style="cursor:pointer;">InActive</button> 
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditContactHeader("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>                                  
                                    <button class="btn btn-danger" onclick=DeleteContactHeader("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveContactHeader(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/ContactHeader/ActiveInActive",
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

$("form#contactHeaderForm").validate({
    rules: {
        heading: {
            required: true,
        },
        paragraph: {
            required: true,
        },
        btnTxt: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please Enter the Heading",
        },
        paragraph: {
            required: "Please Enter the Paragraph",
        },
        btnTxt: {
            required: "Please Enter the Button Text",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/ContactHeader/UpsertPost",               
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#contactHeaderModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#contactHeaderForm')[0].reset();
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

function EditContactHeader(id) {
    $("#editContactHeaderModal").modal('show');
    $.ajax({
        url: "/Admin/ContactHeader/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                console.log(res.obj);
                var heading = res.obj.heading;
                var paragraph = res.obj.paragraph;
                var btnText = res.obj.btnTxt;
                var status = res.obj.status;
                if (heading != "") {
                    $("#heading").val(heading);
                }
                if (paragraph != "") {
                    $("#paragraph").val(paragraph);
                }
                if (btnText != "") {
                    $("#btnTxt").val(btnText);
                }
                if (status == true) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#status").prop('checked', false);
                }
                $("#contactHeaderId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editContactHeaderForm").validate({
    rules: {
        heading: {
            required: true,
        },
        paragraph: {
            required: true,
        },
        btnTxt: {
            required: true,
        },
    },
    messages: {
        heading: {
            required: "Please Enter the Heading",
        },
        paragraph: {
            required: "Please Enter the Paragraph",
        },
        btnTxt: {
            required: "Please Enter the Button Text",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/ContactHeader/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editContactHeaderModal').modal('hide');
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

function DeleteContactHeader(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/ContactHeader/Delete",
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