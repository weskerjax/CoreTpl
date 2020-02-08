/* Use
 * $(document).on('konami', function () {
 *     console.log('konami code trigger');
 * });
 */

(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define(['jquery'], factory);
	} else if (typeof exports === 'object') {
		// Node/CommonJS style for Browserify
		module.exports = factory;
	} else {
		// Browser globals
		factory(jQuery);
	}
}(function ($) {

	var pattern = [38,38,40,40,37,39,37,39,65,66,65,66].join('');

	$.event.special.konami = {
		setup: function () {
			var input = '';

			$(this).on('keyup.konami', function (e) {
				if (e.keyCode == 20) { return; }
				input += e.keyCode;

				var over = input.length - pattern.length;
				if (over > 0) { input = input.substr(over); }
				if (input == pattern) { $(this).trigger('konami'); }
			});
		},
		teardown: function () {
			$(this).off('keyup.konami');
		}
	};
	 
}));
