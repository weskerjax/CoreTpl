
/*偵測 Permission denied to access property*/
try { window.opener && window.opener.document; }
catch(e) { window.opener = null; }



if (!Array.prototype.removeItem) {
	Array.prototype.removeItem = function (item) {
		var i = this.indexOf(item);
		if (i > -1) { this.splice(i, 1); }
		return this;
	}
}



if (!String.prototype.trim) {
	String.prototype.trim = function () {
		return $.trim(this);
	};
}

if ( !String.prototype.contains ) {
	String.prototype.contains = function() {
		return String.prototype.indexOf.apply( this, arguments ) !== -1;
	};
}

if ( !String.prototype.startsWith ) {
	String.prototype.startsWith = function() {
		return String.prototype.indexOf.apply( this, arguments ) === 0;
	};
}


/* String.format('你好{0}，歡迎你來到這裡', '小名'); */
String.format = function (pattern, param1, param2) {
    if (!pattern || arguments.length < 2) { return pattern || ''; }

    var args = Array.prototype.slice.call(arguments, 1);

    return pattern.replace(/\{(\d+)\}/g, function (m, i) {
        if (args[i] == undefined || args[i] == null) { return ''; }
        return args[i];
    });
};



/**設定頁面鎖定*/
window.exitAlert = function(msg) {
	if(msg){ 
		window.onbeforeunload = function(){ return msg; };
	}else{
		window.onbeforeunload = null;
	}
};





/**=()===============================================*/



$.log = (window['console'] && typeof(console.log) === 'function') ?
	function () { console.log.apply(console, arguments); } :
	$.noop;

$.fn.dump = function () {
	$.log(this);
	return this;
};

Array.prototype.dump = function () {
	$.log(this);
	return this;
};



$.fn.serializeObject = function(){
	var o = {};
	var a = this.serializeArray();
	$.each(a, function() {
		if (o[this.name] !== undefined) {
			if (!o[this.name].push) {
				o[this.name] = [o[this.name]];
			}
			o[this.name].push(this.value || '');
		} else {
			o[this.name] = this.value || '';
		}
	});
		
	return o;
};




/*
 * 	var urlList = [ { type: 'GET', url: 'JobCommand/OrderList', data: {} } ];
 */
$.fn.RequestTest = function (urlList, baseUrl) {
	var $table = $('<table class="table table-striped table-bordered table-hover">').appendTo(this);

	$.each(urlList, function (i, option) {
		var $tr = $('<tr>').appendTo($table);
		var $icon = $('<td class="icon min">').appendTo($tr);
		var $content = $('<td class="content">').appendTo($tr);
		var dataJson = option['data'] ? JSON.stringify(option.data) : '';
		$icon.append('<i class="fa fa-spinner fa-spin fa-fw fa-2x">');
		$content.append('<div>' + option.type + ' ' + option.url + ' ' + dataJson + '</div>');

		$.ajax({
			type: option.type,
			url: (baseUrl || '') + option.url,
			data: option.data,
			success: function () {
				$tr.addClass('text-success');
				$icon.html('<i class="fa fa-check fa-fw fa-2x"></i>');
			},
			error: function (jqXHR) {
				$tr.addClass('text-danger');
				$icon.html('<i class="fa fa-remove fa-fw fa-2x"></i>');
				$content.append('<div class="error">' + jqXHR.responseText + '</div>');
			}
		});
	});
};




/*取得時間性序號*/
var _seqNum = ($.now() % 1000) * 100;
$.seqNum = function () { return _seqNum++; };






/* jquery.validate.unobtrusive for bootstrap */
jQuery(function ($) {
	$('form').each(function () {
		var $this = $(this);
		$this.find('.input-validation-error').closest('.form-group').addClass('has-error');

		var validator = $this.data('validator');
		if (!validator) { return; }

		validator.settings._errorPlacement = validator.settings.errorPlacement;
		validator.settings.errorPlacement = function (error, inputEl) {
			validator.settings._errorPlacement(error, inputEl);
			$(inputEl).closest('.form-group').addClass('has-error');

		};

		validator.settings._success = validator.settings.success;
		validator.settings.success = function (error, inputEl) {
			validator.settings._success(error, inputEl);
			$(inputEl).closest('.form-group').removeClass('has-error');
		};
	});
});







