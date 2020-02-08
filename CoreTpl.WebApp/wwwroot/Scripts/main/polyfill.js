


// From https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/keys
if (!Object.keys) {
	Object.keys = (function () {
		'use strict';
		var hasOwnProperty = Object.prototype.hasOwnProperty,
			hasDontEnumBug = !({ toString: null }).propertyIsEnumerable('toString'),
			dontEnums = [
			  'toString',
			  'toLocaleString',
			  'valueOf',
			  'hasOwnProperty',
			  'isPrototypeOf',
			  'propertyIsEnumerable',
			  'constructor'
			],
			dontEnumsLength = dontEnums.length;

		return function (obj) {
			if (typeof obj !== 'function' && (typeof obj !== 'object' || obj === null)) {
				throw new TypeError('Object.keys called on non-object');
			}

			var result = [], prop, i;

			for (prop in obj) {
				if (hasOwnProperty.call(obj, prop)) {
					result.push(prop);
				}
			}

			if (hasDontEnumBug) {
				for (i = 0; i < dontEnumsLength; i++) {
					if (hasOwnProperty.call(obj, dontEnums[i])) {
						result.push(dontEnums[i]);
					}
				}
			}
			return result;
		};
	}());
}



if (!Object.values) {
	Object.values = (function () {
		'use strict';
		var hasOwnProperty = Object.prototype.hasOwnProperty,
			hasDontEnumBug = !({ toString: null }).propertyIsEnumerable('toString'),
			dontEnums = [
			  'toString',
			  'toLocaleString',
			  'valueOf',
			  'hasOwnProperty',
			  'isPrototypeOf',
			  'propertyIsEnumerable',
			  'constructor'
			],
			dontEnumsLength = dontEnums.length;

		return function (obj) {
			if (typeof obj !== 'function' && (typeof obj !== 'object' || obj === null)) {
				throw new TypeError('Object.values called on non-object');
			}

			var result = [], prop, i;

			for (prop in obj) {
				if (hasOwnProperty.call(obj, prop)) {
					result.push(obj[prop]);
				}
			}

			if (hasDontEnumBug) {
				for (i = 0; i < dontEnumsLength; i++) {
					if (hasOwnProperty.call(obj, dontEnums[i])) {
						result.push(obj[dontEnums[i]]);
					}
				}
			}
			return result;
		};
	}());
}








/* 
https://github.com/uxitten/polyfill/blob/master/string.polyfill.js
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/padStart
 */
if (!String.prototype.padStart) {
	String.prototype.padStart = function padStart(targetLength, padString) {
		targetLength = targetLength >> 0; //truncate if number or convert non-number to 0;
		padString = String((typeof padString !== 'undefined' ? padString : ' '));
		if (this.length > targetLength) {
			return String(this);
		}
		else {
			targetLength = targetLength - this.length;
			if (targetLength > padString.length) {
				padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
			}
			return padString.slice(0, targetLength) + String(this);
		}
	};
}



/* 
https://github.com/uxitten/polyfill/blob/master/string.polyfill.js
https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/padEnd
 */
if (!String.prototype.padEnd) {
	String.prototype.padEnd = function padEnd(targetLength, padString) {
		targetLength = targetLength >> 0; //floor if number or convert non-number to 0;
		padString = String((typeof padString !== 'undefined' ? padString : ' '));
		if (this.length > targetLength) {
			return String(this);
		}
		else {
			targetLength = targetLength - this.length;
			if (targetLength > padString.length) {
				padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
			}
			return String(this) + padString.slice(0, targetLength);
		}
	};
}


if (!String.prototype.includes) {
    String.prototype.includes = function (search, start) {
        'use strict';
        if (typeof start !== 'number') { start = 0; }

        if (start + search.length > this.length) {
            return false;
        } else {
            return this.indexOf(search, start) !== -1;
        }
    };
}




