var LINK_ANIMATION_STOPPED = 0;
var LINK_ANIMATION_WALKING = 1;

var LINK_DOWN = 0;
var LINK_UP = 1;
var LINK_RIGHT = 2;
var LINK_LEFT = 3;

var LINK_OFFSET = 2;

var LINK_ANIMATION_FRAMES = [1, 8, 1, 8, 1, 8, 1, 8];
var LINK_ANIMATION_TIMES = [0, 60, 0, 60, 0, 60, 0, 60];

var LINK_MOVEMENT_SPEED = 120;

function Link(args) {
	Sprite.call(this, { image: 'greenlink', width: 44, height: 56, animationFrames: LINK_ANIMATION_FRAMES, animationTimes: LINK_ANIMATION_TIMES });
	
	this._xSpeed = 0;
	this._ySpeed = 0;
	this._direction = LINK_DOWN;
	
	var instance = this;
	
	this.__defineSetter__('animation', function(animation) {
		var value = instance._direction * LINK_OFFSET + animation;
		if (value != instance._animationY) {
			instance._animationX = 0;
			instance._animationY = value;
		}
	});
};

Link.prototype = $.extend(true, {}, Sprite.prototype);

Link.prototype.update = function(milliseconds) {
	this.animation = this._xSpeed == 0 && this._ySpeed == 0 ? LINK_ANIMATION_STOPPED : LINK_ANIMATION_WALKING;
	
	this.x += this._xSpeed * milliseconds / 1000.0;
	this.y += this._ySpeed * milliseconds / 1000.0;
	
	this._xSpeed = 0;
	this._ySpeed = 0;
	
	Sprite.prototype.update.call(this, milliseconds);
};

Link.prototype.moveLeft = function() {
	this._xSpeed = -LINK_MOVEMENT_SPEED;
	this._direction = LINK_LEFT;
};

Link.prototype.moveRight = function() {
	this._xSpeed = LINK_MOVEMENT_SPEED;
	this._direction = LINK_RIGHT;
};

Link.prototype.moveUp = function() {
	this._ySpeed = -LINK_MOVEMENT_SPEED;
	this._direction = LINK_UP;
};

Link.prototype.moveDown = function() {
	this._ySpeed = LINK_MOVEMENT_SPEED;
	this._direction = LINK_DOWN;
};