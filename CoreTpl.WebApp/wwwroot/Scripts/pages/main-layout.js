
/** Navbar Scroll Collapse */
jQuery(function ($) {
	var $win = $(window);
	var $nav = $('#NavbarWrapper');
	var navHeight = $nav.outerHeight() + 10;
	var boundary = navHeight * 1.6;

	$win.on('scroll.NavbarCollapse', function (e) {
		if ($nav.has('.collapse.in, .dropdown.open').length) {
			$nav.css('top', 0);
			return;
		}

		var scrollTop = $win.scrollTop();
		if (scrollTop < boundary) {
			$nav.css('top', 0);
		} else if (scrollTop > boundary) {
			$nav.css('top', -navHeight);
			$(document).triggerHandler('click.bs.dropdown.data-api');
		} else {
			return;
		}
	}).triggerHandler('scroll.NavbarCollapse');
});





/** Sidebar Menu  */
var keepSidebarFilter = 'sidebar_item_filter';

jQuery(function ($) {
	var $body = $('body');

	/* Sidebar Menu 顯示/隱藏 */
	if (localStorage['sidebar_menu_hide'] === 'true') {
		$body.addClass('menu-toggle');
	}

	/* 取消 init */
	setTimeout(function () { $body.removeClass('init'); }, 100);



	$('#MenuToggle').click(function () {
		if ($body.is('.menu-toggle')) {
			$body.removeClass('menu-toggle');
			localStorage['sidebar_menu_hide'] = '';
		} else {
			$body.addClass('menu-toggle');
			localStorage['sidebar_menu_hide'] = 'true';
		}

		setTimeout(function () {
			$(window).trigger('resize');
			$body.trigger('change.menu-toggle');
		}, 600);
	});



	/* Sidebar Menu Item 收合/展開 */
	$('#MenuList .sidebar-dropdown > .sidebar-item').click(function () {
		$(this).parent().toggleClass('open');
	});

});



/** Goto Top */
jQuery(function ($) {
	var $win = $(window);
	var $btn = $('#GotoTop');
	var limit = $win.height() * 2;
	
	$win.on('InitGotoTop scroll', function () {
		$btn.toggleClass('hide', $win.scrollTop() < limit);
	}).triggerHandler('InitGotoTop');
});



