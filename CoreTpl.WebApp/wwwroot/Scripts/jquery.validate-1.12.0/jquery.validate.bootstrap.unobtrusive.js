
var $jqValid = $.validator;

/* vd-name 不可以做為驗證類型 */
$.extend($jqValid.messages, {
    required: "必須填寫！",
    remote: "請修正此欄位！",
    email: "請輸入有效的電子郵件！",
    url: "請輸入有效的網址！",
    date: "請輸入有效的日期！",
    dateISO: "請輸入有效的日期 (YYYY-MM-DD)！",
    number: "請輸入正確的數值！",
    digits: "只可輸入數字！",
    creditcard: "請輸入正確的信用卡！",
    equalTo: "請重複輸入一次！",
    maxlength: $jqValid.format("最多 {0} 個字！"),
    minlength: $jqValid.format("最少 {0} 個字！"),
    rangelength: $jqValid.format("字數必須在 {0} 到 {1} 之間！"),
    range: $jqValid.format("數字大小請在 {0} 到 {1} 之間！"),
    max: $jqValid.format("數字不能大於 {0}！"),
    min: $jqValid.format("數字不能小於 {0}！")
});




$jqValid.addMethod("notEqualTo", function (value, element, param) {
	if (this.optional(element)) { return true; }
	
	var $target = $(param);
	if ( this.settings.onfocusout ) {
		$target.off(".validate-notEqualTo").on("blur.validate-notEqualTo", function() {
			$(element).valid();
		});
	}
	return value !== $target.val();
}, "請輸入不可以重複！");


$jqValid.addMethod("regex", function (value, element, params) {
     if (this.optional(element)) { return true; }
     return (new RegExp('^' + params + '$')).test(value);
}, "請輸入正確的格式！");


$jqValid.addMethod("method", function(value, element, fnName) {
	if (this.optional(element)) { return true; }
	
	if($.isFunction(window[fnName])){ 
		return window[fnName](value, element);
	}
	return true;
}, "請輸入正確的資料！");	




