var pageData = function (imagedata) {
    this.ImageData = [];
};
var jqmView = {
    viewedPage: 'front',
    viewElement: null,
    activePage: 1,
    pgData: [],
    aHeight: 0,
    aWidth: 0,
    windowScale: 100,
    myAlbum: null,
    Init: function() {
        jqmView.viewElement = $('.front');
        
        if ($(window).width() > $(window).height()) {
            jqmView.aHeight = $(window).height() * jqmView.windowScale / 100;
            jqmView.aWidth = jqmView.aHeight * 80 / 100;
        } else {
            jqmView.aWidth = $(window).width() * jqmView.windowScale / 100;
            jqmView.aHeight = jqmView.aWidth * 120 / 100;
        }

        var margin = (parseInt($(window).height() - jqmView.aHeight) / 2) - 15;
        $('.container').css({ 'margin-top': margin });

        $('.container').css({ height: jqmView.aHeight + 'px', width: jqmView.aWidth + 'px' });

        $(window).on('resize', function () {

            if ($(window).width() > $(window).height()) {
                jqmView.aHeight = $(window).height() * jqmView.windowScale / 100;
                jqmView.aWidth = jqmView.aHeight * 80 / 100;
            } else {
                jqmView.aWidth = $(window).width() * jqmView.windowScale / 100;
                jqmView.aHeight = jqmView.aWidth * 120 / 100;
            }

            margin = parseInt($(window).height() - jqmView.aHeight) / 2;
            $('.container').css({ 'margin-top': margin });

            $('.container').css({ height: jqmView.aHeight + 'px', width: jqmView.aWidth + 'px' });

            jqmView.PutPage(jqmView.viewElement, 1);
        });

        var tStart = 0;

        $('body').on('touchstart', function(evt) {
            evt.preventDefault();
            tStart = evt.originalEvent.touches[0].pageX;
        });

        $('body').on('touchend', function (evt) {
            evt.preventDefault();
            if (evt.originalEvent.changedTouches[0].pageX < tStart) {
                jqmView.NextViewPage();
            } else {
                    jqmView.PreviousViewPage();
            }
        });

 
        $(window).on('orientationchange', function (event) {
            if (event.orientation == 'landscape') {
                $.mobile.changePage('#landscape', {transition:'pop', role: 'dialog'});
            } else {
                $.mobile.changePage('#viewpage');
            }
        });

        $(document).on('keydown', function(event) {
            switch(event.which) {
                case 39:
                    jqmView.NextViewPage();
                    break;
                case 37:
                    jqmView.PreviousViewPage();
                    break;
            }
        });

        jqmView.LoadAllPages();
        jqmView.LoadAllViewPages();

    },
    
    SetPageSize: function() {
        if ($(window).width() > $(window).height()) {
            jqmView.aHeight = $(window).height() * jqmView.windowScale / 100;
            jqmView.aWidth = jqmView.aHeight * 80 / 100;
        } else {
            jqmView.aWidth = $(window).width() * jqmView.windowScale / 100;
            jqmView.aHeight = jqmView.aWidth * 120 / 100;
        }

        var margin = (parseInt($(window).height() - jqmView.aHeight) / 2) - 15;
        $('.container').css({ 'margin-top': margin });

        $('.container').css({ height: jqmView.aHeight + 'px', width: jqmView.aWidth + 'px' });
    },
    
    LoadAllPages: function() {
        jqmView.myAlbum = JSON.parse($.cookie('album'));
        $('#viewpage div h1').text(jqmView.myAlbum[0].Name);
        for (var x = 0; x < jqmView.myAlbum[0].PageCount; x++) {
            var photos = jqmView.GetPhotos(jqmView.myAlbum[0].Id, (x + 1));
            var pics = new Array();
            var NP = new pageData();
            for (var y = 0; y < photos.length; y++) {
                pics.push(photos[y]);
            }
            NP.ImageData = pics;
            jqmView.pgData.push(NP);
        }
    },

    GetPhotos: function (albumId, pageNUmber) {
        var query = "Data/GetPhotosForPage?albumId=" + albumId + "&pageNumber=" + pageNUmber;
        return jax.GetData(query);
    },


    LoadAllViewPages: function (isInitial) {
        $('#pagecontainer').css({ height: ($(window).height() - $('#viewpage div:eq(0)').height() - 1) + 'px' });
        jqmView.myAlbum = JSON.parse($.cookie('album'));

        for (var x = 0; x < jqmView.myAlbum[0].PageCount; x++) {
            var mkUp = "<div id='outer' class='outer' align='center' style='display:inline-block;width:" + $(window).width() + ";height:" + $(window).height() + "'><div id='pImage" + x + "' class='container'></div></div>";
            $('#scroller').append(mkUp);
            jqmView.PutPage($('#pImage' + x), x + 1);
        }

        jqmView.SetPageSize();

        var scrollerWidth = $(window).width() * parseInt(jqmView.myAlbum[0].PageCount);
        $('#scroller').css({ width: scrollerWidth + 'px', position:'relative'});
    },

    PutPage: function (el, pgNum) {
        var pg = jqmView.pgData[pgNum - 1];
        for (var x = 0; x < pg.ImageData.length; x++) {
            var units = jqmView.GetScaledUnits(pg.ImageData[x], x);
            var L = parseInt(units.l) + 12;
            var mkUp = "<div class='frame' style='position:absolute;left:" + L + ";top:" + units.t + "'>";
            mkUp += "<img id='pic" + x + "' width='" + units.w + "' src='" + $.cookie('location') + pg.ImageData[x].Url + "'></img></div>";

            $(el).append(mkUp);
            if (jqmView.IsMobile()) {
                $('#pic' + x).on('touchstart', function() {
                    jqmView.OpenLightBox($(this));
                });
            } else {
                $('#pic' + x).on('click', function () {
                    jqmView.OpenLightBox($(this));
                });

            }
        }
    },
    
    OpenLightBox: function (el) {
        quickbox.Create(el.attr('src'));
    },
    
    GetScaledUnits: function (iData, dex) {
        var units = { l: null, t: null, w: null, h: null };
        var scale = jqmView.aHeight / 750 * 100;
        units.w = Math.ceil((iData.Width * scale) / 100).toString();
        units.l = (Math.ceil((iData.OLeft * scale) / 100)) + 'px';
        units.t = (Math.ceil((iData.OTop * scale) / 100)) + 'px';
        return units;
    },
    
    NextViewPage: function() {
        if (jqmView.activePage > 0 && jqmView.activePage < jqmView.myAlbum[0].PageCount) {
            $('#scroller').animate({ left: '-=' + $(window).width().toString() }, 400);
            jqmView.activePage++;
        }
    },
    PreviousViewPage: function() {
        if (jqmView.activePage > 1 && jqmView.activePage <= jqmView.myAlbum[0].PageCount) {
            $('#scroller').animate({ left: '+=' + $(window).width().toString() }, 400);
            jqmView.activePage--;
        }
    },
    IsMobile: function () {
        var isMobile = false;
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            isMobile = true;
        }
        return isMobile;
    }

};

var jax = {
    GetData: function (query, errorMessage) {
        var json = '';
        var QS = $.cookie('location') + query;
        $.ajax({
            url: QS,
            async: false,
            type: 'GET',
            error: function (a, b, c) {
                alert('Unable to get ' + errorMessage + ' data');
            },
            success: function (data) {
                json = JSON.parse(data);
            }
        });

        return json;

    }
};