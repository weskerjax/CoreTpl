
/**=(顯示選擇)===============================================*/

jQuery(function ($) {

	function keepHiddenSelect(visibleId, $base) {
		var hiddenSelect = $base.find(':checkbox:not(:checked)')
			.map(function () { return this.value; }).toArray();

		localStorage[visibleId] = JSON.stringify(hiddenSelect);
	}


	function updateVisible(visibleId, targetId) {
		$(visibleId).remove();

		var hiddenSelect = $.parseJSON(localStorage[visibleId] || '[]');
		var temp = $.map(hiddenSelect, function (value) { return targetId + ' .' + value; });
		if (temp.length == 0) { return; }

		$('<style id="' + visibleId.substr(1) + '" type="text/css">' + temp.join(', ') + '{display:none;}</style>').appendTo('head');
	}
	

	$('[ext-visible-selector]').each(function () {
		var $base = $(this);
		var targetId = $base.attr('ext-visible-selector');
		var visibleId = '#Visible_' + targetId.substr(1);
		
		/*初始化選擇狀態*/
		var hiddenSelect = $.parseJSON(localStorage[visibleId] || '[]');
		$base.find(':checkbox').prop('checked', function () {
			return $.inArray(this.value, hiddenSelect) == -1;
		});
		

		$base.change(function (e) {
			/*阻擋事件傳遞*/
			e.stopImmediatePropagation();
			e.stopPropagation();

			keepHiddenSelect(visibleId, $base);
			updateVisible(visibleId, targetId);
			$base.trigger('selected.visible');
		});


		/*阻擋事件傳遞*/
		$base.click(function (e) {
			e.stopImmediatePropagation();
			e.stopPropagation();
		});


		/*內容改變*/
		$(targetId).on('content-change', function () {
			updateVisible(visibleId, targetId);
		});
	});


	/*觸發初始完成事件*/
	setTimeout(function () {
		$('[ext-visible-selector]').trigger('visible-init');
	}, 100);

});
