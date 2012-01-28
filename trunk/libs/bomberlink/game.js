function Game(canvas, settings) {
	var defaultSettings = {
		fps: 60,
		background: '#000'
	};
	
	this._canvas = $('#' + canvas).css('display', 'block')[0];
	this._settings = $.extend(defaultSettings, settings);
	this._pressedKeys = {}
	
	var instance = this;
	this.__defineGetter__('canvas', function() { return instance._canvas; });
	this.__defineGetter__('settings', function() { return instance._settings; });
	this.__defineGetter__('pressedKeys', function() { return instance._pressedKeys; });
};

Game.prototype.keyPressed = function(code) {
	var keys = {
		leftArrow: 37,
		upArrow: 38,
		rightArrow: 39,
		downArrow: 40
	};
	return this.pressedKeys[keys[code]] == true;
};

Game.prototype.start = function() {
	var game = this;
	
	$(window).keydown(function(event) {
		game._pressedKeys[event.which] = true;
	}).keyup(function(event) {
		game._pressedKeys[event.which] = false;
	}).focus();
	
	var lastTime = new Date().getTime();
	var gameLoop = function() {
		// Calculate ellapsed milliseconds since last loop
		var now = new Date().getTime();
		var milliseconds = now - lastTime;
		
		// Call the game Update function
		game.update(milliseconds);
		
		// Reset Game Canvas to the Color of the Background
		var context = game.canvas.getContext('2d');
		context.fillStyle = game._settings.background;
		context.fillRect(0, 0, $(game.canvas).width(), $(game.canvas).height());
		
		// Call the game Render function
		game.render(context);
		
		lastTime = now;
	};
	
	gameLoop();
	setInterval(gameLoop, 1000 / this.settings.fps);
};

Game.prototype.update = function(milliseconds) { }
Game.prototype.render = function(context) { }