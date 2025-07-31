(function() {

	/* ====Sticky Navbar Start==== */
	$(function(){
		var myNav = $('.navbar');
		$(window).scroll(function(){
			if($(window).scrollTop() <= 50){
				myNav.removeClass('sticky-nav');
			}else{
				myNav.addClass('sticky-nav');
			}
		})
	})
	/* ====Sticky Navbar End==== */

	/* ====Button (Scrolling Bottom to Top) Start==== */
	var mybtn = document.getElementById('scroll-bottom-top');
	window.onscroll = function() {
		scrollFunction();
	};

	function scrollFunction() {
		if (
			document.body.scrollTop > 50 ||
			document.documentElement.scrollTop > 50
			) {
			mybtn.style.opacity = '1';
	} 
	else {
		mybtn.style.opacity = '0';
	}
}
mybtn.addEventListener('click', topFunction);

function topFunction() {
	document.body.scrollTop = 0;
	document.documentElement.scrollTop = 0;
}
/* ====Button (Scrolling Bottom to Top) End==== */


/* ====Course Item Slider Start==== */
$('#course-item-slider').slick({
	dots: false,
	infinite: true,
	speed: 1000,
	autoplay: true,
	autoplaySpeed: 1500,
	autoplayHoverPause: true,
	slidesToShow: 5,
	slidesToScroll: 1,
	responsive: [
	{
		breakpoint: 1024,
		settings: {
			slidesToShow: 3,
			slidesToScroll: 1,
			infinite: true,
			dots: true
		}
	},
	{
		breakpoint: 600,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	},
	{
		breakpoint: 480,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	}
    ]
});

/* ====Course Item Slider End==== */

/* ====Course Slider Start==== */
jQuery('#web-software-course-slider').slick({
  dots: true,
  arrows: true,
  infinite: true,
  speed: 1000,
  slidesToShow: 3,
  slidesToScroll: 1,
  autoplay: false,
  autoplaySpeed: 1500,
  autoplayHoverPause: true,
  responsive: [
  {
    breakpoint: 1024,
    settings: {
      slidesToShow: 3,
      slidesToScroll: 1,
      infinite: true,
      dots: true
    }
  },
  {
    breakpoint: 600,
    settings: {
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      autoplay: false
    }
  },
  {
    breakpoint: 480,
    settings: {
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      autoplay: false
    }
  }
    ]
});
jQuery('#day-long-course-slider').slick({
  dots: true,
  arrows: true,
  infinite: true,
  speed: 1000,
  slidesToShow: 3,
  slidesToScroll: 1,
  autoplay: false,
  autoplaySpeed: 1500,
  autoplayHoverPause: true,
  responsive: [
  {
    breakpoint: 1024,
    settings: {
      slidesToShow: 3,
      slidesToScroll: 1,
      infinite: true,
      dots: true
    }
  },
  {
    breakpoint: 600,
    settings: {
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      autoplay: false
    }
  },
  {
    breakpoint: 480,
    settings: {
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      autoplay: false
    }
  }
    ]
});
/* ====Course Slider End==== */


/* ====Our All Popular Course Start==== */
$(".course-topic-tab h4").on("click", function(){
	$(".course-topic-tab h4").removeClass("active");
	$(this).addClass("active");
});


$('#course-tab-slider').slick({
	dots: false,
	infinite: true,
	speed: 1000,
	slidesToShow: 4,
	slidesToScroll: 1,
	responsive: [
	{
		breakpoint: 1024,
		settings: {
			slidesToShow: 3,
			slidesToScroll: 1,
			infinite: true,
			dots: true
		}
	},
	{
		breakpoint: 600,
		settings: {
			slidesToShow: 2,
			slidesToScroll: 2
		}
	},
	{
		breakpoint: 480,
		settings: {
			slidesToShow: 2,
			slidesToScroll: 1
		}
	}
    ]
});


$('#all-course-slider').slick({
	dots: false,
	infinite: true,
	speed: 1000,
	slidesToShow: 3,
	slidesToScroll: 1,
	responsive: [
	{
		breakpoint: 1024,
		settings: {
			slidesToShow: 3,
			slidesToScroll: 1,
			infinite: true,
			dots: true
		}
	},
	{
		breakpoint: 600,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	},
	{
		breakpoint: 480,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	}
    ]
});


$("#all-course").click(function(){
	$(".cyber-security, .digital-marketing, .web-software, .project-management-professional, .design-and-multimedia, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".all-course").show();
});

$("#digital-marketing").click(function(){
	$(".all-course, .cyber-security, .web-software, .project-management-professional, .design-and-multimedia, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".digital-marketing").fadeIn(1000);
});

$("#cyber-security").click(function(){
	$(".all-course, .digital-marketing, .web-software, .project-management-professional, .design-and-multimedia, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".cyber-security").fadeIn(1000);
});

$("#web-software").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .project-management-professional, .design-and-multimedia, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".web-software").fadeIn(1000);
});

$("#project-management-professional").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .web-software, .design-and-multimedia, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".project-management-professional").fadeIn(1000);
});

