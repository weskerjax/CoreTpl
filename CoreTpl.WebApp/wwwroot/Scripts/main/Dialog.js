
/**=[對話匡處理]==========================================================*/
var Dialog = {
	_hasParent: function () { return window.parent !== window && window.parent['Dialog']; },
	_closeId: 0,
	observes: {},
	on: function (event, callback) {
		if (!this.observes[event]) {
			this.observes[event] = [];
		}
		this.observes[event].push(callback);
		return this;
	},

	exec: function (event, data, params, path) {
		var callbacks = this.observes[event];
		if (!callbacks) { return this; }

		var i = callbacks.length;
		while (i--) { callbacks[i](data, params, path); }

		return this;
	},

	trigger: function (event, data) {
        if (this._hasParent()) {
            var params = {};
            var raw = location.search.substr(1).split('&') || [];
            for (var i = 0; i < raw.length; i++) {
                var single = raw[i].replace(/[+]/g, ' ').split('=');
                if (!single[0]) { continue; }
                params[decodeURIComponent(single[0])] = decodeURIComponent((single[1] || ''));
            }

            window.parent.Dialog.exec(event, data, params, location.pathname);
        }
        else {
            this.exec(event, data, params, location.pathname);
        }
		return this;
	},

	resize: function (width, height) {
		if (this._hasParent()) {
			window.parent.Dialog.resize(width, height);
		}
		else {
			$.iframeDialog.resize(width, height);
		}
		return this;
	},

	open: function (url, type) {
		return $.iframeDialog.open(url);
	},

	close: function (delay) {
		if (Dialog._hasParent()) {
			window.parent.Dialog.close(delay);
			return this;
		}

		clearTimeout(this._closeId);
		if (delay) {
			this._closeId = setTimeout(function () { $.iframeDialog.close(); }, delay);
		}
		else {
			$.iframeDialog.close(); 
		}
		return this;
	}

};

/*訊息*/
Dialog.on('alert.status-msg', function (msg) {
	StatusMsg.alert(msg);
}).on('error.status-msg', function (msg) {
	StatusMsg.error(msg);
});


jQuery(function ($) {

	var seqNum = ($.now() % 1000) * 100;


	/* Dialog */
	$(document).on('click', 'a[target=dialog]', function (event) {
		event.preventDefault();
		Dialog.open(this.href);
	});

	$(document).on('click', 'a[target=window]', function (event) {
		event.preventDefault();
		var subwin = window.open(this.href, 'subwin_' + (seqNum++), 'top=0,left=0,height=150,width=150,scrollbars=no,resizable=yes');
		subwin.focus();
	});


	$('#DialogLayout').delegate('[data-dismiss="modal"]', 'click.dismiss.modal', function () {
		Dialog.close();
	});

	if (Dialog._hasParent()) {
		$(document).on('keyup.dismiss.modal', function (e) {
			if ($(e.target).is(':input')) { return; }
			e.which == 27 && Dialog.close();
		});
	}
});






