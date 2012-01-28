function Bomberlink(canvas, settings) {
	Game.call(this, canvas);
	this._x = 0;
	this._y = 0;
	this._link = new Link();
};

Bomberlink.prototype = $.extend(true, {}, Game.prototype);

Bomberlink.prototype.update = function(milliseconds) {
	if (this.keyPressed('rightArrow')) this._link.moveRight();
	if (this.keyPressed('leftArrow')) this._link.moveLeft();
	if (this.keyPressed('upArrow')) this._link.moveUp();
	if (this.keyPressed('downArrow')) this._link.moveDown();
	
	this._link.update(milliseconds);
};

Bomberlink.prototype.render = function(context) {
	this._link.render(context);
};