$("#design-and-multimedia").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .web-software, .project-management-professional, .mobile-app-development, .spoken-english, .networking-technology").hide();
	$(".design-and-multimedia").fadeIn(1000);
});

$("#mobile-app-development").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .web-software, .design-and-multimedia, .project-management-professional, .spoken-english, .networking-technology").hide();
	$(".mobile-app-development").fadeIn(1000);
});

$("#spoken-english").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .web-software, .design-and-multimedia, .project-management-professional, .mobile-app-development, .networking-technology").hide();
	$(".spoken-english").fadeIn(1000);
});

$("#networking-technology").click(function(){
	$(".all-course, .digital-marketing, .cyber-security, .web-software, .design-and-multimedia, .project-management-professional, .mobile-app-development, .spoken-english").hide();
	$(".networking-technology").fadeIn(1000);
});
/* ====Our All Popular Course End==== */


/* ====Our Partner Area Button Active Start==== */
$(document).on('click', '.btn-area ul li', function(){
	$(this).addClass('active').siblings().removeClass('active')
});


$("#second-tab").click(function(){
	$("#content-one, #content-three").hide();
	$("#content-two").fadeIn(1000);
});

$("#first-tab").click(function(){
	$("#content-two, #content-three").hide();
	$("#content-one").fadeIn(1000);
});

$("#third-tab").click(function(){
	$("#content-one, #content-two").hide();
	$("#content-three").fadeIn(1000);
});
/* ====Our Partner Area Button Active End==== */


/* ====Our Students Comments Slider Start==== */
$('.students-comments-slider').slick({
	dots: false,
	infinite: true,
	speed: 1000,
	autoplay: true,
	autoplaySpeed: 1500,
	autoplayHoverPause: true,
	slidesToShow: 2,
	slidesToScroll: 1,
	responsive: [
	{
		breakpoint: 1024,
		settings: {
			slidesToShow: 2,
			slidesToScroll: 1,
			infinite: true,
			dots: true
		}
	},
	{
		breakpoint: 600,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	},
	{
		breakpoint: 480,
		settings: {
			slidesToShow: 1,
			slidesToScroll: 1
		}
	}
    ]
});
	/* ====Our Students Comments Slider End==== */



	/* ====All Events Slider Start==== */
	$('#all-events-slider').slick({
		dots: false,
		infinite: true,
		speed: 1000,
		autoplay: true,
		autoplaySpeed: 1500,
		autoplayHoverPause: true,
		slidesToShow: 6,
		slidesToScroll: 1,
		responsive: [
			{
				breakpoint: 1024,
				settings: {
					slidesToShow: 3,
					slidesToScroll: 1,
					infinite: true,
					dots: true
				}
			},
			{
				breakpoint: 600,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1
				}
			},
			{
				breakpoint: 480,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1
				}
			}
		]
	});
/* ====All Events Slider End==== */













})()