jQuery(function ($) {
	var $win = $(window);


	$(document).on('click','[ext-delete-row]',function(){
		var rowSelect = $(this).attr('ext-delete-row') || 'tr';
		var $row = $(this).closest(rowSelect);
		var $parent = $row.parent();

		$row.trigger('row-deleting');
		$row.remove();
		$parent.trigger('row-deleted');

	}); 	
	

	/**=(下拉選單固定高度)===============================================*/
	$(document).on('shown.bs.dropdown', function (e) {
		var $menu = $(e.target).find('.dropdown-menu.fix-height');
		if ($menu.length === 0) { return; }

		var height = $win.scrollTop() + $win.height() - $menu.offset().top - 10;
		$menu.css('max-height', height);
		$menu.scroll(function (e) { e.stopPropagation(); });
	});
	
	
	/**=(權限替換)===============================================*/
	$(document).on('content-change init-deny', function () {
		$('[ext-deny]').each(function () {
			var $this = $(this);

			$this.replaceWith($('<span>', {
				'class': $this.attr('class') + ' disabled',
				'title': $this.attr('title') ? $this.attr('title') + '-權限不足' : '權限不足',
				'html': $this.html(),
			}));
		});
	}).triggerHandler('init-deny');
	
	


	/**=(表單改變時自動送出)===============================================*/
	$('[ext-change-submit]').change(function () {
		$(this).closest('form').submit();
	});

	/**=(表單改變時自動送出)===============================================*/
	$('[ext-change-to-page]').change(function (e) {
		e.preventDefault(); 
		e.stopPropagation();
		
		var $this = $(this);
		document.location = $this.attr('ext-change-to-page') + $this.val();
		return false;
	});




	/**=(點擊後送出指定表單)===============================================*/
	$('[ext-submit-form]').click(function () {
		$($(this).attr('ext-submit-form')).submit();
	});

	
	/**=(點擊後送出 POST 數值組 )===============================================*/
	$('[ext-post-values]').click(function () {
		var $this = $(this);
		var $form = $('<form method="post" action="">');
		var map = $this.attr('ext-post-values').toQueryObject();
		$.each(map, function(key, value) {
			$('<input type="hidden" />').attr('name',key).val(value).appendTo($form);
		});
		
		$.each(['action', 'target'], function(i, name) {
			var value = $this.attr(name); 
			if(value){ $form.attr(name, value); }
		});
		
		$form.appendTo('body').submit();
	});
	
	
	/**=(表單改變後的離開提醒)===============================================*/
	$('form[ext-exit-alert]').change(function () {
		window.exitAlert($(this).attr('ext-exit-alert'));
	});

	
	/**=(表單送出確認提醒)===============================================*/
	$('form[ext-submit-alert]').submit(function (event) {
		if (event.result === false) { return false; }
		return confirm($(this).attr('ext-submit-alert'));
	});
	


	/**=(選擇全部)===============================================*/
	$('[ext-select-all]').each(function () {
		var $this = $(this);
		var targetExpr = $this.attr('ext-select-all');
		
		$this.on('change.select-all', function () {
			var checked = $this.prop('checked');
			$(targetExpr).filter(':enabled').prop('checked', checked).trigger('change', ['select-all']);
		});
		

		var delayId = 0;
		$(document).on('change.select-all', targetExpr, function (e, flag) {
			if (flag == 'select-all') { return; }
			if (delayId) { return; }

			delayId = setTimeout(function () {
				var $targets = $(targetExpr).filter(':enabled');
				var count = $targets.filter(':checked').length;

				$this.prop('indeterminate', 0 < count && count < $targets.length);
				$this.prop('checked', count === $targets.length);

				delayId = 0;
			}, 100);
		})

		$(targetExpr).eq(0).trigger('change.select-all');
	});


	

	/**=(對話框改變後 reload)===============================================*/
	if ($('[dialog-change-reload]').length > 0) {
		Dialog.on('change', function (data) {
			$.ezReload('[dialog-change-reload]', true);
		});
	}


	

	/**=(元素樣式切換)===============================================*/
	/* <button ext-toggle="Hidden" ext-expr="#mng_list">切換</button>
	 * <div id="mng_list" class="Hidden"></div>
	 */
	$('[ext-toggle]').css('cursor', 'pointer').on('click.toggle', function () {
		$($(this).attr('ext-expr')).toggleClass($(this).attr('ext-toggle'));
	});

	
	



	/**=(隨著捲動水平固定)===============================================*/
	var $horizontalFix = $('[ext-horizontal-fix]');
	if ($horizontalFix.length > 0) {
		$win.scroll(function () {
			var delta = Math.max(0, $win.scrollLeft());
			$horizontalFix.css('transform', delta ? 'translateX(' + delta + 'px)' : '');
		});
	}


	/**=(隨著捲動的工具列)===============================================*/
	$('[ext-scroll-follow]').each(function () {
		var $toolbar = $(this);
		$toolbar.css({
			'z-index': '10',
			'position': 'relative',
		});
		$toolbar.data('offsetTop', $toolbar.offset().top);

		$win.scroll(function () {
			var offsetTop = $toolbar.data('offsetTop');

			if ($win.scrollTop() > offsetTop) {
				var relativeTop = $win.scrollTop() - offsetTop;
				$toolbar.css('top', relativeTop);
			} else {
				/*解除定位*/
				$toolbar.css('top', '0');
			}
		});
	});

	$win.scroll();



	/**=(選取標記)===============================================
	 * <tbody ext-selected-mark="tr => ui-selected">
	 */
	$(document).on('change init-selected', '[ext-selected-mark]', function () {
		var split = $(this).attr('ext-selected-mark').split('=>');
		var closest = split[0];
		var style = split[1];

		$(this).find(':radio, :checkbox').each(function () {
			$(this).closest(closest).toggleClass(style, this.checked);
		});
	});

	$('[ext-selected-mark]').trigger('init-selected');




	/**=(可選取的核選框)===============================================
	 * <tbody ext-selectable-checkbox="tr">
	 * <tbody ext-selectable-checkbox="tr => .Checkbox :checkbox">
	 */
	$('[ext-selectable-checkbox]').each(function () {
		var $base = $(this);
		var split = $base.attr('ext-selectable-checkbox').split('=>');
		var filter = split[0];
		var checkboxExpr = split[1] || ':checkbox';
		var cancelSelector = ':input, label, a, .btn, .text-select';

		$base.selectable({
			cancel: cancelSelector,
			filter: filter,
			stop: function () {
				$base.find(checkboxExpr).prop("checked", false);
				$base.find('.ui-selected').find(checkboxExpr).prop("checked", true);
				$base.find(checkboxExpr).trigger('change', ['selectable']);
			}
		});

		$base.on('change init-selected', filter, function (e, flag) {
			if (flag == 'selectable') { return; }
			if (!$(e.target).is(checkboxExpr)) { return; }
			$(this).toggleClass('ui-selected', e.target.checked);
		});
		$base.find(checkboxExpr).filter(':checked').trigger('init-selected');
		
		
		/* 讓文字可以選取 */
		$base.on('content-change init-text-select', function (e) {
			$base.find('td').each(function () {
				var $td = $(this);
				var $child = $td.children(':first');
				if ($child.is(cancelSelector)) { return; }

				if ($child.length == 0) { $td.html('<span class="text-select">' + $td.html() + '</span>'); }
				else { $child.addClass('text-select'); }
			});
		});

		setTimeout(function () { $base.triggerHandler('init-text-select'); }, 4000);
	});


	
});





