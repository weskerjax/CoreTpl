
/**=( 輔助輸入 ext-typeahead )===============================================*/
jQuery(function ($) {

    function source(query, callback, init) {
        if (!$.trim(query)) {
            this.$element.triggerHandler('selected.typeahead');
            return;
        }

        var data = 'query=' + encodeURIComponent(query);
        var url = $.trim(this.$element.attr('ext-typeahead'));

        var params = $.trim(this.$element.attr('ext-typeahead-params'));
        if (params) { data += '&' + params; }

        /* 中斷之前的請求 */
        if (this._xhr && this._xhr.state() == 'pending') {
            this._xhr.abort();
        }

        var ajax = init ? 'ajax' : 'ezAjax';
        this._xhr = $[ajax]({
            type: 'POST',
            url: url,
            data: data,
            waitMsg: '查詢中...',
            success: callback
        });
    }


    $(document).on('init.typeahead focus.typeahead', '[ext-typeahead]', function (e) {
        var $this = $(this);
        if ($this.data('typeahead')) { return; }

        var format = $this.attr('ext-typeahead-format') || '${Value} ${Label}';

        $this.typeahead({
            items: 40,
            delay: 800,
            minLength: 1,
            autoSelect: false,
            menuMaxHeight: 200,
            source: source,
            updater: function (item) {
            	return item.Value;
            },
            itemToString: function (item) {
                return $.tmpl('<div>' + format + '</div>', item).html();
            }
        });

        $this.on('init.typeahead selected.typeahead', function (event, item) {
            var label = item ? item.Label : '';
            var displayId = $this.attr('ext-typeahead-display');
            if (!displayId) {
                displayId = '#' + $this.attr('id') + '-display';
            }
            var $display = $(displayId);

            if ($display.is(':input')) {
                $display.val(label);
            } else {
                $display.html(label);
            }
        });


        /* 關閉 Browser 原本的 AutoComplete */
        $this.attr('autocomplete', 'off');

        var query = $.trim($this.val());
        if (!query) { return; }

        /* 初次顯示 */
        $this.typeahead('source', query, function (items) {
            $.each(items, function (i, item) {
                if (item && item.Value != query) { return; }
                $this.trigger('init.typeahead', [item]);
                return false;
            });
        }, true);
    });


    /* 觸發初始事件 */
    $('[ext-typeahead]').trigger('init.typeahead');
});

