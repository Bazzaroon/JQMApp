var userPage = {    
    jUpg: null,
    Create: function(userId) {
        userPage.jUpg = $('#userhome');
        userPage.jUpg.append("<div id='gslide'></div>");
        var images = userPage.GetImages(userId);
    },

    GetImages: function (userId) {
        $.ajax({
            url: $.cookie('location') + 'SiteUser/Index?id=' + userId,
            type: 'get',
            async: false,
            error: function() {
                alert('Unable to load images');
            }
            
        });
    }
};