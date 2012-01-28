function Sprite(args) {
	this._animationX = 0;
	this._animationY = 4;
	this._image = $('#' + args.image)[0];
	
	this._animationFrames = args.animationFrames || [1];
	this._animationTimes = args.animationTimes || [0];
	this._animationCounter = 0;
	
	this._x = args.x || 0;
	this._y = args.y || 0;
	
	this._width = args.width || $(this._image).width();
	this._height = args.height || $(this._height).height();
	
	var instance = this;
	this.__defineGetter__('x', function() { return instance._x; });
	this.__defineSetter__('x', function(value) { instance._x = Math.round(value * 100) / 100.0; });
	
	this.__defineGetter__('y', function() { return instance._y; });
	this.__defineSetter__('y', function(value) { instance._y = Math.round(value * 100) / 100.0; });
	
	this.__defineGetter__('width', function() { return $(instance._image).width(); });
	this.__defineGetter__('height', function() { return $(instance._image).height(); });
};

Sprite.prototype.update = function(milliseconds) {
	if (this._animationFrames[this._animationY] <= 1) return;
	
	this._animationCounter += milliseconds
	if (this._animationCounter > this._animationTimes[this._animationY]) {
		this._animationCounter = 0;
		this._animationX = (this._animationX +1) % (this._animationFrames[this._animationY]);
	}
};

Sprite.prototype.render = function(context) {
	context.drawImage(this._image, this._animationX * this._width, this._animationY * this._height, this._width, this._height, this._x | 0, this._y | 0, this._width, this._height);
};