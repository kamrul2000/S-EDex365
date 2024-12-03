$(document).ready(function () {
    loadSiteTable();
});
function loadSiteTable() {
    dataTable = $("#siteInfoTable").DataTable({
        "ajax": {
            "url": "/Admin/SiteInfo/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "15%" },
            { "data": "mobileNo1", "width": "10%" },
            { "data": "mobileNo2", "width": "10%" },
            { "data": "email", "width": "15%" },
            { "data": "address", "width": "15%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveSiteInfo("${data.id}") style="cursor:pointer;">Active</button>
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveSiteInfo("${data.id}") style="cursor:pointer;">InActive</button>
                                    </div>`;
                    }
                }, "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditSiteInfo("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>
                                    <button class="btn btn-danger" onclick=DeleteSiteInfo("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveSiteInfo(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/SiteInfo/ActiveInActive",
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
    previewImg.src = URL.createObjectURL(event.target.files[0]);
}
function editpreview() {
    editpreviewImg.src = URL.createObjectURL(event.target.files[0]);
}

$("form#siteInfoForm").validate({
    rules: {
        files: {
            required: true,
        },
        Name: {
            required: true,
        },
        Email: {
            email: true,
        },
        MobileNo1: {
            number: true,
        },
        MobileNo2: {
            number: true,
        }
    },
    messages: {
        files: {
            required: "Please Select Image",
        },
        Name: {
            required: "Please Enter the Name",
        },
        Email: {
            email: "Please Enter a Right Email Address",
        },
        MobileNo1: {
            number: "Please Enter a Phone Number",
        },
        MobileNo2: {
            number: "Please Enter a Phone Number",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/SiteInfo/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#siteInfoModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#siteInfoForm')[0].reset();
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

function EditSiteInfo(id) {
    $("#editSiteInfoModal").modal('show');
    $.ajax({
        url: "/Admin/SiteInfo/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {
                var Name = res.obj.name;
                var MobileNo1 = res.obj.mobileNo1;
                var MobileNo2 = res.obj.mobileNo2;
                var ConDescription = res.obj.conDescription;
                var Email = res.obj.email;
                var Address = res.obj.address;
                var FacebookLink = res.obj.facebookLink;
                var TwitterLink = res.obj.twitterLink;
                var YoutubeLink = res.obj.youtubeLink;
                var imageUrl = res.obj.logoUrl;
                var status = res.obj.status;
                var id = res.obj.id;
                if (Name != "") {
                    $("#name").val(Name);
                }
                if (MobileNo1 != "") {
                    $("#mobileNo1").val(MobileNo1);
                }
                if (MobileNo2 != "") {
                    $("#mobileNo2").val(MobileNo2);
                }
                if (ConDescription != "") {
                    $("#conDescription").val(ConDescription);
                }
                if (Email != "") {
                    $("#email").val(Email);
                }
                if (Address != "") {
                    $("#address").val(Address);
                }
                if (FacebookLink != "") {
                    $("#facebookLink").val(FacebookLink);
                }
                if (YoutubeLink != "") {
                    $("#youtubeLink").val(YoutubeLink);
                }
                if (TwitterLink != "") {
                    $("#twitterLink").val(TwitterLink);
                }
                if (imageUrl != "") {
                    $("#editimageUrl").val("");
                    $("#editpreviewImg").attr('src', imageUrl);
                }
                if (status == true) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#Status").prop('checked', false);
                }
                $("#infoId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editSiteInfoForm").validate({
    rules: {
        Name: {
            required: true,
        },
        Email: {
            email: true,
        },
        MobileNo1: {
            number: true,
        },
        MobileNo2: {
            number: true,
        }
    },
    messages: {
        Name: {
            required: "Please Enter the Name",
        },
        Email: {
            email: "Please Enter a Right Email Address",
        },
        MobileNo1: {
            number: "Please Enter a Phone Number",
        },
        MobileNo2: {
            number: "Please Enter a Phone Number",
        },
    },
    submitHandler: function (form) {
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/SiteInfo/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editSiteInfoModal').modal('hide');
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

function DeleteSiteInfo(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/SiteInfo/Delete",
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