/*
function isBigEnough(element, index, array) { 
  return element >= 10; 
} 

[12, 5, 8, 130, 44].every(isBigEnough);   // false 
[12, 54, 18, 130, 44].every(isBigEnough); // true
 */
if (!Array.prototype.every) {
	Array.prototype.every = function (callbackfn, thisArg) {
		'use strict';
		var T, k;

		if (this == null) { throw new TypeError('this is null or not defined'); }

		// 1. Let O be the result of calling ToObject passing the this 
		//    value as the argument.
		var O = Object(this);

		// 2. Let lenValue be the result of calling the Get internal method
		//    of O with the argument "length".
		// 3. Let len be ToUint32(lenValue).
		var len = O.length >>> 0;

		// 4. If IsCallable(callbackfn) is false, throw a TypeError exception.
		if (typeof callbackfn !== 'function') { throw new TypeError(); }

		// 5. If thisArg was supplied, let T be thisArg; else let T be undefined.
		if (arguments.length > 1) { T = thisArg; }

		// 6. Let k be 0.
		k = 0;

		// 7. Repeat, while k < len
		while (k < len) {
			var kValue;

			// a. Let Pk be ToString(k).
			//   This is implicit for LHS operands of the in operator
			// b. Let kPresent be the result of calling the HasProperty internal 
			//    method of O with argument Pk.
			//   This step can be combined with c
			// c. If kPresent is true, then
			if (k in O) {

				// i. Let kValue be the result of calling the Get internal method
				//    of O with argument Pk.
				kValue = O[k];

				// ii. Let testResult be the result of calling the Call internal method
				//     of callbackfn with T as the this value and argument list 
				//     containing kValue, k, and O.
				var testResult = callbackfn.call(T, kValue, k, O);

				// iii. If ToBoolean(testResult) is false, return false.
				if (!testResult) {
					return false;
				}
			}
			k++;
		}
		return true;
	};
}




/*
var words = ["spray", "limit", "elite", "exuberant", "destruction", "present"];

var longWords = words.filter(function(element, index, array){
  return element.length > 6;
})
 */
if (!Array.prototype.filter) {
	Array.prototype.filter = function (fun/*, thisArg*/) {
		'use strict';

		if (this === void 0 || this === null) { throw new TypeError(); }

		var t = Object(this);
		var len = t.length >>> 0;
		if (typeof fun !== 'function') { throw new TypeError(); }

		var res = [];
		var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
		for (var i = 0; i < len; i++) {
			if (i in t) {
				var val = t[i];

				// NOTE: Technically this should Object.defineProperty at
				//       the next index, as push can be affected by
				//       properties on Object.prototype and Array.prototype.
				//       But that method's new, and collisions should be
				//       rare, so use the more-compatible alternative.
				if (fun.call(thisArg, val, i, t)) {
					res.push(val);
				}
			}
		}

		return res;
	};
}



/*
function isBigEnough(element, index, array) {
  return element >= 15;
}

[12, 5, 8, 130, 44].find(isBigEnough); // 130
 */
if (!Array.prototype.find) {
	Array.prototype.find = function (predicate) {
		if (this === null) { throw new TypeError('Array.prototype.find called on null or undefined'); }
		if (typeof predicate !== 'function') { throw new TypeError('predicate must be a function'); }

		var list = Object(this);
		var length = list.length >>> 0;
		var thisArg = arguments[1];
		var value;

		for (var i = 0; i < length; i++) {
			value = list[i];
			if (predicate.call(thisArg, value, i, list)) {
				return value;
			}
		}
		return undefined;
	};
}




// https://tc39.github.io/ecma262/#sec-array.prototype.findIndex
/*
function isBigEnough(element, index, array) {
  return element >= 15;
}

[12, 5, 8, 130, 44].findIndex(isBigEnough); //3
 */
