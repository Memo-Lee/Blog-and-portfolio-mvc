// sayfa açıldığında herşey yüklendiğin de çalışır.
// $(window).on("load",function(){
// })

// sayfa açıldığın hiçbir şey yüklemeden çalışır
$(document).ready(function () {
    if (localStorage.getItem("openmenu") == "true") {
        if ($('#fh5co-aside').css("left") != "0px") {
            $('#fh5co-aside').animate({ left: '0px' }, 0);
            $('.aside-close').animate({ left: '200px' }, 750);
            $('.aside-open').animate({ left: '-85px' }, 0);
            $('#site-responsive-m').css({ padding: '0' }, 750);
            $('#site-responsive-2').addClass("col-md-2", 750);
            $('#site-responsive-10').removeClass("col-md-12", 750).addClass("col-md-10", 750);
            $(this).animate({ left: '-80px' }, 750);
        }
    } else {
        if ($('#fh5co-aside').css("left") == "0px") {
            $('#fh5co-aside').animate({ left: '-240px' }, 0);
            $('.aside-close').animate({ left: '-40px' }, 0);
            $('.aside-open').animate({ left: '0px' }, 300);
            $('#aside-sm-icon-id').addClass("aside-sm-icon", 750);
            $('#site-responsive-m').css({ paddingLeft: '100px' }, 750);
            $('#site-responsive-2').removeClass("col-md-2", 750);
            $('#site-responsive-10').removeClass("col-md-10", 750).addClass("col-md-12", 750);
        }
    }
    $('.aside-close').click(function () {
        // ilk başta css döndürülüyor.
        // class olarak css şeklinde getirmek lazım...
        // bastığımızsa set etmemiz lazım.
        localStorage.setItem("openmenu", "false");
        if ($('#fh5co-aside').css("left") == "0px") {
            $('#fh5co-aside').animate({ left: '-240px' }, 0);
            $('.aside-close').animate({ left: '-40px' }, 0);
            $('.aside-open').animate({ left: '0px' }, 300);
            $('#aside-sm-icon-id').addClass("aside-sm-icon", 750);
            $('#site-responsive-m').css({ paddingLeft: '100px' }, 750);
            $('#site-responsive-2').removeClass("col-md-2", 750);
            $('#site-responsive-10').removeClass("col-md-10", 750).addClass("col-md-12", 750);
        }
    });
    $('.aside-open-icon').click(function () {
        localStorage.setItem("openmenu", "true");
        if ($('#fh5co-aside').css("left") != "0px") {
            $('#fh5co-aside').animate({ left: '0px' }, 0);
            $('.aside-close').animate({ left: '200px' }, 750);
            $('.aside-open').animate({ left: '-85px' }, 0);
            $('#site-responsive-m').css({ padding: '0' }, 750);
            $('#site-responsive-2').addClass("col-md-2", 750);
            $('#site-responsive-10').removeClass("col-md-12", 750).addClass("col-md-10", 750);
            $(this).animate({ left: '-80px' }, 750);
        }
    });
});