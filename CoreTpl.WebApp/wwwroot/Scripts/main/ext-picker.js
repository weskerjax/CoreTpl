

moment.lang('zh-tw', {
	months: '一月_二月_三月_四月_五月_六月_七月_八月_九月_十月_十一月_十二月'.split('_'),
	monthsShort: '1月_2月_3月_4月_5月_6月_7月_8月_9月_10月_11月_12月'.split('_'),
	weekdays: '星期日_星期一_星期二_星期三_星期四_星期五_星期六'.split('_'),
	weekdaysShort: '週日_週一_週二_週三_週四_週五_週六'.split('_'),
	weekdaysMin: '日_一_二_三_四_五_六'.split('_'),
	longDateFormat: {
		LT: 'HH:mm',
		L: 'YYYY-MM-DD',
		LL: 'YYYY-MM-DD',
		LLL: 'YYYY-MM-DD LT',
		LLLL: 'YYYY-MM-DD LT',
		l: 'YYYY-MM-DD',
		ll: 'YYYY-MM-DD',
		lll: 'YYYY-MM-DD LT',
		llll: 'YYYY-MM-DD LT'
	},
	meridiem: function (hour, minute, isLower) {
		return '';
	},
	calendar: {
		sameDay: '[今天]LT',
		nextDay: '[明天]LT',
		nextWeek: '[下]ddddLT',
		lastDay: '[昨天]LT',
		lastWeek: '[上]ddddLT',
		sameElse: 'L'
	},
	ordinal: function (number, period) {
		switch (period) {
			case 'd':
			case 'D':
			case 'DDD':
				return number + '日';
			case 'M':
				return number + '月';
			case 'w':
			case 'W':
				return number + '週';
			default:
				return number;
		}
	},
	relativeTime: {
		future: '%s內',
		past: '%s前',
		s: '幾秒',
		m: '一分鐘',
		mm: '%d分鐘',
		h: '一小時',
		hh: '%d小時',
		d: '一天',
		dd: '%d天',
		M: '一個月',
		MM: '%d個月',
		y: '一年',
		yy: '%d年'
	}
});

$.extend($.fn.datetimepicker.defaults, {
	useMinutes: true, // en/disables the minutes picker
	useSeconds: true, // en/disables the seconds picker
	useCurrent: false, // when true, picker will set the value to the current date/time
	showToday: true,  // shows the today indicator
	useStrict: true,  // use "strict" when validating dates
	collapse: true,  // use "strict" when validating dates
	language: 'zh-tw', // sets language locale strings and moment objects
	icons: {
		time: 'fa fa-clock-o',
		date: 'fa fa-calendar',
		up: 'fa fa-arrow-up',
		down: 'fa fa-arrow-down'
	}
});



/**=( 輔助輸入 ext-picker )===============================================*/
jQuery(function ($) {

	var twOptions = {
		transferOutYear: function (year) { return year - 1911; },
		transferInDate: function (dateStr) {
			if (!dateStr) { return ''; }

			return dateStr.replace(/^[0-9]+\b/, function (year) {
				return parseInt(year, 10) + 1911;
			});
		}
	};

	/* DateTimePicker */
	$(document).on('focus.picker', '[ext-picker]', function (e) {
		var $this = $(this);
		if ($this.data('DateTimePicker')) { return; }

		var format = $this.attr('ext-format');

		switch ($this.attr('ext-picker')) {
			case 'datetime':
				$this.datetimepicker({ format: (format || 'yyyy-MM-dd HH:mm:ss') }); break;
			case 'date':
				$this.datetimepicker({ format: (format || 'yyyy-MM-dd'), pickTime: false }); break;
			case 'time':
				$this.datetimepicker({ format: (format || 'HH:mm:ss'), pickDate: false }); break;
			case 'tw-date':
			case 'cn-date':
				$this.datetimepicker($.extend({}, twOptions, { format: (format || 'yyyy-MM-dd'), pickTime: false }));
				break;
			case 'tw-datetime':
			case 'cn-datetime':
				$this.datetimepicker($.extend({}, twOptions, { format: (format || 'yyyy-MM-dd HH:mm:ss') }));
				break;
		}
	});


});

