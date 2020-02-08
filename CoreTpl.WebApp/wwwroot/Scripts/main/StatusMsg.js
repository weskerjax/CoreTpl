
/**=[訊息處理]==========================================================*/

var StatusMsg = {
	alertTpl: '<span class="alert alert-success twinkle"><i class="fa fa-info-circle fa-lg fa-fw"></i>${msg}</span>',
	warningTpl: '<span class="alert alert-warning twinkle"><i class="fa fa-warning fa-lg fa-fw"></i>${msg}</span>',
	errorTpl: '<span class="alert alert-danger twinkle"><i class="fa fa-exclamation-circle fa-lg fa-fw"></i>${msg}</span>',
	waitTpl: '<span class="alert alert-warning twinkle"><i class="fa fa-spinner fa-spin fa-lg fa-fw"></i>${msg}</span>',

	id: '#StatusMsg',
	timerId: null,
	clear: function () {
		$(this.id).empty();
		clearTimeout(this.timerId);
		$(window).off('.StatusMsg');
	},
	show: function (tpl, msg) { /*顯示訊息*/
		this.clear(); /*清除之前的訊息*/
		$(tpl.replace('${msg}', msg)).appendTo(this.id);
	},
	alert: function (msg) {
		this.show(this.alertTpl, msg); /*顯示訊息*/
		this.activeDelayRemove(); /*自動移除訊息*/
	},
	warning: function (msg) {
		this.show(this.warningTpl, msg); /*顯示訊息*/
		this.activeDelayRemove(); /*自動移除訊息*/
	},
	error: function (msg) {
		this.show(this.errorTpl, msg); /*顯示訊息*/
		this.activeDelayRemove(); /*自動移除訊息*/
	},
	wait: function (msg) {
		this.show(this.waitTpl, msg); /*顯示訊息*/
	},
	activeDelayRemove: function () {
		/* user 活動啟動延遲刪除 */
		$(window).one('mousemove.StatusMsg', function () {
			StatusMsg.timerId = setTimeout(function () {
				StatusMsg.clear();
			}, 10000);
		});
	}
};

jQuery(function ($) { StatusMsg.activeDelayRemove(); });




