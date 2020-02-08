
(function () {
	function transformInt(num1, num2, padZeno, compute) {
		num1 = '' + (num1 || 0);
		num2 = '' + (num2 || 0);

		var p1 = 0, p2 = 0;
		try { p1 = num1.split(".")[1].length } catch (e) { }
		try { p2 = num2.split(".")[1].length } catch (e) { }

		if (padZeno) {
			while (p1 < p2) { p1++; num1 += '0'; }
			while (p2 < p1) { p2++; num2 += '0'; }
		}

		var int1 = parseInt(num1.replace(".", ""), 10);
		var int2 = parseInt(num2.replace(".", ""), 10);
		return compute(int1, int2, p1, p2);
	}


	/*浮點數相加*/
	window.floatAdd = function(num1, num2) {
		return transformInt(num1, num2, true, function (int1, int2, p1, p2) {
			return (int1 + int2) / Math.pow(10, p1);
		});
	};

	/*浮點數相減*/
	window.floatSub = function(num1, num2) {
		return transformInt(num1, num2, true, function (int1, int2, p1, p2) {
			return (int1 - int2) / Math.pow(10, p1);
		});
	};

	/*浮點數相乘*/
	window.floatMul = function(num1, num2) {
		return transformInt(num1, num2, false, function (int1, int2, p1, p2) {
			return (int1 * int2) / Math.pow(10, p1 + p2);
		});
	};

	/*浮點數相除*/
	window.floatDiv = function (num1, num2) {
		return transformInt(num1, num2, false, function (int1, int2, p1, p2) {
			return (int1 / int2) / Math.pow(10, p1 - p2);
		});
	};

})();