if (!Array.prototype.findIndex) {
	Array.prototype.findIndex = function (predicate) {
		// 1. Let O be ? ToObject(this value).
		if (this == null) { throw new TypeError('"this" is null or not defined'); }

		var o = Object(this);

		// 2. Let len be ? ToLength(? Get(O, "length")).
		var len = o.length >>> 0;

		// 3. If IsCallable(predicate) is false, throw a TypeError exception.
		if (typeof predicate !== 'function') { throw new TypeError('predicate must be a function'); }

		// 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
		var thisArg = arguments[1];

		// 5. Let k be 0.
		var k = 0;

		// 6. Repeat, while k < len
		while (k < len) {
			// a. Let Pk be ! ToString(k).
			// b. Let kValue be ? Get(O, Pk).
			// c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
			// d. If testResult is true, return k.
			var kValue = o[k];
			if (predicate.call(thisArg, kValue, k, o)) {
				return k;
			}
			// e. Increase k by 1.
			k++;
		}

		// 7. Return -1.
		return -1;
	};
}




// Production steps of ECMA-262, Edition 5, 15.4.4.18
// Reference: http://es5.github.io/#x15.4.4.18
/*
var a = ['a', 'b', 'c'];

a.forEach(function(element, index, array) {
	console.log(element);
});
 */
if (!Array.prototype.forEach) {

	Array.prototype.forEach = function(callback, thisArg) {
		var T, k;

		if (this == null) { throw new TypeError(' this is null or not defined'); }

		// 1. Let O be the result of calling toObject() passing the
		// |this| value as the argument.
		var O = Object(this);

		// 2. Let lenValue be the result of calling the Get() internal
		// method of O with the argument "length".
		// 3. Let len be toUint32(lenValue).
		var len = O.length >>> 0;

		// 4. If isCallable(callback) is false, throw a TypeError exception. 
		// See: http://es5.github.com/#x9.11
		if (typeof callback !== "function") { throw new TypeError(callback + ' is not a function'); }

		// 5. If thisArg was supplied, let T be thisArg; else let
		// T be undefined.
		if (arguments.length > 1) { T = thisArg; }

		// 6. Let k be 0
		k = 0;

		// 7. Repeat, while k < len
		while (k < len) {

			var kValue;

			// a. Let Pk be ToString(k).
			//    This is implicit for LHS operands of the in operator
			// b. Let kPresent be the result of calling the HasProperty
			//    internal method of O with argument Pk.
			//    This step can be combined with c
			// c. If kPresent is true, then
			if (k in O) {

				// i. Let kValue be the result of calling the Get internal
				// method of O with argument Pk.
				kValue = O[k];

				// ii. Call the Call internal method of callback with T as
				// the this value and argument list containing kValue, k, and O.
				callback.call(T, kValue, k, O);
			}
			// d. Increase k by 1.
			k++;
		}
		// 8. return undefined
	};
}





// https://tc39.github.io/ecma262/#sec-array.prototype.includes
/*
var a = [1, 2, 3];
a.includes(2); // true 
a.includes(4); // false

arr.includes(searchElement)
arr.includes(searchElement, fromIndex)
 */
if (!Array.prototype.includes) {
	Array.prototype.includes = function(searchElement, fromIndex) {

		// 1. Let O be ? ToObject(this value).
		if (this == null) { throw new TypeError('"this" is null or not defined'); }

		var o = Object(this);

		// 2. Let len be ? ToLength(? Get(O, "length")).
		var len = o.length >>> 0;

		// 3. If len is 0, return false.
		if (len === 0) {
			return false;
		}

		// 4. Let n be ? ToInteger(fromIndex).
		//    (If fromIndex is undefined, this step produces the value 0.)
		var n = fromIndex | 0;

		// 5. If n ≥ 0, then
		//  a. Let k be n.
		// 6. Else n < 0,
		//  a. Let k be len + n.
		//  b. If k < 0, let k be 0.
		var k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);

		function sameValueZero(x, y) {
			return x === y || (typeof x === 'number' && typeof y === 'number' && isNaN(x) && isNaN(y));
		}

		// 7. Repeat, while k < len
		while (k < len) {
			// a. Let elementK be the result of ? Get(O, ! ToString(k)).
			// b. If SameValueZero(searchElement, elementK) is true, return true.
			// c. Increase k by 1. 
			if (sameValueZero(o[k], searchElement)) {
				return true;
			}
			k++;
		}

		// 8. Return false
		return false;
	};
}




