(function($) {

	/**
	 * Waits for all matched elements load and then calls the callback
	 * @params callback {Function} The function that will be called when all elements have been loaded
	 */
	$.fn.waitForLoad = function(callback) {
		var size = this.size();
		var loaded = 0;
		
		return this.each(function() {
			if (this.complete) {
				loaded += 1;
				if (loaded == size) callback();
			} else {
				$(this).load(function() {
					loaded += 1;
					if (loaded == size) callback();
				});
			}
		});
	};

})(jQuery);