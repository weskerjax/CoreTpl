

(function ($) {

	/* 規定是否使用傳統的方式淺層進行序列化(參數序列化)*/
	$.ajaxSettings.traditional = true;


	/**=(ezAjax)===============================================*/
	$.ezAjax = function (options) {

		var defaults = {
			traditional: true,
			global: false,
			type: 'POST',
			url: document.location,
			waitMsg: 'Waiting...',
			button: '',
			beforeSend: $.noop,
			success: function (msg) {
				StatusMsg.alert(msg);
			},
			error: $.noop,
			complete: $.noop,
			final: $.noop
		};

		options = $.extend(defaults, options);

		options.userBeforeSend = options.beforeSend;
		options.userSuccess = options.success;
		options.userError = options.error;
		options.userComplete = options.complete;

		options.beforeSend = function () {
			StatusMsg.wait(this.waitMsg);
			$.ezAjax.isProgress = true;

			/* 滑鼠等待圖示 */
			$('body').addClass('cursor-progress');


			/* 按鈕等待圖示 */
			var $button = $(this.button);
			if ($button.length > 0) {
				$button.prop('disabled', true).addClass('disabled');

				var $icon = $button;
				if (!$button.is('.fa')) {
					$icon = $button.find('.fa');
				}

				$icon.addClass('fa-spinner fa-spin');
			}

			this.userBeforeSend.apply(this, arguments);
		};

		options.success = function () {
			StatusMsg.clear();
			this.userSuccess.apply(this, arguments);
		};

		options.error = function (jqXHR, textStatus, errorThrown) {
			var showMsg = this.userError.apply(this, arguments);
			if (showMsg === false) { return; }
			StatusMsg.error(jqXHR.responseText);
		};


		options.complete = function (jqXHR, textStatus) {
			var isEnd = this.userComplete.apply(this, arguments);
			if (jqXHR.status == 200 && isEnd === false) { return; }

			/* 滑鼠等待圖示 */
			$('body').removeClass('cursor-progress');

			/* 按鈕等待圖示 */
			var $button = $(this.button);
			if ($button.length > 0) {
				$button.prop('disabled', false).removeClass('disabled');

				var $icon = $button;
				if (!$button.is('.fa')) {
					$icon = $button.find('.fa');
				}

				$icon.removeClass('fa-spinner fa-spin');
			}

			$.ezAjax.isProgress = false;

			this.final.apply(this, arguments);
		};


		/* 自動處理 JSON.stringify */
		if (options.contentType == 'application/json' && typeof (options.data) !== 'string') {
			options.data = JSON.stringify(options.data);
		}

		return $.ajax(options);
	};





	/**=(ezReload)===============================================
	 * $.ezReload(selector);
	 * $.ezReload(selector, isQuiet);
	 * $.ezReload(selector, callback);
	 * $.ezReload(selector, isQuiet, callback);
	 * */
	$.ezReload = function (selector, isQuiet, callback) {
		var handle = 'ezAjax';

		if (typeof (isQuiet) === 'function') {
			callback = isQuiet;
		} else if (isQuiet === true) {
			handle = 'ajax';
		}

		return $[handle]({
			type: 'GET',
			url: document.location,
			waitMsg: '重新載入...',
			success: function (result) {
				var $result = $('<div>').append($.parseHTML(result)).find(selector);

				$(selector).each(function (i) {
					if (!$result.get(i)) { return; }
					replaceHtml($(this), $result.eq(i));					
				}).trigger('content-change');

				if (callback) { callback(result); }
				$(window).trigger('reload.ez');
			}
		});
	};



	function replaceHtml($source, $target) {
		var $inputs = $source.clone().find(':input');
		$source.html($target.html());

		/* 回復表單輸入狀態 */
		$source.find(':input').each(function () {
			var $this = $(this);
			var $find = $inputs.filter('[id="' + $this.attr('id') + '"], [name="' + $this.attr('name') + '"]');

			if ($this.is(':radio, :checkbox')) {
				$find = $find.filter('[value="' + $this.val() + '"]');
				$this.prop('checked', $find.prop('checked'));
			}
			else {
				$this.val($find.val());
			}
		});
	}

})(jQuery);