/*#[ ext-table-fix, ext-table-fix-base 表格標頭固定 ]#############################################*/

jQuery(function ($) {
	var $win = $(window);

	/*水平捲動處理*/
	$('table').has('th[ext-table-fix], td[ext-table-fix]').each(function () {
		var $table = $(this).addClass('table-scroll-fix');
		var $columns = $table.find('th, td').filter('[ext-table-fix]').css('z-index', 10);
		
		$table.find('tr').each(function(){
			$(this).find('[ext-table-fix]:last').addClass('last');
		});
		
		var startLeft = 0;
		var $base = $table.closest('[ext-table-fix-base]');
		if($base.length === 0){
			$base = $win;
			startLeft = $columns.offset().left - (parseInt($('body').css('padding-left')) || 0);
		}
		$base.on('scroll', function() {
			var delta = Math.max(0, $(this).scrollLeft() - startLeft);
			$columns.css('transform', delta ? 'translateX('+delta+'px)' : '');
		}).triggerHandler('scroll');        
	});
	
	

	/*水平捲動處理-固定末端欄位*/
	$('table').has('th[ext-fix-last], td[ext-fix-last]').each(function () {
		var $table = $(this).addClass('table-scroll-fix');
		var $base = $table.closest('[ext-table-fix-base]');
		var $columns = $table.find('th, td').filter('[ext-fix-last]').css('z-index', 10);
		$columns.filter('td').css('background-color', '#fff');

		var startLeft = 0;

		$(window).on('resize selected.orderable', function () {
			startLeft = $table.width() - $base.prop("clientWidth");
			$base.triggerHandler('scroll');
		}).triggerHandler('resize');

		$base.on('scroll', function () {
			var delta = Math.min(0, $base.scrollLeft() - startLeft);
			$columns.css('transform', delta ? 'translateX(' + delta + 'px)' : '');
		}).triggerHandler('scroll');
	});



	/*垂直捲動處理*/
	$('table').has('thead[ext-table-fix], tr[ext-table-fix]').each(function () {
		var $table = $(this).addClass('table-scroll-fix');
		var $rows = $table.find('thead, tr').filter('[ext-table-fix]').css('z-index', 11);

		var offset = 0;
		var startTop = 0;
		var $base = $table.closest('[ext-table-fix-base]');
		if($base.length === 0){ 
			$base = $win;
			startTop = $rows.offset().top - offset;
		}
		
		$base.on('scroll', function() {
			var delta = Math.max(0, $base.scrollTop() - startTop);
			if(delta){
				$rows.css('transform', 'translateY('+delta+'px)'); 
			}else{
				$rows.css('transform', '');
				if (startTop) { startTop = $rows.offset().top - offset; } /* 重新取得定位 */
			}
		}).triggerHandler('scroll');        
	});
	
	
	/* 將 thead 移到 table 最後面 */
	$('thead[ext-table-fix]').has('[ext-table-fix]').each(function () {
		$(this).appendTo($(this).parent());
	});
});




$(window).load(function () {
	setTimeout(function () {
		$('form').submit(function (event) {
			if (event.result === false) { return false; }
			
			/**將表單送出前自動解鎖留到最後才處理，這樣在資料檢查未通過時，就不會執行到這裡*/
			window.exitAlert(null); 

			/**只能送出一次的處理 one-submit="btnSelector" */
			var $form = $(this);
			if (!$form.is('[ext-one-submit]')) { return; }

			if ($form.data('submit-flag')) { return false; }
			$form.data('submit-flag', true);
			
			/**滑鼠等待圖示*/
			$('body').addClass('cursor-wait');


			/**等待訊息*/
			if ($form.is('[ext-submit-wait]')) {
				StatusMsg.wait($form.attr('ext-submit-wait'));
			}

			
			/**送出按鈕處理*/
			var btnSelector = $form.attr('ext-one-submit') || '[type=submit]';
			var $button = $form.find(btnSelector);
			$button.addClass('disabled');
			
			var $icon = $button;
			if (!$button.is('.fa')) {
				$icon = $button.find('.fa');
			}
			$icon.addClass('fa-spinner fa-spin');
		});
	}, 100);
});
