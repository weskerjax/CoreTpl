

/*#[ title 屬性 tip ]####################################################################################*/
jQuery(function ($) {
    var $tip = $('<span class="title-tip"></span>').appendTo('body');

    $tip.hide = function () {
        $tip.removeClass('title-tip-show');
        clearTimeout($tip.timer);
        setTimeout(function () { $tip.css('top', -100); }, 300);
    };

    $tip.show = function ($el, tipText) {
        $tip.hide();

        $tip.timer = setTimeout(function () {
            var winWidth = $(window).width();
            var width = $el.outerWidth();
            var offset = $el.offset();
            var pos = {
                top: offset.top - 28,
                left: offset.left - Math.max(0, 18 - width / 2),
                right: winWidth - offset.left - width
            };

            if (pos.top < $(window).scrollTop() + 120) {
                pos.top = offset.top + $el.outerHeight() + 10;
                $tip.addClass('title-tip-bottom');
            } else {
                $tip.removeClass('title-tip-bottom');
            }

            if (pos.left > winWidth - 120) {
            	pos.left = 'auto';
                $tip.addClass('title-tip-right');
            } else {
            	pos.right = 'auto';
                $tip.removeClass('title-tip-right');
            }

            $tip.css(pos);
            $tip.html(tipText);
            $tip.addClass('title-tip-show');
        }, 400);
    };

    $(document).on('mouseenter', '[title]', function () {
        var $this = $(this);
        var tipText = $this.attr('title');

        if (typeof (tipText) !== 'string') { return; }
        $this.removeAttr('title');

        tipText = $.trim(tipText);
        if (!tipText) { return; }

        $this.mouseleave($tip.hide).mouseenter(function (e) {
            $tip.show($this, tipText);
        }).triggerHandler('mouseenter');
    });
});

