$(document).ready(function () {
    loadMenuTable();
});
function loadMenuTable() {
    dataTable = $("#menuTable").DataTable({
        "ajax": {
            "url": "/Admin/Menu/GetAll"
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "65%" },
            {
                "data": {
                    id: "id", status: "status"
                },
                "render": function (data) {
                    ;
                    if (data.status == true) {
                        return `<div class="text-center">
                                       <button class="btn btn-primary" onclick=ActiveInActiveMenu("${data.id}") style="cursor:pointer;">Active</button>
                                    </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                        <button class="btn btn-danger" onclick=ActiveInActiveMenu("${data.id}") style="cursor:pointer;">InActive</button>
                                    </div>`;
                    }
                }, "width": "20%"
            }
        ]
    });
}

function ActiveInActiveMenu(id) {
    swal({
        title: "Are you sure that you want to do this",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willActive) => {
        if (willActive) {
            $.ajax({
                type: "GET",
                url: "/Admin/Menu/ActiveInActive",
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