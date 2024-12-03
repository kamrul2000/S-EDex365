$(document).ready(function () {
    loadPackageTable();
});
function loadPackageTable() {
    dataTable = $("#packageTable").DataTable({
        "ajax": {
            "url": "/Admin/Package/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "10%" },
            { "data": "description", "width": "10%" },
            { "data": "mbps", "width": "5%" },
            { "data": "nix", "width": "5%" },
            { "data": "bdix", "width": "10%" },
            { "data": "otconu", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActivePackage("${data.id}") style="cursor:pointer;">Active</button>
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActivePackage("${data.id}") style="cursor:pointer;">InActive</button>
                                    </div>`;
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                    <button class="btn btn-success" onclick=EditPackage("${data}") style="cursor:pointer;"><i class="fas fa-edit"></i></button>
                                    <button class="btn btn-danger" onclick=DeletePackage("${data}")><i class="fas fa-trash-alt"></i></button>
                                </div>`;
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActivePackage(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/Package/ActiveInActive",
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

$("form#packageForm").validate({
    rules: {
        name: {
            required: true,
        },
        description: {
            required: true,
        },
        mbps: {
            required: true,
            range: [1, 1000],
        },
        nix: {
            required: true,
            range: [1, 1000],
        },
        bdix: {
            required: true,
            range: [1, 1000],
        },
        otconu: {
            required: true,
            range: [1, 100000],
        },
        price: {
            required: true,
            range: [1, 100000],
        },
    },
    messages: {
        name: {
            required: "Please Enter the Name",
        },
        description: {
            required: "Please Enter the Description",
        },
        mbps: {
            required: "Please Enter the MBPS",
            range: "Please Enter the MBPS betwen 1 to 1000",
        },
        nix: {
            required: "Please Enter the NIX",
            range: "Please Enter the NIX betwen 1 to 1000",
        },
        bdix: {
            required: "Please Enter the BDIX",
            range: "Please Enter the BDIX betwen 1 to 1000",
        },
        otconu: {
            required: "Please Enter the OCT+ONU ",
            range: "Please Enter the OTC+ONU Price betwen 1 to 100000",
        },
        price: {
            required: "Please Enter the Price",
            range: "Please Enter the Price betwen 1 to 100000",
        },
    },
    submitHandler: function (form) {
        var formData = new FormData(form);
        $.ajax({
            url: "/Admin/Package/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function (res) {
                $('#packageModal').modal('hide');
                if (res.success == true) {
                    toastr.success(res.message);
                    $('#packageForm')[0].reset();
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

function EditPackage(id) {
    $("#editpackageModal").modal('show');
    $.ajax({
        url: "/Admin/Package/Edit",
        type: "GET",
        data: {
            id: id
        },
        success: function (res) {
            if (res.success == true) {

                var name = res.obj.name;
                var description = res.obj.description;
                var mbps = res.obj.mbps;
                var nix = res.obj.nix;
                var bdix = res.obj.bdix;
                var otconu = res.obj.otconu;
                var price = res.obj.price;
                var status = res.obj.status;
                var id = res.obj.id;
                if (name != "") {
                    $("#name").val(name);
                }
                if (description != "") {
                    $("#description").val(description);
                }
                if (mbps != "") {
                    $("#mbps").val(mbps);
                }
                if (nix != "") {
                    $("#nix").val(nix);
                }
                if (bdix != "") {
                    $("#bdix").val(bdix);
                }
                if (otconu != "") {
                    $("#otconu").val(otconu);
                }
                if (price != "") {
                    $("#price").val(price);
                }
                if (status == true) {
                    $("#status").prop('checked', true);
                }
                else {
                    $("#status").prop('checked', false);
                }
                $("#packageId").val(id);
            }
            else {
                toastr.error(res.message);
            }
        },
    })
}

$("form#editpackageForm").validate({
    rules: {
        name: {
            required: true,
        },
        description: {
            required: true,
        },
        mbps: {
            required: true,
            range: [1, 1000],
        },
        nix: {
            required: true,
            range: [1, 1000],
        },
        bdix: {
            required: true,
            range: [1, 1000],
        },
        otconu: {
            required: true,
            range: [1, 100000],
        },
        price: {
            required: true,
            range: [1, 100000],
        },
    },
    messages: {
        name: {
            required: "Please Enter the Name",
        },
        description: {
            required: "Please Enter the Description",
        },
        mbps: {
            required: "Please Enter the MBPS",
            range: "Please Enter the MBPS betwen 1 to 1000",
        },
        nix: {
            required: "Please Enter the NIX",
            range: "Please Enter the NIX betwen 1 to 1000",
        },
        bdix: {
            required: "Please Enter the BDIX",
            range: "Please Enter the BDIX betwen 1 to 1000",
        },
        otconu: {
            required: "Please Enter the OCT+ONU ",
            range: "Please Enter the OTC+ONU Price betwen 1 to 100000",
        },
        price: {
            required: "Please Enter the Price",
            range: "Please Enter the Price betwen 1 to 100000",
        },
    },
    submitHandler: function (form) {
        console.log(form);
        var editformData = new FormData(form);
        $.ajax({
            url: "/Admin/Package/UpsertPost",
            type: "POST",
            processData: false,
            contentType: false,
            data: editformData,
            success: function (res) {
                $('#editpackageModal').modal('hide');
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

function DeletePackage(id) {
    swal({
        title: "Are you sure that you want to delete this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "GET",
                url: "/Admin/Package/Delete",
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
