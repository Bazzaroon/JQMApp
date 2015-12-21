﻿var global = {
    imageId: -1,
    Album: null,
    AlbumId: 1000,
    ScreenWidth: -1,
    ScreenHeight: -1,
    editing: true,
    activePage: '1',
    UserId:0
};
var Users = function () {
    this.Id = 0;
    this.UserName = null;
    this.Email = null;
    this.Active = true;
    this.Administrator = false;
    this.AlbumId = 0;
    this.PasswordHash = null;
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

var photo = function() {
    this.Id = 0;
    this.OTop = 0;
    this.OLeft = 0;
    this.Width = 0;
    this.Height = 0;
    this.Url = null;
    this.Scale = 0;
    this.PageNumber = 0;
    this.AlbumId = 0;
};

var pageData = function(imagedata) {
    this.ImageData = [];
};

var imagexy = {
    imageObj: null,
    
    Put: function() {
        
    },
    Get: function() {
        
    },
    Place: function() {
        
    }
};

var pages = {
    pgData: [],

    Portrait: function () {
        
    },
    Add: function (albumId) {
        for (var x = 0; x < global.Album[0].PageCount; x++) {
            var photos = pages.GetPhotos(albumId, (x+1));
            var pics = new Array();
            var NP = new pageData();
            for (var y = 0; y < photos.length; y++) {
                    pics.push(photos[y]);
                }
            NP.ImageData = pics;
            pages.pgData.push(NP);
        }
    },
    AddPage: function (pageNumber) {
        var data = pages.pgData[pageNumber - 1];
        var mkUp = "<div id='pg" + pageNumber + "' data-role='page'>";
        mkUp += "<div class='pagecontainer'>";
        for (var x = 0; x < data.ImageData.length; x++) {
            mkUp += "<img style='position:relative;width:" + data.ImageData[x].Width + "px;height:" + data.ImageData[x].Height + "px;";
            mkUp += "left:" + data.ImageData[x].OLeft + "px;top:" + data.ImageData[x].OTop + "px' src='" + data.ImageData[x].Url + "'/>";
        }
        mkUp += "</div></div>";
        $('body').append(mkUp);
    },
    RemovePage: function (pageNumber) {

    },
    GetPhotos: function (albumId, pageNUmber) {
        var query = "Data/GetPhotosForPage?albumId=" + albumId + "&pageNumber=" + pageNUmber;
        return jax.GetData(query);
    },
    Show: function (pageNUmber, orientation) {
        $.mobile.changePage('#pg' + pageNUmber);
    },
    NextPage: function () {
        var cPage = parseInt(global.activePage) + 1;
        global.activePage = cPage;
        editor.ShowPage();
    },
    PreviousPage: function(){
        var aPage = parseInt(global.activePage) - 1;
        if (aPage < 1) return;
        global.activePage = aPage;
        editor.ShowPage();
    }

};

function PageSize() {
    // based on 1024 X 768
    
    var SW = $(window).width();
    var SH = $(window).height();
    var pageDimentions = { w: 0, h: 0 };
    var percentW = parseInt((1024 / SW) * 100);
    var wdth = parseInt((600 / 1024) * 100);
    var actualWidth = parseInt((wdth / percentW) * 100);
}

var editor = {
    isEditing: false,
    imageId: null,
    imagePhoto: null,
    GetThumbs: function(el, pfx) {
        var offset = 0;
        $('#thumbs').mThumbnailScroller({
            type: 'hover-precise',
            setHeight: 100
        });
        offset = $(document).height() > $(document).width() ? 250 : 185;
        aHeight = $(document).height() - offset;
        aWidth = parseInt((aHeight * 80) / 100);
        $('#editorpage').css({ height: aHeight + 'px', width: aWidth + 'px' });
        if (offset > 185) {
            $('#editorpage').css({ margin: '10px 0 0 20px' })
        } else {
            $('#editorpage').css({ margin: '10px auto' })
        }

        $(window).on('resize', function() {
            offset = $(document).height() > $(document).width() ? 250 : 185;
            aHeight = $(document).height() - offset;
            aWidth = parseInt((aHeight * 80) / 100);
            $('#editorpage').css({ height: aHeight + 'px', width: aWidth + 'px' });
            if (offset > 185) {
                $('#editorpage').css({ margin: '10px 0 0 20px' });
            } else {
                $('#editorpage').css({ margin: '10px auto' });
            }
            editor.ShowPage();
        });

        $('#xrange').on('change', function(event) {
            if ($('#radio-mini-1').prop('checked')) {
                $('#eddy img').attr('width', $('#xrange').val());
            }
            if ($('#radio-mini-2').prop('checked')) {
                $('#eddy').css({ left: $('#xrange').val() + 'px' });
            }
        });

        $('#yrange').on('change', function(event) {
            if ($('#radio-mini-2').prop('checked')) {
                $('#eddy').css({ top: $('#yrange').val() + 'px' });
            }
        });

        $('#radio-mini-2').on('click', function() {
            $('#xrange').val(parseInt($('#eddy').css('left'))).slider('refresh');
            $('#yrange').val(parseInt($('#eddy').css('top'))).slider('refresh');
            $('#yrange').slider('enable');
        });
        $('#radio-mini-1').on('click', function() {
            $('#yrange').slider('disable');
            $('#xrange').val($('#eddy img').attr('width')).slider('refresh');
            $('#yrange').val('#eddy').css('width').slider('refresh');
        });

        var mkUp = '';
        var mpfx = pfx ? '../' : '';
        var query = "Data/GetAllThumbs?albumId=" + global.AlbumId;
        var thumbs = jax.GetData(query);
        for (var x = 0; x < thumbs.length; x++) {
            var source = thumbs[x].Url.toLowerCase();
            var src = source.replace('.jpg', '_t.jpg');
            var fullSourcePath = $.cookie('location') + src;
            mkUp += "<li><a href='#'><img id='" + thumbs[x].Id + "' onclick='editor.OpenImageEditor(this, true)' src='" + fullSourcePath + "' ui-draggable></img></a><li>";
        }
        $(el).append(mkUp);
        if (pages.pgData[global.activePage] != null) {
            editor.ShowPage();
        }
    },
    GetPage: function(pagenumber) {
        var query = "/Data/GetPhotosForPage?albumId=" + global.AlbumId + "&pageNumber=" + pagenumber;
        var pics = jax.GetData(query);

    },
    SetPageSize: function() {

    },
    OpenImageEditor: function(G, fromScroller) {
        if (editor.isEditing) return;
        var url = G.src.replace('_t', '');
        $('#editorpage').append("<div id='eddy'><img width='200' id='" + G.id + "' src='" + url + "'></div>");
        editor.isEditing = true;
        $('#epanel').show();
        if (fromScroller) {
            $('#xrange').val(200).slider('refresh');
            $('#yrange').slider('disable');
        }

    },
    MapEddyToPhoto: function() {
        var E = $('#eddy');
        E.css({ left: photo.OLeft + 'px', top: photo.OTop + 'px', width: photo.Width + 'px', height: '100px' });
    },
    ShowPage: function() {
        $('#editorpage').empty();
        $('#imagemanager h1').text('Page ' + global.activePage);
        var pgIndex = parseInt(global.activePage) - 1;
        var pg = pages.pgData[pgIndex];
        for (var x = 0; x < pg.ImageData.length; x++) {
            var units = editor.GetScaledUnits(pg.ImageData[x]);
            $('#editorpage').append("<div data-image-index='" + x + "' data-index='" + pg.ImageData[x].Id + "' style='position:absolute;left:" + units.l + ";top:" + units.t + "'><img onclick='editor.Edit(this)' width='" + units.w + "' height='" + units.h + "' src='" + $.cookie('location') + pg.ImageData[x].Url + "'></img></div>");
        }
    },
    GetScaledUnits: function(iData) {
        var units = { l: null, t: null, w: null, h: null };
        var ed = $('#editorpage');
        var scale = ed.height() / 750 * 100;
        units.w = Math.ceil((iData.Width * scale) / 100).toString();
        units.l = (Math.ceil((iData.OLeft * scale) / 100)) + 'px';
        units.t = (Math.ceil((iData.OTop * scale) / 100)) + 'px';
        return units;
    },
    Edit: function(G) {
        editor.isEditing = false;
        $('#eddy').removeAttr('id');
        $(G).parent().attr('id', 'eddy');
        $(G).css({ border: '2px solid Red' });
        $('#epanel').show();
        editor.isEditing = true;
    },
    Cancel: function() {
        if ($('#eddy').attr('data-index') != 0) {
            if (confirm('Remove this image?')) {
                editor.RemoveImage();
                return;
            }
        }

        if ($('#eddy').attr('data-index') == 0) {
            $('#eddy').remove();
        } else {
            $('#eddy img').css({ border: 'none' });
        }
    },
    Save: function() {
        if (!confirm('Update image?')) return;
        var ed = $('#editorpage');
        var scale = Math.ceil(ed.height() / 750 * 100);
        var P = new photo();
        if ($('#eddy').attr('data-index') == undefined) {
            P.Id = 0;
        } else {
            P.Id = parseInt($('#eddy').attr('data-index'));
        }
        P.OTop = editor.Ceil($('#eddy').css('top'), scale);
        P.OLeft = editor.Ceil($('#eddy').css('left'), scale);
        P.Width = editor.Ceil($('#eddy img').attr('width'), scale);
        P.Url = $('#eddy img').attr('src').replace($.cookie('location'), '');
        P.AlbumId = global.AlbumId;
        P.PageNumber = global.activePage;
        P.GraphicId = parseInt($('#eddy img').attr('id'));

        $.ajax({
            url: $.cookie('location') + 'Data/AddImageToPage',
            type: 'post',
            async: false,
            data: JSON.stringify(P),
            success: function(data) {
                if (P.Id == 0) {
                    P.Id = data;
                    pages.pgData[global.activePage - 1].ImageData.push(P);
                }
                editor.isEditing = false;
                $('#eddy').removeAttr('id');
            },
            error: function() {
                alert('Unable to save new image on page');
            }
        });
    },
    Ceil: function(css, scale) {
        var val = parseInt(css.replace('px', ''));
        var rVal = Math.ceil((val * scale) / 100);
        return rVal;
    },
    RemoveImage: function() {
        $.ajax({
            url: $.cookie('location') + 'Data/DeletePhoto?Id=' + $('#eddy').attr('data-index'),
            type:'get',
            async:false,
            error:function() {
                alert('Unable to delete photo');
            },
            success:function() {
                pages.pgData[global.activePage - 1].ImageData.splice([parseInt($('#eddy').attr('data-image-index'))], 1);
                $('#eddy').remove();
                editor.isEditing = false;
            }
        });
    }


};