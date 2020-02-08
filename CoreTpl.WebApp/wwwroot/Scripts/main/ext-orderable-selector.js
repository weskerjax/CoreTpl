
/**=(欄位順序選擇)===============================================*/

jQuery(function ($) {

	/*紀錄欄位狀態*/
	function keepColumnStatus(orderableId, $base) {
		var columnStatus = {};
		$base.find(':checkbox').each(function () {
			var $this = $(this);
			columnStatus[$this.val()] = $this.prop('checked');
		});
		
		localStorage[orderableId] = JSON.stringify(columnStatus);		
		$base.trigger('stored.orderable', [orderableId, columnStatus]);
	}


	/*更新欄位顯示*/
	function updateColumnVisible(orderableId, targetId) {
		var columnStatus = $.parseJSON(localStorage[orderableId] || '{}');

		var styles = [];

		var temp = $.map(columnStatus, function (checked, column) {
			if (column == '_ellipsisExpand') { return null; }

			return checked ? null : targetId + ' .' + column;
		});
		if (temp.length > 0) { styles.push(temp.join(', ') + '{display:none}'); }

		if (columnStatus._ellipsisExpand) { styles.push(targetId + ' .ellipsis {display:inline}'); }

		$(orderableId).remove();

		if (styles.length === 0) { return; }
		$('<style id="' + orderableId.substr(1) + '" type="text/css">' + styles.join(' ') + '</style>').appendTo('head');
	}


	/*更新欄位順序*/
	function updateColumnOrder(orderableId, targetId) {
		var columnStatus = $.parseJSON(localStorage[orderableId] || '{}');
		var columns = $.map(columnStatus, function (checked, column) { return '.' + column; });
		if (columns.length == 0) { return; }

		var $target = $(targetId);
		var $trs = $target.find('tr');
		var $child = $target.children().detach(); /* 利用 detach 加快速度 */

		$trs.each(function () {
			var $tr = $(this);

			var $order = $(columns).map(function (i, col) { return $tr.find(col)[0]; });
			if ($order.length > 1) { $order.first().after($order); }
		});

		$target.append($child);
	}
	 



	$('[ext-orderable-selector]').each(function () {
		var $base = $(this);
		var $sort = $('<span>').appendTo($base);
		var targetId = $base.attr('ext-orderable-selector');
		var orderableId = '#Orderable_' + targetId.substr(1);

		
		/*初始化 展開/收合文字 */
		if ($base.attr('ext-ellipsis-toggle') == 'true') {
			$('<li class="divider"></li>').prependTo($base);

			var $ellipsis = $(
				'<li class="EllipsisToggleBtn cursor-pointer">'+
					'<label class="item">' +
						'<input class="hide" type="checkbox" value="_ellipsisExpand" />' +
						'<span class="ExpandText"><i class="fa-dm fa fa-text-width text-success"></i> 展開文字</span>' +
						'<span class="CollapseText hide"><i class="fa-dm fa fa-text-width text-danger"></i> 收合文字</span>' +
					'</label>' +
				'</li>'
			);
			
			$ellipsis.prependTo($base).on('change init-ellipsis', function () {
				var checked = $ellipsis.find(':checkbox').prop('checked');
				$ellipsis.find('.ExpandText').toggleClass('hide', checked);
				$ellipsis.find('.CollapseText').toggleClass('hide', !checked);
			});

			var columnStatus = $.parseJSON(localStorage[orderableId] || '{}');
			var checked = columnStatus['_ellipsisExpand'] || false;			
			$ellipsis.find(':checkbox').prop('checked', checked).trigger('init-ellipsis');
		}



		/*初始化欄位選項*/
		$(targetId).find('[ext-orderable-col]').each(function () {
			var $col = $(this);
			var colName = $col.attr('ext-orderable-col') || $col.attr('class');
			colName = colName.split(' ')[0];
			$('<li><label class="item"><input type="checkbox" value="' + colName + '" />' + $col.text() + '</label></li>').appendTo($sort);
		});


		/*初始化選擇狀態＆順序*/
		if (!localStorage[orderableId]) {
			$sort.find(':checkbox').prop('checked', true);
		}
		else {
			var columnStatus = $.parseJSON(localStorage[orderableId] || '{}');
			$.each(columnStatus, function (column, checked) {
				var $checkbox = $sort.find(':checkbox[value="' + column + '"]');
				$checkbox.prop('checked', checked);
				$checkbox.closest('li').appendTo($sort);
			});

			updateColumnOrder(orderableId, targetId);
			updateColumnVisible(orderableId, targetId);
		}


		/*全選處理*/
		var $checkbox = $sort.find(':checkbox');
		var $selectAll = $('<li><label class="item"><input type="checkbox" />=全選=</label></li>').insertBefore($sort).find(':checkbox');

		$selectAll.on('change.select-all', function () {
			$checkbox.prop('checked', $selectAll.prop('checked'));

			keepColumnStatus(orderableId, $base);
			updateColumnVisible(orderableId, targetId);
			$base.trigger('selected.orderable');
		});

		var delayId = 0;
		$sort.on('change.select-all init', function () {
			if (delayId) { return; }

			delayId = setTimeout(function () {
				var count = $checkbox.filter(':checked').length;

				$selectAll.prop('indeterminate', 0 < count && count < $checkbox.length);
				$selectAll.prop('checked', count === $checkbox.length);

				delayId = 0;
			}, 100);
		}).triggerHandler('init');



		/*欄位順序*/
		$sort.sortable({
			opacity: 0.7,
			cursor: 'move',
			axis: 'y',
			update: function (e, ui) {
				keepColumnStatus(orderableId, $base);
				updateColumnOrder(orderableId, targetId);
				$base.trigger('sorted.orderable');
			}
		});


		/* 顯示/隱藏 */
		$sort.change(function (e) {
			/*阻擋事件傳遞*/
			e.stopImmediatePropagation();
			e.stopPropagation();

			keepColumnStatus(orderableId, $base);
			updateColumnVisible(orderableId, targetId);
			$base.trigger('selected.orderable');
		});


		/*阻擋事件傳遞*/
		$base.click(function (e) {
			e.stopImmediatePropagation();
			e.stopPropagation();
		});


		/*內容改變*/
		$(targetId).on('content-change', function (e) {
			updateColumnOrder(orderableId, targetId);
			updateColumnVisible(orderableId, targetId);
		});
	});


	/*觸發初始完成事件*/
	setTimeout(function () {
		$('[ext-orderable-selector]').trigger('orderable-init');
	}, 100);

});
