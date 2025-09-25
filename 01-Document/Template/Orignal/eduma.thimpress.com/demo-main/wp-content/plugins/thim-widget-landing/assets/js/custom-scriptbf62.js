(function ($) {
    'use strict';

    var thim_landing = {
        slickslider: function () {
            var item_filter = $('.list-demo__item'),
                item_description = $('.list-demo-category-description'),
                wrapper = $('.animation-sequence');
            item_description.hide();
            $(".filter-button-group .list-demo-category__btn").click(function () {
                var item_data = $(this).data('filter');
                $('.filter-button-group .list-demo-category__btn').removeClass('active');
                $(this).addClass('active')

                if (wrapper.length > 0){
                    item_filter.hide().addClass( 'demo-hidden' );
                    wrapper.find(item_data).each(function (i) {
                        var self = $(this);
                        $(self).show()
                        setTimeout(function () {
                            $(self).removeClass('demo-hidden');
                        }, 80 * (i+1));
                    });
                    if (item_description.length > 0){
                        item_description.hide();
                        $(item_data).show();
                    }
                }else {
                    item_filter.hide();
                    item_description.hide();
                    $(item_data).show();
                }
                
            });
        },
    }
    $(document).ready(function () {
        thim_landing.slickslider;

    })
    $(window).on('elementor/frontend/init', function () {
        elementorFrontend.hooks.addAction('frontend/element_ready/thim-ekits-list-demo.default',
            thim_landing.slickslider)

    })

    // install app
    const installApp = document.getElementById("install-app");
    const actionApp = document.getElementById("action-app");

    if (installApp) {
        installApp.addEventListener('keyup', function () {
            if (actionApp) {
                if (this.value.length > 0) {
                    actionApp.removeAttribute('disabled')
                } else {
                    actionApp.setAttribute('disabled', 'disabled')
                }
            }
        })
    }
    if (actionApp) {
        actionApp.addEventListener("click", function () {
            if (!this.hasAttribute('disabled')) {
                window.open(`https://maxsale.storepify.app/authenticate/oauth?shop=${installApp.value}`, '_blank')
            }
        })
    }

    // Testimonial slick
    $('.slider-content').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        infinite: false,
        asNavFor: '.slider-thumb',
    });
    $('.slider-thumb').slick({
        infinite: true,
        centerPadding: '0',
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.slider-content',
        centerMode: true,
        dots: false,
        arrows: false,
        focusOnSelect: true,
        vertical: true,
    });

    $(document).ready(function () {
        $(document).on('click', '#view-more-addons, .popup-tabs .e-n-tabs-heading', function (e) {
            e.stopPropagation()
            $('body').addClass('add-ons-open')
        })

        $(document).on('click', '.close-add-ons, .close-popup-tabs', function (e) {
            $('body').removeClass('add-ons-open')
        })
        $(document).on('click', '.add-ons-more, .e-n-tabs-content > div, .e-n-tabs-content .elementor-widget-image', function (e) {
            e.stopPropagation()
        })
        $(document).on('click', '.demo-show-popup', function (e) {
            e.stopPropagation()
            $(this).toggleClass('show')
        })
        $(document).on('click', '.list-demo__item__popup__content', function (e) {
            e.stopPropagation()
        })
    });

    var $element = $('.elementor-fixed .back-to-top__swapper');

    if ($element) {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                $element.addClass('scrolldown').removeClass('scrollup');
            } else {
                $element.addClass('scrollup').removeClass('scrolldown');
            }
        });

        $element.on('click', function () {
            $('html,body').animate({ scrollTop: '0px' }, 800);
            return false;
        });
    }

    jQuery(document).ready(function () {
        const headerOffset = 80;

        function onScroll() {
            const scrollPos = jQuery(document).scrollTop() + headerOffset + 1;

            jQuery('#thim-ekits-menu-landing-eduma a[href^="#"]').each(function () {
                const currLink = jQuery(this);
                const refElement = jQuery(currLink.attr("href"));

                if (refElement.length) {
                    const refTop = refElement.offset().top;
                    const refBottom = refTop + refElement.outerHeight();

                    if (scrollPos >= refTop && scrollPos < refBottom) {
                        jQuery('#thim-ekits-menu-landing-eduma a').removeClass("active");
                        currLink.addClass("active");
                        return false;
                    }
                }
            });
        }

        jQuery(document).on("scroll", onScroll);

        jQuery('#thim-ekits-menu-landing-eduma a[href^="#"]').on('click', function (e) {
            e.preventDefault();

            const target = this.hash;
            const $target = jQuery(target);

            if ($target.length) {
                jQuery(document).off("scroll");

                jQuery('#thim-ekits-menu-landing-eduma a').removeClass('active');
                jQuery(this).addClass('active');

                jQuery('html, body').animate({
                    scrollTop: $target.offset().top - headerOffset
                }, 500, function () {
                    jQuery(document).on("scroll", onScroll);
                    onScroll();
                });
            }
        });

        onScroll();
    });

})(jQuery);

// List Video
let playItems = document.querySelectorAll(".list-play__item");
if (playItems.length > 0) {
    playItems.forEach(function (link, index) {
        link.addEventListener("click", function (event) {
            let target = null
            if (event.target.classList.contains('list-play__item')) {
                target = event.target
            }
            if (event.target.closest('.list-play__item')) {
                target = event.target.closest('.list-play__item')
            }
            let dataList = target.getAttribute("data-list");
            let arrDataList = dataList.split('-')
            let keyLast = arrDataList[arrDataList.length - 1]
            let listVideo = target.closest(".list-video__wapper");
            let videoActive = listVideo.querySelector(".list-play__item.active");
            if (videoActive) {
                videoActive.classList.remove("active");
            }
            link.classList.add("active");
            let boxes = listVideo.querySelectorAll(".list-play__video");
            boxes.forEach((box, indexChild) => {
                box.classList.remove("active");
                let arrDataListTmp = box.getAttribute("data-list").split('-')
                let keyLastTmp = arrDataListTmp[arrDataListTmp.length - 1]
                if (parseInt(keyLast) === parseInt(keyLastTmp)) {
                    box.classList.add("active");
                }
            });
        });
    });
}

jQuery(window).on('elementor/frontend/init', () => {
    const addHandler = ($element) => {
        elementorFrontend.elementsHandler.addHandler(window.ThimEkits.ThimSlider, {
            $element,
        });
    };
    elementorFrontend.hooks.addAction('frontend/element_ready/thim-ekits-slide-custom.default', addHandler);
});

