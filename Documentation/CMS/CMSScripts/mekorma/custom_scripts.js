$(document).ready(function ($) {
    
    $("#version-text-togglebtn").click(function () {
        $("#version-text-toggle").toggle();
    });

    $(".navlinks li.parent-link").hover(function () {

        checkHiddenChildNav();
        $(this).children("ul").slideDown(250);

    }, function () {
        
        checkHiddenChildNav();
        $(this).children("ul").slideUp(250);
    });


    $(".navlinks li.parent-link").click(function () {
        checkHiddenChildNav();
        $(this).children("ul").slideToggle();
    });


    function checkHiddenChildNav() {

        var check = $(this).children("ul").is(":hidden");
        if (check == true) {
            $(this).addClass("menuHighlightMobile");
            $(this).addClass("menuHighlightDesktop");
            $(this).children("div").addClass("navSelect");
        }
        else {
            $(this).removeClass("menuHighlightMobile");
            $(this).removeClass("menuHighlightDesktop");
            $(this).children("div").removeClass("navSelect");
        }
    }


    $(".PagerPage").click(function () {
        $(".PagerPage").each(function () {
            $(this).css("background-image","url('/App_Themes/SiteImages/slider-btn.png')");
        });
        $(this).css("background-image", "url('/App_Themes/SiteImages/slider-btn-selected.png')");
    });
    
    if ($("#hero-section > *").length === 0)
    {
        $("#hero-section").css("border-bottom", "0");
        $("#hero-section").css("border-top", "0");
        $("#hero-section").addClass("mobileHeroBorder");
    }

    $(".control:empty").parent().hide();
    
    $(".tutorialLink").click(function () {
        var id = $(this).attr("id");
        $("#" + id + "Div").toggle();

        //swap button image
        return $(this).is('.openlink') ? $(this).css('background-image', 'url(/App_Themes/SiteImages/plus.png)').removeClass('openlink') : $(this).css('background-image', 'url(/App_Themes/SiteImages/minus.png)').addClass('openlink');
    });

});

