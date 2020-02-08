

jQuery(function ($) {
    var idleLevel = 0;
    var limit = 40;

    /* 使用者有活動時才會進行 Interval */
    $.liveInterval = function (interval, func) {
        var pass = 0;

        return setInterval(function () {
            pass = pass > 0 ? Math.min(pass - 1, idleLevel) : idleLevel;
            if (idleLevel >= limit) { pass = idleLevel; }

            if (pass <= 0) { func(); }
        }, interval);
    };


    /*----------------------------------------------------*/

    var lastActive = $.now();
    var $iframe = $('iframe');

    var parent = null;
    if (window.parent !== window && $.isFunction(window.parent['_setUserIdleLevel'])) {
        parent = window.parent;
    }


    function setLevel(level, isUp) {
        lastActive = $.now();
        idleLevel = level;

        /* 向父視窗通知 */
        if (isUp && parent) { return parent._setUserIdleLevel(level, isUp); }

        /* 向子視窗通知 */
        $iframe
            .map(function () { return this.contentWindow })
            .filter(function () { return $.isFunction(this['_setUserIdleLevel']) })
            .each(function () { this._setUserIdleLevel(level, false); });
    }
    window._setUserIdleLevel = setLevel;


    /* 事件監聽 */
    $(document).on('mousemove', function () { setLevel(0, true); });
    $(window).on('focus', function () { setLevel(0, true); });
    $(window).on('blur', function () { setLevel(limit, true); });


    /* 週期性增加閒置等級 */
    var interval = 5000;
    setInterval(function () {
        if (($.now() - lastActive) < interval) { return; }
        idleLevel = Math.min(idleLevel + 1, limit);
    }, interval);
});
