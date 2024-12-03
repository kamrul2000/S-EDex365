var message = $("#loginBox").text();
if (message != "") {
    toastr.error(message);
}
var message = $("#logoutBox").text();
if (message != "") {
    toastr.success(message);
}