// Production steps of ECMA-262, Edition 5, 15.4.4.19
// Reference: http://es5.github.io/#x15.4.4.19
/*
var numbers = [1, 5, 10, 15];
var roots = numbers.map(function(element, index, array) {
   return element * 2;
});
// [2, 10, 20, 30]
 */
if (!Array.prototype.map) {

	Array.prototype.map = function(callback/*, thisArg*/) {

		var T, A, k;
		if (this == null) { throw new TypeError('this is null or not defined'); }

		// 1. Let O be the result of calling ToObject passing the |this| 
		//    value as the argument.
		var O = Object(this);

		// 2. Let lenValue be the result of calling the Get internal 
		//    method of O with the argument "length".
		// 3. Let len be ToUint32(lenValue).
		var len = O.length >>> 0;

		// 4. If IsCallable(callback) is false, throw a TypeError exception.
		// See: http://es5.github.com/#x9.11
		if (typeof callback !== 'function') { throw new TypeError(callback + ' is not a function'); }

		// 5. If thisArg was supplied, let T be thisArg; else let T be undefined.
		if (arguments.length > 1) { T = arguments[1]; }

		// 6. Let A be a new array created as if by the expression new Array(len) 
		//    where Array is the standard built-in constructor with that name and 
		//    len is the value of len.
		A = new Array(len);

		// 7. Let k be 0
		k = 0;

		// 8. Repeat, while k < len
		while (k < len) {

			var kValue, mappedValue;

			// a. Let Pk be ToString(k).
			//   This is implicit for LHS operands of the in operator
			// b. Let kPresent be the result of calling the HasProperty internal 
			//    method of O with argument Pk.
			//   This step can be combined with c
			// c. If kPresent is true, then
			if (k in O) {

				// i. Let kValue be the result of calling the Get internal 
				//    method of O with argument Pk.
				kValue = O[k];

				// ii. Let mappedValue be the result of calling the Call internal 
				//     method of callback with T as the this value and argument 
				//     list containing kValue, k, and O.
				mappedValue = callback.call(T, kValue, k, O);

				// iii. Call the DefineOwnProperty internal method of A with arguments
				// Pk, Property Descriptor
				// { Value: mappedValue,
				//   Writable: true,
				//   Enumerable: true,
				//   Configurable: true },
				// and false.

				// In browsers that support Object.defineProperty, use the following:
				// Object.defineProperty(A, k, {
				//   value: mappedValue,
				//   writable: true,
				//   enumerable: true,
				//   configurable: true
				// });

				// For best browser support, use the following:
				A[k] = mappedValue;
			}
			// d. Increase k by 1.
			k++;
		}

		// 9. return A
		return A;
	};
}




// Production steps of ECMA-262, Edition 5, 15.4.4.21
// Reference: http://es5.github.io/#x15.4.4.21
// https://tc39.github.io/ecma262/#sec-array.prototype.reduce
/*
var sum = [0, 1, 2, 3].reduce(function(acc, val) {
  return acc + val;
}, 0);
// sum is 6

arr.reduce(function(accumulator, currentValue, currentIndex, array) { }, [initialValue])

 */
