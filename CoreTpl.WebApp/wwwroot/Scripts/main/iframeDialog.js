
(function (factory) {
	if (typeof exports === 'object') {
		// Node/CommonJS
		factory(require('jquery'));
	} else if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define(['jquery'], factory);
	} else {
		// Browser globals
		factory(jQuery);
	}
} (function ($) {
	"use strict";

	var $body = null;
	var _initSize = 150;
	var _isCurrentSize = false;
	var _dragStartPos = null;
	var _drapMode = null;
	var _dialog = {};

	
	_dialog.open = function(url) {
		_isCurrentSize = false;

		var $modal = $(
			'<div id="DialogWidget" class="modal fade">'+
				'<div class="modal-dialog">'+
					'<div id="DragMask"></div>'+
					'<i class="top-handle"></i>'+
					'<i class="right-handle"></i>'+
					'<i class="left-handle"></i>'+
					'<i class="bottom-handle"></i>'+
					'<i class="right-bottom-handle"></i>'+
					'<i class="left-bottom-handle"></i>'+
					'<span class="modal-dismiss fa-stack" data-dismiss="modal">'+
						'<i class="fa fa-circle fa-inverse fa-stack-2x"></i>'+
						'<i class="fa fa-times-circle fa-stack-2x"></i>'+
					'</span>'+
					'<div class="modal-content">'+
						'<i class="fa fa-spinner fa-spin"></i>'+
						'<iframe id="DialogFrame" name="DialogFrame"></iframe>'+
					'</div>'+
				'</div>'+
			'</div>'
		);
		$modal.attr({
			'aria-labelledby':'TestDialog',
			'aria-hidden':'true',
			'tabindex':'-1',
			'role':'dialog'
		});
		
		$modal.find('iframe').attr({
			'allowTransparency':'true',
			'frameborder':'0',
			'scrolling':'no',
			'tabindex':'-1',
			'src': url
		});
		
		$modal.modal({ backdrop: 'static', keyboard: true });
		$modal.on('hidden.bs.modal', function () {
			$(this).remove();
		});
		
		
		$body = $modal.find('.modal-dialog'); 
		$body.css({
			'position': 'fixed',
			'top': 30,
			'left': ($(window).width() - _initSize) / 2,
			'width': _initSize,
			'height': _initSize,
			'margin': 0
		});
		
		_initEvent();
	};	
	
	
	_dialog.close = function() {
		if(!$body){ return; }
		if(!beforeunload()){ return; }
		
		$('#DialogWidget').modal('hide');
		$body = null;
	};	

	
	_dialog.resize = function(width, height, force) {
		if(!$body){ return; }
		if(_isCurrentSize && !force){ return; }
		
		var winWidth = $(window).width();			
		var winHeight = $(window).height();		
		
		if(width > winWidth && winWidth < 760){
			width = winWidth - 20;
		}else{
			width = Math.min(winWidth - 100, width);
		}
		
		if(height > winHeight && winHeight < 600){
			height = winHeight - 20;
		}else{
			height = Math.min(winHeight - 50, height);
		}
		
		$('#DialogWidget .modal-dialog').css({
			'top': Math.min((winHeight - height), 60) / 2,
			'left': (winWidth - width) / 2,
			'width': width,
			'height': height,
			'margin': 0
		});
	};	
	
	
	function _initEvent() {

		var selector = [
			'.top-handle',
			'.right-handle',
			'.left-handle',
			'.bottom-handle',
			'.right-bottom-handle',
			'.left-bottom-handle'
		].join(',');
		
		$body.on('mousedown', selector, function(event) {
			_dragStartPos = {
				'top': parseInt($body.css('top'), 10) || 0,
				'left': parseInt($body.css('left'), 10) || 0,
				'width': $body.width(),
				'height': $body.height(),
				'clientX': event.clientX,
				'clientY': event.clientY,
				'winWidth':	$(window).width(),			
				'winHeight': $(window).height()				
			}; 
			
			$(document).mouseup(_drapDestroyHandle);
			$(document).mousemove(_drapMoveHandle);
			
			_isCurrentSize = true;
			_drapMode = $(this).attr('class');
			$body.find('#DragMask').attr('class', _drapMode);
			$body.css('transition','none');
		});		
	}
	
	function beforeunload(){
		var $iframe = $body.find('iframe');
		if($iframe.length == 0){ return true; }
		
		var win = $iframe[0].contentWindow;
		if(typeof(win.onbeforeunload) !== 'function'){ return true; } 

		var msg = win.onbeforeunload();
		if(!msg){ return true; } 
		
		return confirm(msg); 
	};
	
	
	function inRange(min, max, value){
		return Math.max(min, Math.min(max, value));
	}

	function _drapDestroyHandle(){
		$(document).unbind('mouseup', _drapDestroyHandle);
		$(document).unbind('mousemove', _drapMoveHandle);				
		$body.css('transition','');
		$body.find('#DragMask').removeAttr('class');
	}
	
	function _drapMoveHandle(event){
		var pos = _dragStartPos;
		var deltaX = event.clientX - pos.clientX;
		var deltaY = event.clientY - pos.clientY;
		var newPos = {};
		
		switch(_drapMode) {
			case 'top-handle':
				var maxTop = pos.winHeight - pos.height;         
				var maxLeft = pos.winWidth - pos.width;           
				newPos.top = inRange(0, maxTop, pos.top + deltaY);
		        newPos.left = inRange(0, maxLeft, pos.left + deltaX);
				break;
			case 'right-handle':
				var maxWidth = pos.winWidth - pos.left;           
	            newPos.width = inRange(_initSize, maxWidth, pos.width + deltaX);
				break;
			case 'left-handle':
				var maxLeft = pos.left + pos.width - _initSize;           
				newPos.left = inRange(0, maxLeft, pos.left + deltaX);
				newPos.width = pos.left - newPos.left + pos.width;
				break;
			case 'bottom-handle':
				var maxHeight = pos.winHeight - pos.top;           
	            newPos.height = inRange(_initSize, maxHeight, pos.height + deltaY);
				break;
			case 'right-bottom-handle':
				var maxHeight = pos.winHeight - pos.top;           
				var maxWidth = pos.winWidth - pos.left;           
	            newPos.height = inRange(_initSize, maxHeight, pos.height + deltaY);
	            newPos.width = inRange(_initSize, maxWidth, pos.width + deltaX);
				break;
			case 'left-bottom-handle':
				var maxLeft = pos.left + pos.width - _initSize;           
				var maxHeight = pos.winHeight - pos.top;           
				newPos.left = inRange(0, maxLeft, pos.left + deltaX);
				newPos.width = pos.left - newPos.left + pos.width;
	            newPos.height = inRange(_initSize, maxHeight, pos.height + deltaY);
				break;
		}
		
		$body.css(newPos);
		if(newPos.height){
			$body.find('.widget').height(newPos.height);				
		}		
	};				


	$.iframeDialog = _dialog;
}));
	
