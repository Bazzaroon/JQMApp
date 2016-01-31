
var quickbox = {    
    mkUp: "<div class='quickbox'><div><div></div></div><img src='#'></img></div>",
    scale: 90,
    
    Create: function(url) {
    $('body').append(quickbox.mkUp);
    var orientation = $(document).height() > $(document).width() ? 'portrait' : 'landscape';

    var actualWidth = parseInt($(document).width() * quickbox.scale / 100);
    var actualHeight = parseInt($(document).height() * quickbox.scale / 100);
   
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
        var hGap = $(document).width() - QBW;
        hGap = parseInt(hGap / 2);
        
        var QBH = parseInt($('.quickbox').css('height'));
        var vGap = $(document).height() - QBH;
        vGap = parseInt(vGap / 2);

        $('.quickbox').css({ top: vGap + 'px', left: hGap + 'px' });
    },
    
    Close: function() {
        $('.quickbox').remove();
    },
    
    IsMobile: function () {
        var isMobile = false;
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            isMobile = true;
        }
        return isMobile;
    }

    


};