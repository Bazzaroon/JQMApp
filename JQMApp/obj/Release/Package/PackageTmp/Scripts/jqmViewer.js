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
    windowScale:90,
    
    Init: function() {
        jqmView.viewElement = $('.front');
        
        if ($(window).width() > $(window).height()) {
            jqmView.aHeight = $(window).height() * jqmView.windowScale / 100;
            jqmView.aWidth = jqmView.aHeight * 80 / 100;
        } else {
            jqmView.aWidth = $(window).width() * jqmView.windowScale / 100;
            jqmView.aHeight = jqmView.aWidth * 120 / 100;
        }

        var margin = parseInt($(window).height() - jqmView.aHeight) / 2;
        $('#pagecontainer').css({ 'margin-top': margin });

        $('#pagecontainer').css({ height: jqmView.aHeight + 'px', width: jqmView.aWidth + 'px' });

        $(window).on('resize', function () {

            if ($(window).width() > $(window).height()) {
                jqmView.aHeight = $(window).height() * jqmView.windowScale / 100;
                jqmView.aWidth = jqmView.aHeight * 80 / 100;
            } else {
                jqmView.aWidth = $(window).width() * jqmView.windowScale / 100;
                jqmView.aHeight = jqmView.aWidth * 120 / 100;
            }

            margin = parseInt($(window).height() - jqmView.aHeight) / 2;
            $('#pagecontainer').css({ 'margin-top': margin });

            $('#pagecontainer').css({ height: jqmView.aHeight + 'px', width: jqmView.aWidth + 'px' });

            jqmView.PutPage(jqmView.viewElement, 1);
        });

        $('.front').click(function(event) {
            if (event.offsetX > jqmView.aWidth - 70 && event.offsetX < jqmView.aWidth) {
                jqmView.PutPage($('.back'), 2);
                $('#cover').addClass('opened');
            }
        });
        
        jqmView.LoadAllPages();
        jqmView.LoadPage(true);

    },
    
    LoadAllPages: function() {
        var myAlbum = JSON.parse($.cookie('album'));
        for (var x = 0; x < myAlbum[0].PageCount; x++) {
            var photos = jqmView.GetPhotos(myAlbum[0].Id, (x + 1));
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


    LoadPage: function (isInitial) {
        var pgTo = -1;
        var pgnum = parseInt(jqmView.activePage);
        if (isInitial) {
            jqmView.PutPage(jqmView.viewElement, 1);
        }
    },

    PutPage: function (el, pgNum) {

        $(jqmView.viewElement).empty();
        
        var pg = jqmView.pgData[pgNum - 1];
        for (var x = 0; x < pg.ImageData.length; x++) {
            var units = jqmView.GetScaledUnits(pg.ImageData[x]);
            $(el).append("<div class='photoclass' style='position:absolute;left:" + units.l + ";top:" + units.t + "'><img onclick='editor.Edit(this)' width='" + units.w + "' src='" + $.cookie('location') + pg.ImageData[x].Url + "'></img></div>");
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