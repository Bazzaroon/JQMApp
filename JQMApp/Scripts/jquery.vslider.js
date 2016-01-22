//Jquery vertical slider

$.widget( "custom.vslider", {
    self:null,
	opts:null,

	options:{
		orientation:'horizontal',
		max: 100,
		min: 0,
		length: 200,
		thickness: 15,
		value: 50
	},

	_create: function() {
	    self = this;
	    opts = self.options;
	    if (opts.orientation == 'horizontal') {
	        var mkUp = "<div class='vslider' style='width:" + opts.length + "px;height:" + opts.thickness + "px'></div>";
	    } else {
	        var mkUp = "<div class='vslider' style='width:" + opts.thickness + "px;height:" + opts.length + "px'></div>";
	    }

	    var oLeft = opts.orientation == 'horizontal' ? '2px' : '0px';

	    switch (opts.orientation) {
	    case 'vertical':
	        $(this.element).css({ width: '30px' });
	        break;
	    case 'horizontal':
	        $(this.element).css({ height: '30px' });
	        break;
	    }

	    mkUp += "<div class='thumb' style='left:" + oLeft + "'></div>";

	    $(this.element).append(mkUp);
	    switch (opts.orientation) {	    

	    case 'vertical':
	        $('.vslider').css({ 'margin-left': '7.5px' });
	        break;
	        case 'horizontal':
	            $('.vslider').css({ 'margin-top': '7.5px' });
	            break;
	    }
	    self._SetThumb();
	},
    
	_SetThumb: function () {
	    var val = parseInt(opts.value * opts.length / 100);
	    switch(opts.orientation) {
	        case 'vertical':
	            $('.thumb').css({ top: (opts.length - val) + 'px' });
	            break;
	        case 'horizontal':
	            $('.thumb').css({ left: (opts.length - val) + 'px' });
	            break;
	    }
	}

});