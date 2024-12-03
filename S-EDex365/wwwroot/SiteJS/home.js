var menuId = $('.nav-item').map(function () {
    return $(this).attr('id');
});
var menuList = [];
$(menuId).each(function (val, text) {
    menuList.push(text);
});
$('section').each(function () {
    if (menuList.indexOf($(this).attr("data-section")) == -1) {
        $(this).addClass('d-none');
    }
});

$(document).ready(function () {
    getData();
});
function getData() {
    $.ajax({
        url:'/Home/GetMediaData/',
        type: "GET",
    }).done(function (response) {
        $("#mediaList").html(response);
    })
}
$(".list").click(function () {
    $.ajax({
        url: '/Home/GetMediaData/',
        type: "GET",
        data: {
            id: $(this).attr("data-id")
        },
    }).done(function (response) {
        $("#mediaList").html(response);
    });
})


$(window).on('load',function () {
    
    var maxService = Math.max.apply(null, $(".single-service").map(function ()
    {
        return $(this).height();
    }).get());
    var maxMedia = Math.max.apply(null, $(".mediaCard").map(function ()
    {
        return $(this).height();
    }).get());
    var maxPackage = Math.max.apply(null, $(".single-package").map(function ()
    {
        return $(this).height();
    }).get());
    $(".single-service").height(maxService);
    $(".single-package").height(maxPackage);
    $(".mediaCard").height(maxMedia);
})
