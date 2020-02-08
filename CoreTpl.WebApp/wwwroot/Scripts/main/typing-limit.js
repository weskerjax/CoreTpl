

/**=(UI 輸入限制)===============================================*/

var typingLimitHandler = {};


/** 輔助輸入 */
typingLimitHandler['uint'] = function (el) { /*無符號整數*/
	$(el).keydown(function (e) {
		if (e.key == '.' || e.key == '-') { return false; }

		if (e.altKey || e.ctrlKey) { return; }
		if (e.keyCode < 58 || e.keyCode > 90) { return; }
		e.preventDefault();
	});

	$(el).keyup(function () {
		var m = this.value.match(/(([0-9]*)?)/);
		this.value = m ? m[0] : '';
	});
};

typingLimitHandler['int'] = function (el) { /*整數*/
	$(el).keydown(function (e) {
		var val = $.trim(this.value);
		if (e.key == '-' && val) { return false; }
		if (e.key == '.') { return false; }

		if (e.altKey || e.ctrlKey) { return; }
		if (e.keyCode < 58 || e.keyCode > 90) { return; }
		e.preventDefault();
	});

	$(el).keyup(function () {
		var m = this.value.match(/(-?([0-9]*)?)/);
		this.value = m ? m[0] : '';
	});
};

typingLimitHandler['number'] = function (el) { /*浮點數*/
	$(el).keydown(function (e) {
		var val = $.trim(this.value);
		if (e.key == '-' && val) { return false; }
		if (e.key == '.' && ~val.indexOf('.')) { return false; }

		if (e.altKey || e.ctrlKey) { return; }
		if (e.keyCode < 58 || e.keyCode > 90) { return; }
		e.preventDefault();
	});

	$(el).keyup(function () {
		var m = this.value.match(/(-?([0-9]*)?)(\.[0-9]*)?/);
		this.value = m ? m[0] : '';
	});
};

typingLimitHandler['id'] = function (el) { /*帳號格式*/
	$(el).keyup(function () {
		var m = this.value.match(/[a-z][a-z0-9_\.]*/);
		this.value = m ? m[0] : '';
	});
};

typingLimitHandler['alnum'] = function (el) { /*保留字母和数字及底線*/
	$(el).keyup(function () {
		var m = el.value.match(/[a-z0-9_]*/i);
		this.value = m ? m[0] : '';
	});
};

typingLimitHandler['upper'] = function (el) { /*英文大寫*/
	$(el).css('text-transform', 'uppercase').change(function () {
		this.value = this.value.toUpperCase();
	});
};



jQuery(function ($) {
	$(document).on('init.typing-limit focus', '[typing-limit]', function () {
		var $this = $(this);
		var type = $this.attr('typing-limit');
		if ($this.data('has-typing-limit')) { return; }

		if (typeof typingLimitHandler[type] === 'function') {
			typingLimitHandler[type](this);
			$(this).data('has-typing-limit', true);
		}
	});
});

