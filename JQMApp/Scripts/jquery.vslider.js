//Jquery vertical slider

$.widget( "custom.vslider", {
    self:null,
	opts:null,

	options:{
		orientation:'vertical',
		max: 100,
		min: 0,
		length: 200,
		thickness: 30
	},

	_create: function() {
		self = this;
		opts = self.options;
		var mkUp = "<div class='vslider' style='width:" + opts.length + "px;height:" + opts.thickness + "px'></div>";
		$(this.element).append(mkUp);
    }
});