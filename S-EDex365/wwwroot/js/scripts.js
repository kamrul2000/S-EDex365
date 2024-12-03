
(function ($) {
    "use strict";
    $(function () {


        $('.header-menu a[href="#"]').on('click', function (event) {
            event.preventDefault();
        });

        $(".header-menu").menumaker({
            title: '<i class="fa fa-bars"></i>',
            format: "multitoggle"
        });

        var mainHeader = $('.main-header');

        if (mainHeader.length) {
            var sticky = new Waypoint.Sticky({
                element: mainHeader[0]
            });
        }



        var bgImg = $('[data-bg-img]');

        bgImg.css('background', function () {
            return 'url(' + $(this).data('bg-img') + ') center center';
        });


        $('.parsley-validate, .parsley-validate-wrap form').parsley();


        var mainSlider = new Swiper('.main-slider', {
            loop: true,
            spaceBetween: 1,
            speed: 500,
            autoplay: {
                delay: 5000,
            },
            pagination: {
                el: '.main-slider-pagination',
                clickable: true,
            }
        });

        mainSlider.on('slideChangeTransitionStart', function () {
            var $el = $(this.slides[this.activeIndex]),
                $animate = $el.find('[data-animate]');

            $animate.each(function () {
                var $t = $(this);

                $t.removeClass('animated ' + $t.data('animate'));
            });
        }).on('slideChangeTransitionEnd', function () {
            var $el = $(this.slides[this.activeIndex]),
                $animate = $el.find('[data-animate]');

            $animate.each(function () {
                var $el = $(this),
                    $duration = $el.data('duration'),
                    $delay = $el.data('delay');

                $duration = typeof $duration === 'undefined' ? '0.6' : $duration;
                $delay = typeof $delay === 'undefined' ? '0' : $delay;

                $el.addClass('animated ' + $el.data('animate')).css({
                    'animation-duration': $duration + 's',
                    'animation-delay': $delay + 's'
                });
            });
        });


        jQuery('img.svg').each(function () {
            var $img = jQuery(this);
            var imgID = $img.attr('id');
            var imgClass = $img.attr('class');
            var imgURL = $img.attr('src');

            jQuery.get(imgURL, function (data) {
                // Get the SVG tag, ignore the rest
                var $svg = jQuery(data).find('svg');

                // Add replaced image's ID to the new SVG
                if (typeof imgID !== 'undefined') {
                    $svg = $svg.attr('id', imgID);
                }
                // Add replaced image's classes to the new SVG
                if (typeof imgClass !== 'undefined') {
                    $svg = $svg.attr('class', imgClass + ' replaced-svg');
                }

                // Remove any invalid XML tags as per http://validator.w3.org
                $svg = $svg.removeAttr('xmlns:a');

                // Check if the viewport is set, else we gonna set it if we can.
                if (!$svg.attr('viewBox') && $svg.attr('height') && $svg.attr('width')) {
                    $svg.attr('viewBox', '0 0 ' + $svg.attr('height') + ' ' + $svg.attr('width'));
                }

                // Replace image with new SVG
                $img.replaceWith($svg);

            }, 'xml');
        });

        function pageItemHeight() {
            $('.page-image').height(
                function () {
                    return $(this).width();
                }
            );
        }

        pageItemHeight();

        $(window).resize(function () {
            pageItemHeight();
        });
    });


    $(window).on('load', function () {
        $('.isotope').isotope({
            itemSelector: '.isotope > div'
        });
    });


    /* 18: Preloader
    ==============================================*/

    $(window).on('load', function () {

        function removePreloader() {
            var preLoader = $('.preLoader');
            preLoader.fadeOut();
        }
        setTimeout(removePreloader, 250);
    });

    /* 19: Content animation
    ==============================================*/

    $(window).on('load', function () {

        var $animateEl = $('[data-animate]');

        $animateEl.each(function () {
            var $el = $(this),
                $name = $el.data('animate'),
                $duration = $el.data('duration'),
                $delay = $el.data('delay');

            $duration = typeof $duration === 'undefined' ? '0.6' : $duration;
            $delay = typeof $delay === 'undefined' ? '0' : $delay;

            $el.waypoint(function () {
                $el.addClass('animated ' + $name)
                    .css({
                        'animation-duration': $duration + 's',
                        'animation-delay': $delay + 's'
                    });
            }, { offset: '93%' });
        });
    });

})(jQuery);


$("#homeBtn").addClass('activeSection');
$('.nav-item').css('cursor', 'pointer');
$("#homeBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#home-section").offset().top
    }, 1000);

});
$("#payBtn").click(function () {
    window.open('https://www.google.com');
});
$("#aboutBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#about-section").offset().top
    }, 1000);

});
$("#serviceBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#service-section").offset().top
    }, 1000);

});

$("#priceBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#price-section").offset().top
    }, 1000);


});
$("#mediaBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#media-section").offset().top
    }, 1000);
});

$("#contactBtn").click(function () {
    $('html, body').animate({
        scrollTop: $("#contact-section").offset().top
    }, 1000);
});

$('#navbar li').click(function () {
    $(this).addClass('activeSection');
    $(this).parent().children('li').not(this).removeClass('activeSection');
});
$(".list").click(function () {
    const value = $(this).attr('data-filter');
    if (value == 'All') {
        $('.all').show('1500');
    }
    else {
        $('.all').not('.' + value).hide('1500');
        $('.all').filter('.' + value).show('1500');
    }
});
$('.list').click(function () {
    $(this).addClass('active').siblings().removeClass('active');
});

(function ($) { $.fn.menumaker = function (options) { var cssmenu = $(this), settings = $.extend({ title: "Menu", format: "dropdown", sticky: false }, options); return this.each(function () { cssmenu.prepend('<div id="menu-button">' + settings.title + '</div>'); $(this).find("#menu-button").on('click', function () { $(this).toggleClass('menu-opened'); var mainmenu = $(this).next('ul'); if (mainmenu.hasClass('open')) { mainmenu.slideUp('fast').removeClass('open'); } else { mainmenu.slideDown('fast').addClass('open'); if (settings.format === "dropdown") { mainmenu.find('ul').slideDown(); } } }); cssmenu.find('li ul').parent().addClass('has-sub-item'); multiTg = function () { cssmenu.find(".has-sub-item").prepend('<span class="submenu-button"></span>'); cssmenu.find('.submenu-button').on('click', function () { $(this).toggleClass('submenu-opened'); if ($(this).siblings('ul').hasClass('open')) { $(this).siblings('ul').removeClass('open').slideUp('fast'); } else { $(this).siblings('ul').addClass('open').slideDown('fast'); } }); }; if (settings.format === 'multitoggle') multiTg(); else cssmenu.addClass('dropdown'); if (settings.sticky === true) cssmenu.css('position', 'fixed'); resizeFix = function () { if ($(window).width() > 992) { cssmenu.find('ul').show(); } if ($(window).width() < 992) { cssmenu.find('ul').hide().removeClass('open'); } }; resizeFix(); return $(window).on('resize', resizeFix); }); }; })(jQuery);


$('.has-grand-child > a').on('click', function () {
    $(this).parent().siblings().removeClass('has-open');
});
var url = window.location;
$('ul.menu li a').filter(function () {
    return this.href == url;
}).parentsUntil().addClass('has-open');
$('ul.menu li a').filter(function () {
    return this.href == url;
}).parent().addClass('has-active');
$('ul.menu li a').on('click', function () {
    $(this).parent().siblings().removeClass('has-open');
});
