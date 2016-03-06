
    function Album(id, name, userid, created, active, pagecount, slides, coverimage, stopusers) {
        this.Id = id;
        this.Name = name;
        this.UserId = userid;
        this.Created = created;
        this.Active = active;
        this.PageCount = pagecount;
        this.Slides = slides;
        this.CoverImage = coverimage;
        this.StopUsers = stopusers;
    };

var desktop = {
    UpdateAlbumById: function(id, form) {
        var tData = $("#" + form + " :input");
        var cbData = $("#" + form + " :input[type='checkbox']");
        cbData.each(function() {
            var N = $(this).attr('id');
        });

        var fData = sql.Encode(tData, id);
        desktop.SqlAjax(fData);
    },
    ChangeHomePage: function (url) {
        var wedAlbum = JSON.parse($.cookie('album'));
        var URL = $.cookie('location') + 'Data/UpdateHomePage?fileName=' + url + "&albumId=" + wedAlbum[0].Id;
        $.ajax({
            url: URL,
            async: false,
            type: 'get',
            error: function () {
                alert('Unable to change album home page');
            },
            success: function () {
                location.href = $.cookie('location');
            }
        });
    },
    UpdateUserRecord: function(index, id) {
        var dex = index;
        var tableName = 'Users';
        var sqlStr = "update " + tableName + " set ";
        sqlStr += "UserName = '" + $('#urecord' + dex).find('#uname').val() + "', ";
        sqlStr += "Active = '" + $('#urecord' + dex).find('#active').prop('checked') + "', ";
        sqlStr += "Administrator = '" + $('#urecord' + dex).find('#admin').prop('checked') + "'";
        sqlStr += " where Id = " + id;

        desktop.SqlAjax(btoa(sqlStr), $('#urecord' + dex).find('#usrmsg'));


    },

    SqlAjax: function (data, el) {
        $.ajax({
            url: $.cookie('location') + 'Data/SqlUpdate64',
            type: 'post',
            async: false,
            data: data,
            error: function () {
                alert('Unable to update album');
            },
            success: function() {
                el.addClass('tick');
                setTimeout(function() {
                    el.removeClass('tick'); }, 3000);
            }
        });
    }
};

var sql = {    
  Encode: function(el, id) {
      var tableName = el.attr('name').split('.')[0];
      var sqlStr = "update " + tableName + " set ";
      el.each(function () {
          switch($(this).attr('type')) {
              case 'text':
                  sqlStr += $(this).attr('name').split('.')[1] + " = '" + $(this).val() + "', ";
                  break;
              case 'checkbox':
                  sqlStr += $(this).attr('name').split('.')[1] + " = '" + $(this).prop('checked') + "', ";
                  break;
          }
      });
      sqlStr = sqlStr.substr(0, sqlStr.lastIndexOf(','));
      sqlStr += " where Id = '" + id + "'";
      return btoa(sqlStr);
  }  
};