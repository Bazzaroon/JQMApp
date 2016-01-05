var pageData = function (imagedata) {
    this.ImageData = [];
};
var jqmView = {
    viewedPage: 'front',
    viewElement: null,
    activePage: 1,
    pgData:[],
    
    Init: function() {
        jqmView.viewElement = $('.front');
        
        var offset = $(document).height() > $(document).width() ? 250 : 185;
        var aHeight = $(document).height() - offset;
        var aWidth = parseInt((aHeight * 80) / 100);
        $(jqmView.viewElement).css({ height: aHeight + 'px', width: aWidth + 'px' });
        if (offset > 185) {
            $(jqmView.viewElement).css({ margin: '10px 0 0 20px' });
        } else {
            $(jqmView.viewElement).css({ margin: '10px auto' });
        }

        $(window).on('resize', function () {
            offset = $(document).height() > $(document).width() ? 250 : 185;
            aHeight = $(document).height() - offset;
            aWidth = parseInt((aHeight * 80) / 100);
            $(jqmView.viewElement).css({ height: aHeight + 'px', width: aWidth + 'px' });
            if (offset > 185) {
                $(jqmView.viewElement).css({ margin: '10px 0 0 20px' });
            } else {
                $(jqmView.viewElement).css({ margin: '10px auto' });
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

        switch (pos) {
            case 'next':
                pgTo = pgnum >= album[0].PageCount ? pgnum : pgnum + 1;
                jqmView.viewElement = jqmView.viewElement == $('.front') ? $('.back') : $('.front');
                jqmView.PutPage(jqmView.viewElement, pgTo);
                jqmView.activePage = pgTo;
                break;
        }
    },

    PutPage: function (el, pgNum) {
        var pg = jqmView.pgData[pgNum - 1];
        for (var x = 0; x < pg.ImageData.length; x++) {
            var units = jqmView.GetScaledUnits(pg.ImageData[x]);
            $(el).append("<div class='photoclass' style='position:absolute;left:" + units.l + ";top:" + units.t + "'><img onclick='editor.Edit(this)' width='" + units.w + "' src='" + $.cookie('location') + pg.ImageData[x].Url + "'></img></div>");
        }
    },
    
    GetScaledUnits: function (iData) {
        var units = { l: null, t: null, w: null, h: null };
        var ed = $(jqmView.viewElement);
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