jQuery(function ($, undefined) {
	var adapters = $.map($jqValid.methods, function(fn, name){ return name; });

	function findFieldName(form, inputEl) {
    	var $inputEl = $(inputEl);
    	var labelFor = $inputEl.attr('id');
    	
    	if($inputEl.is(":radio, :checkbox")){
    		labelFor = $inputEl.attr('name').replace(']','').replace('.','-').replace('[','-');
    	}

    	return $(form).find('label[for='+labelFor+']').text();
	}
	
	
	function findContainer(form, inputEl) {
    	var $inputEl = $(inputEl);
    	var id = $inputEl.attr('name') || $inputEl.attr('vd-name') || $inputEl.attr('id');
    	
    	id = id.replace(']','').replace('.','-').replace('[','-');	
    	return $(form).find("#" + id + "-error");
	}

	
	/* 捲動到錯誤輸入置於畫面正中央*/
	function scrollToInvalid(event, validator) {
        if (!validator.numberOfInvalids()) { return; }
        
        var $el = $(validator.errorList[0].element);
        var $win = $(window);
        $win.scrollTop($el.offset().top - $win.height()/2);
	}

	
	/*單項正確時的處理*/
    function onSuccess($error, inputEl) {
    	var $inputEl = $(inputEl);
    	
        $container = findContainer(this, inputEl);
        $container.empty();
    	
    	var $group = $inputEl.closest('.form-group');
    	if($group.length > 0){
    		$group.removeClass('has-error');
    	}else{
    		$inputEl.parent().removeClass('has-error');
    	}
    }	
    
		
    /*單項錯誤時的處理*/
    function onError($error, $inputEl) {
    	var messages = $error.map(function(){ 
    		return this.innerHTML; 
    	}).toArray().join('<br />');
    	
        $container = findContainer(this, $inputEl);
        $container.html('<span>' + messages + '</span>');
    	
    	var $group = $inputEl.closest('.form-group');
    	if($group.length > 0){
    		$group.addClass('has-error');
    	}else{
    		$inputEl.parent().addClass('has-error');
    	}
    }
    

    /* ""
     * "[2,3]"
     * "[2,3] => msg"
     * "1"
     * "1 => msg"
     * "#password"
     * "#password => msg"
     * "msg"
     * */
    function parseValue(value) {
		var result = { rule: true, message: undefined };
		
		value = $.trim(value);
		if(!value){ return result; } /* "" */
		
		var split = value.split('=>');
		
		if(split.length > 1){
			split[0] = $.trim(split[0]);
			split[1] = $.trim(split[1]);
			
	    	try { 
	    		result.rule = $.parseJSON(split[0]);
			} catch (e) {
				result.rule = split[0];
			}
			result.message = $jqValid.format(split[1]);

		}else if(value[0] == '#' || value[0] == '.'){
			result.rule = value; /* "#password" */
			
		}else{	
	    	try { 
	    		result.rule = $.parseJSON(value);
			} catch (e) {
				result.rule = true;
				result.message = $jqValid.format(value);
			}
		}
		
		return result; 
    }
    
    
	function parseElement(inputEl) {
		var $inputEl = $(inputEl);
		if($inputEl.attr('vd-disable') == 'true'){ return null; }
		
		var param  = { rules: {}, messages: {} };
		
		/*解析 Attribute 的設定*/
		var hasSetting = false;
		$.each(adapters, function(i, name){
			var value = $inputEl.attr('vd-' + name);
			if(value === undefined){ return; }
			
			var result = parseValue(value);
			param.rules[name] = result.rule; 
			param.messages[name] = result.message;
			
			hasSetting = true;
		});		
		
		return (hasSetting ? param : null);
	};
	
	
	
	
	$('form').each(function(){
		$form = $(this);
		
		var settings = {
			rules: {},
			messages: {},
            errorPlacement: $.proxy(onError, this),
            success: $.proxy(onSuccess, this)
		};
		
		var hasSetting = false;
		$form.find(':input, [vd-name]').each(function(){
			var param = parseElement(this);
			if(!param){ return; }
			
			var name = this.name || $(this).attr('vd-name');
			settings.messages[name] = param.messages; 
			settings.rules[name] = param.rules;
			hasSetting = true;
		});
		
		if(!hasSetting){ return; }
		
		
		/* 動態欄位追加驗證 */
		$form.submit(function() {
			$form.trigger('pre-submit-valid');
			
			var hasRemove = {};
	        $form.find(':input, [vd-name]').each(function(){
	        	var $this = $(this);
	        	var name = $this.attr('name') || $this.attr('vd-name');
	        	if(!name){ return; }
	        	
	        	var $input = $form.find(':input[name="'+name+'"]');
	        	if($input.length == 0){ return; }
	        	
	        	if(!hasRemove[name]){
	        		hasRemove[name] = true;
	        		$input.rules("remove");
	        	}
	            var param = parseElement(this);
	            if(!param){ return; }
	            
	            param.rules.messages = param.messages;
	            $input.rules("add", param.rules);
	        });		    
	        
	        return $form.valid();
		});
		
		$form.validate(settings);
		
		$form.bind("invalid-form.validate", scrollToInvalid);		
	});
	
	
	
	window.validatorParseValue = parseValue;
	
	window.testValidatorParseValue = function() {
	
		console.log("", parseValue(""));
		console.log("[2,3]", parseValue("[2,3]"));
		console.log("[2,3] => msg", parseValue("[2,3] => msg"));
		console.log("1", parseValue("1"));
		console.log("1 => msg", parseValue("1 => msg"));
		console.log("#password", parseValue("#password"));
		console.log("#password => msg", parseValue("#password => msg"));
		console.log("msg", parseValue("msg"));
		console.log("[0-9]+", parseValue("[0-9]+"));
		console.log("[0-9]+ => msg", parseValue("[0-9]+ => msg"));
		
	};
	
});

