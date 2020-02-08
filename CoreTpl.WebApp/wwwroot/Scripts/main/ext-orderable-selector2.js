/**=(欄位順序選擇)===============================================*/

(function ($) {
	"use strict";

	var ellipsisColumn = '_ellipsisExpand';



	/* class Orderable */
	function Orderable(el, options) {
		var self = this;

		$.extend(self, {
			targetId: '',
			showEllipsis: false,
			columnStatus: null,
		}, options);

		self.$menu = $(el);
		self.$target = $(self.targetId);
		self.orderableId = '#Orderable_' + self.targetId.substr(1);

		if (self.showEllipsis) {
			self._initEllipsisBtn();
		}


		self._initMenu();
		self.setColumnStatus(self.columnStatus);

		/*觸發初始完成事件*/
		setTimeout(function () {
			self.$menu.trigger('orderable-init');
		}, 100);
	}



	Orderable.prototype = {

		/*初始化 展開/收合文字 */
		_initEllipsisBtn: function () {
			var self = this;

			$('<li class="divider"></li>').prependTo(self.$menu);

			var $ellipsis = $(
				'<li class="EllipsisToggleBtn cursor-pointer">' +
					'<label class="item">' +
						'<input class="hide" type="checkbox" value="' + ellipsisColumn + '" />' +
						'<span class="ExpandText"><i class="fa-dm fa fa-text-width text-success"></i> 展開文字</span>' +
						'<span class="CollapseText hide"><i class="fa-dm fa fa-text-width text-danger"></i> 收合文字</span>' +
					'</label>' +
				'</li>'
			);

			$ellipsis.prependTo(self.$menu).on('change init-ellipsis', function () {
				var checked = $ellipsis.find(':checkbox').prop('checked');
				$ellipsis.find('.ExpandText').toggleClass('hide', checked);
				$ellipsis.find('.CollapseText').toggleClass('hide', !checked);
			});
						
			var checked = self.columnStatus && (self.columnStatus[ellipsisColumn] || false);
			$ellipsis.find(':checkbox').prop('checked', checked).trigger('init-ellipsis');
		},



		/* 初始化 Field */
		_initMenu: function () {
			var self = this;
			self.$sort = $('<span>').appendTo(self.$menu);


			/*初始化欄位選項*/
			self.$target.find('[ext-orderable-col]').each(function () {
				var $col = $(this);
				var colName = $col.attr('ext-orderable-col') || $col.attr('class');
				colName = colName.split(' ')[0];
				$('<li><label class="item"><input type="checkbox" value="' + colName + '" />' + $col.text() + '</label></li>').appendTo(self.$sort);
			});


			/*初始化選擇狀態＆順序*/
			if (!self.columnStatus) {
				self.$sort.find(':checkbox').prop('checked', true);
				self._keepColumnStatus();
			}


			/*全選處理*/
			var $checkbox = self.$sort.find(':checkbox');
			var $selectAll = $('<li><label class="item"><input type="checkbox" />=全選=</label></li>').insertBefore(self.$sort).find(':checkbox');

			$selectAll.on('change.select-all', function () {
				$checkbox.prop('checked', $selectAll.prop('checked'));
				self._keepColumnStatus();
			});

			var delayId = 0;
			self.$sort.on('change.select-all init', function () {
				if (delayId) { return; }

				delayId = setTimeout(function () {
					var count = $checkbox.filter(':checked').length;

					$selectAll.prop('indeterminate', 0 < count && count < $checkbox.length);
					$selectAll.prop('checked', count === $checkbox.length);

					delayId = 0;
				}, 100);
			}).triggerHandler('init');

			

			/*欄位順序*/
			self.$sort.sortable({
				opacity: 0.7,
				cursor: 'move',
				axis: 'y',
				update: function (e, ui) {
					self._keepColumnStatus();
					self._updateColumnOrder();
					self.$menu.trigger('sorted.orderable');
				}
			});


			/* 顯示/隱藏 */
			self.$menu.change(function (e) {
				/*阻擋事件傳遞*/
				e.stopImmediatePropagation();
				e.stopPropagation();

				self._keepColumnStatus();
				self._updateColumnVisible();
				self.$menu.trigger('selected.orderable');
			});


			/*阻擋事件傳遞*/
			self.$menu.click(function (e) {
				e.stopImmediatePropagation();
				e.stopPropagation();
			});


			/*內容改變*/
			self.$target.on('content-change', function (e) {
				self._updateColumnOrder();
				self._updateColumnVisible();
			});
		}, 



		/*紀錄欄位狀態*/
		_keepColumnStatus: function () {
			var self = this;
			var status = {};

			self.$menu.find(':checkbox').each(function () {
				var $this = $(this);
				status[$this.val()] = $this.prop('checked');
			});

			self.columnStatus = status;
			self.$menu.trigger('stored.orderable', [self.targetId, status]);
		},


		/*更新欄位顯示*/
		_updateColumnVisible: function () {
			var self = this;
			var styles = [];

			var temp = $.map(self.columnStatus, function (checked, column) {
				if (column == ellipsisColumn) { return null; }

				return checked ? null : self.targetId + ' .' + column;
			});
			if (temp.length > 0) { styles.push(temp.join(', ') + '{display:none}'); }

			if (self.columnStatus[ellipsisColumn]) { styles.push(self.targetId + ' .ellipsis {display:inline}'); }

			$(self.orderableId).remove();

			if (styles.length === 0) { return; }
			$('<style id="' + self.orderableId.substr(1) + '" type="text/css">' + styles.join(' ') + '</style>').appendTo('head');
		},


		/*更新欄位順序*/
		_updateColumnOrder: function () {
			var self = this;
			var columns = $.map(self.columnStatus, function (checked, column) { return '.' + column; });
			if (columns.length == 0) { return; }

			var $trs = self.$target.find('tr');
			var $child = self.$target.children().detach(); /* 利用 detach 加快速度 */

			$trs.each(function () {
				var $tr = $(this);

				var $order = $(columns).map(function (i, col) { return $tr.find(col)[0]; });
				if ($order.length > 1) { $order.first().after($order); }
			});

			self.$target.append($child);
		},
		
		
		setColumnStatus: function (columnStatus) {
			var self = this;

			self.columnStatus = columnStatus;
			
			$.each(self.columnStatus, function (column, checked) {
				var $checkbox = self.$sort.find(':checkbox[value="' + column + '"]');
				$checkbox.prop('checked', checked);
				$checkbox.closest('li').appendTo(self.$sort);
			});

			self._updateColumnOrder();
			self._updateColumnVisible();
		}		 
	};





	$.fn.orderable = function (options) {
		var args = Array.prototype.slice.call(arguments, 1);

		if (typeof (options) === 'string') {
			var $el = this.eq(0);
			var instance = $el.data('Orderable');
			if (!instance || !instance[options]) { return instance; }

			if ($.isFunction(instance[options])) {
				return instance[options].apply(instance, args);
			} else {
				return instance[options];
			}
		}

		return this.each(function () {
			var $el = $(this);
			var instance = $el.data('Orderable');
			if (instance) { return; }

			instance = new Orderable(this, options);
			$el.data('Orderable', instance);
		});
	};



	/* unobtrusive API */
	jQuery(function ($) {
		$('[ext-orderable-selector]').each(function () {
			var $this = $(this);
			var targetId = $this.attr('ext-orderable-selector');
			var orderableId = '#Orderable_' + targetId.substr(1);
			var columnStatus = $.parseJSON(localStorage[orderableId] || 'null');
			
			$this.orderable({
				targetId: targetId,
				columnStatus: columnStatus,
			});
			
			$this.on('stored.orderable', function (e, id, columnStatus) {
				localStorage[orderableId] = JSON.stringify(columnStatus);
			});
		});
	});

})(window.jQuery);



 