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
    windowScale: 90,
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

        $('.container').on('swiperight', function() {
            alert('swiped in an upward direction');
        });

        $('body').on('keydown', function(event) {
            switch(event.which) {
                case 39: //right arrow
                    jqmView.NextViewPage();
                    break;
                case 37: //left arrow
                    jqmView.PreviousViewPage();
                    break;
            }
        });

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
        $('#pagecontainer').css({ height: ($(window).height() - $('#viewpage div:eq(0)').height() - 5) + 'px' });
        jqmView.myAlbum = JSON.parse($.cookie('album'));
        var mkUp = "<div id='outer' align='center' style='display:inline-block;width:" + $(window).width() + ";height:" + $(window).height() + "'><div class='container'></div></div>";

        for (var x = 0; x < jqmView.myAlbum[0].PageCount; x++) {
            $('#outer').css({ width: $(window).width() + 'px', height: $(window).height() + 'px' });
            $('#scroller').append(mkUp);
        }
        jqmView.SetPageSize();

        var scrollerWidth = $(window).width() * parseInt(jqmView.myAlbum[0].PageCount);
        $('#scroller').css({ width: scrollerWidth + 'px', position: 'absolute' });
    },

    PutPage: function (el, pgNum) {
        var pg = jqmView.pgData[pgNum - 1];
        for (var x = 0; x < pg.ImageData.length; x++) {
            var units = jqmView.GetScaledUnits(pg.ImageData[x]);
            $(el).append("<div class='photoclass' style='position:absolute;left:" + units.l + ";top:" + units.t + "'><img width='" + units.w + "' src='" + $.cookie('location') + pg.ImageData[x].Url + "'></img></div>");
        }
    },
    
    GetScaledUnits: function (iData) {
        var units = { l: null, t: null, w: null, h: null };
        var ed = $('#pagecontainer');
        var scale = ed.height() / 750 * 100;
        units.w = Math.ceil((iData.Width * scale) / 100).toString();
        units.l = (Math.ceil((iData.OLeft * scale) / 100)) + 'px';
        units.t = (Math.ceil((iData.OTop * scale) / 100)) + 'px';
        return units;
    },
    
    NextViewPage: function() {
        $('#scroller').animate({ left: '-=' + $(window).width().toString() }, 500);
    },
    PreviousViewPage: function() {
        $('#scroller').animate({ left: '+=' + $(window).width().toString() }, 500);
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