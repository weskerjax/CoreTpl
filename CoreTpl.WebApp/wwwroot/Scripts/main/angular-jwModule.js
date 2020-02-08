
(function () {
	var jwModule = angular.module('JwModule', []);


	jwModule.filter('showDate', function () {
		return function (date) {
			if (!date) { return date; }
			return moment(date).format('YYYY-MM-DD');
		};
	});


	jwModule.directive('jwUppercase', function () {
		return {
			restrict: 'A',
			require: 'ngModel',
			link: function (scope, element, attrs, modelCtrl) {
				var jwUppercase = function (value) {
					if (!value) { return value; }

					var upper = value.toUpperCase();
					if (upper !== value) {
						modelCtrl.$setViewValue(upper);
						modelCtrl.$render();
					}
					return upper;
				}
				modelCtrl.$parsers.push(jwUppercase);
				jwUppercase(scope[attrs.ngModel]);
			}
		};
	});


	jwModule.directive('jwHasError', ['$parse', function ($parse) {
		return function (scope, element, attrs) {
			var msgField = attrs.jwHasError + '_$error';

			scope.$watch(msgField, function (value) {
				element.toggleClass('has-error', value != undefined && value.length > 0);
			});
		};
	}]);



	jwModule.directive('jwMessage', ['$parse', function ($parse) {
		return function (scope, element, attrs) {
			var msgField = attrs.jwMessage + '_$error';

			scope.$watch(msgField, function (value) {
				if (!value || value.length == 0) {
					element.html('');
				} else {
					element.html('<span>' + value.join(' ') + '</span>');
				}
			});
		};
	}]);


    jwModule.service('jwMessage', ['$rootScope', function ($rootScope) {

        this.add = function (model, field, msg) {
			var msgField = field + '_$error';

			if (!angular.isArray(model[msgField])) { model[msgField] = []; }
			model[msgField].push(msg);
		};

        this.clear = function (model, field) {
			if (field) {
				var msgField = field + '_$error';
				model[msgField] = [];
			} else {
				angular.forEach(model, function (value, field) {
					if (field.endsWith('_$error')) { model[field] = []; }
				});
			}
        };

	}]);



    /* jQuery factory */
    jwModule.factory('$', function () { return jQuery; });


})();