if (!Array.prototype.reduce) {
	Array.prototype.reduce = function (callback /*, initialValue*/) {
		if (this === null) { throw new TypeError('Array.prototype.reduce called on null or undefined'); }
		if (typeof callback !== 'function') { throw new TypeError(callback + ' is not a function'); }

		// 1. Let O be ? ToObject(this value).
		var o = Object(this);

		// 2. Let len be ? ToLength(? Get(O, "length")).
		var len = o.length >>> 0;

		// Steps 3, 4, 5, 6, 7      
		var k = 0;
		var value;

		if (arguments.length >= 2) {
			value = arguments[1];
		} else {
			while (k < len && !(k in o)) { k++; }

			// 3. If len is 0 and initialValue is not present,
			//    throw a TypeError exception.
			if (k >= len) { throw new TypeError('Reduce of empty array with no initial value'); }
			value = o[k++];
		}

		// 8. Repeat, while k < len
		while (k < len) {
			// a. Let Pk be ! ToString(k).
			// b. Let kPresent be ? HasProperty(O, Pk).
			// c. If kPresent is true, then
			//    i.  Let kValue be ? Get(O, Pk).
			//    ii. Let accumulator be ? Call(
			//          callbackfn, undefined,
			//          « accumulator, kValue, k, O »).
			if (k in o) { value = callback(value, o[k], k, o); }

			// d. Increase k by 1.      
			k++;
		}

		// 9. Return accumulator.
		return value;
	};
}




// Production steps of ECMA-262, Edition 5, 15.4.4.22
// Reference: http://es5.github.io/#x15.4.4.22
/*
var flattened = [[0, 1], [2, 3], [4, 5]].reduceRight(function(a, b) {
	return a.concat(b);
}, []);

// flattened is [4, 5, 2, 3, 0, 1]
 */
if ('function' !== typeof Array.prototype.reduceRight) {
	Array.prototype.reduceRight = function (callback /*, initialValue*/) {
		'use strict';
		if (null === this || 'undefined' === typeof this) { throw new TypeError('Array.prototype.reduce called on null or undefined'); }
		if ('function' !== typeof callback) { throw new TypeError(callback + ' is not a function'); }

		var t = Object(this), len = t.length >>> 0, k = len - 1, value;
		if (arguments.length >= 2) {
			value = arguments[1];
		} else {
			while (k >= 0 && !(k in t)) {
				k--;
			}
			if (k < 0) { throw new TypeError('Reduce of empty array with no initial value'); }
			value = t[k--];
		}
		for (; k >= 0; k--) {
			if (k in t) { value = callback(value, t[k], k, t); }
		}
		return value;
	};
}




// Production steps of ECMA-262, Edition 5, 15.4.4.17
// Reference: http://es5.github.io/#x15.4.4.17
/*
function isBiggerThan10(element, index, array) {
  return element > 10;
}

[2, 5, 8, 1, 4].some(isBiggerThan10);  // false
[12, 5, 8, 1, 4].some(isBiggerThan10); // true
 */
if (!Array.prototype.some) {
	Array.prototype.some = function(fun/*, thisArg*/) {
		'use strict';

		if (this == null) { throw new TypeError('Array.prototype.some called on null or undefined'); }
		if (typeof fun !== 'function') { throw new TypeError(); }

		var t = Object(this);
		var len = t.length >>> 0;

		var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
		for (var i = 0; i < len; i++) {
			if (i in t && fun.call(thisArg, t[i], i, t)) {
				return true;
			}
		}

		return false;
	};
}




/*##################################################################*/


/*
 */

if (!Array.prototype.unique) {
	Array.prototype.unique = function (fun/*, thisArg*/) {
		'use strict';

		if (this == null) { throw new TypeError('Array.prototype.some called on null or undefined'); }
		
		function sameValueZero(x, y) {
			return x === y || (typeof x === 'number' && typeof y === 'number' && isNaN(x) && isNaN(y));
		}

		var result = [];

		if (typeof fun === 'function') {
			this.forEach(function (element, index, array) {
				var value = fun(element);
				var isExists = result.some(function (element) { return sameValueZero(fun(element), value); });
				if (isExists) { return; }

				result.push(element);
			});
		} else {
			this.forEach(function (element, index, array) {
				if (result.includes(element)) { return; }
				result.push(element);
			});
		}

		return result;
	}
}




if (!Array.prototype.remove) {
	Array.prototype.remove = function (item) {
		var i = this.indexOf(item);
		if (i > -1) { this.splice(i, 1); }
		return this;
	}
}




