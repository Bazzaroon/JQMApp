
var quickbox = {    
    mkUp: "<div class='quickbox'><div><div></div></div><img src='#'></img></div>",
    overlay: "<div class='overlay'></div>",
    scale: 90,
    
    Create: function(url) {
    $('body').append(quickbox.mkUp);

    var orientation = $(window).height() > $(window).width() ? 'portrait' : 'landscape';

    var actualWidth = parseInt($(window).width() * quickbox.scale / 100);
    var actualHeight = parseInt($(window).height() * quickbox.scale / 100);
   
    switch (orientation) {
        case 'portrait':
            $('.quickbox img').attr('width', actualWidth + 'px');
            break;
        case 'landscape':
            $('.quickbox img').attr('height', actualHeight + 'px');
            break;
        }

            $('.quickbox img').attr('src', url);

            quickbox.CenterBox();
        
            if (jqmView.IsMobile()) {
                $('.quickbox div:nth-of-type(1)').on('touchstart', function () {
                    quickbox.Close();
                });
            } else {
                $('.quickbox div:nth-of-type(1)').on('click', function () {
                    quickbox.Close();
                });

            }


    },
    
    CenterBox: function() {
        var QBW = parseInt($('.quickbox').css('width'));
        var hGap = $(window).width() - QBW;
        hGap = parseInt((hGap / 2) + 10);
        
        var QBH = parseInt($('.quickbox').css('height'));
        var vGap = $(window).height() - QBH;
        vGap = parseInt(vGap / 2);

        $('.quickbox').css({ top: vGap + 'px', left: hGap + 'px' });

        $('body').append(quickbox.overlay);
        $('.quickbox').fadeIn(500);
    },
    
    Close: function() {
        $('.quickbox').remove();
        $('.overlay').remove();
    },
    
    IsMobile: function () {
        var isMobile = false;
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            isMobile = true;
        }
        return isMobile;
    }

    


};