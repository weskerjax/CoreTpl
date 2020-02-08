
; (function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD is used - Register as an anonymous module.
		define(['jquery', 'moment'], factory);
	} else {
		// AMD is not used - Attempt to fetch dependencies from scope.
		if (!jQuery) {
			throw 'bootstrap-datetimepicker requires jQuery to be loaded first';
		} else if (!moment) {
			throw 'bootstrap-datetimepicker requires moment.js to be loaded first';
		} else {
			factory(jQuery, moment);
		}
	}
}

	(function ($, moment) {
		if (typeof moment === 'undefined') {
			alert("momentjs is requried");
			throw new Error('momentjs is required');
		}



		var dpGlobal = {
			modes: [{
				clsName: 'days',
				navFnc: 'month',
				navStep: 1
			}, {
				clsName: 'months',
				navFnc: 'year',
				navStep: 1
			}, {
				clsName: 'years',
				navFnc: 'year',
				navStep: 10
			}],
			headTemplate:
				'<thead><tr>' +
				'<th class="prev">&lsaquo;</th><th colspan="5" class="switch"></th><th class="next">&rsaquo;</th>' +
				'</tr></thead>',
			contTemplate: '<tbody><tr><td colspan="7"></td></tr></tbody>'
		};

		var tpGlobal = {
			hourTemplate: '<span data-action="showHours"   data-time-component="hours"   class="timepicker-hour"></span>',
			minuteTemplate: '<span data-action="showMinutes" data-time-component="minutes" class="timepicker-minute"></span>',
			secondTemplate: '<span data-action="showSeconds"  data-time-component="seconds" class="timepicker-second"></span>'
		};

		dpGlobal.template =
			'<div class="datepicker-days">' +
			'<table class="table-condensed">' + dpGlobal.headTemplate + '<tbody></tbody></table>' +
			'</div>' +
			'<div class="datepicker-months">' +
			'<table class="table-condensed">' + dpGlobal.headTemplate + dpGlobal.contTemplate + '</table>' +
			'</div>' +
			'<div class="datepicker-years">' +
			'<table class="table-condensed">' + dpGlobal.headTemplate + dpGlobal.contTemplate + '</table>' +
			'</div>';

		tpGlobal.getTemplate = function (picker) {
			var opt = picker.options;
			var icons = opt.icons;
			return (
				'<div class="timepicker-picker">' +
				'<table class="table-condensed">' +
				'<tr>' +
				'<td><a href="#" class="btn" data-action="incrementHours"><span class="' + icons.up + '"></span></a></td>' +
				'<td class="separator"></td>' +
				'<td>' + (opt.useMinutes ? '<a href="#" class="btn" data-action="incrementMinutes"><span class="' + icons.up + '"></span></a>' : '') + '</td>' +
				(opt.useSeconds ?
					'<td class="separator"></td><td><a href="#" class="btn" data-action="incrementSeconds"><span class="' + icons.up + '"></span></a></td>' : '') +
				(picker.use24hours ? '' : '<td class="separator"></td>') +
				'</tr>' +
				'<tr>' +
				'<td>' + tpGlobal.hourTemplate + '</td> ' +
				'<td class="separator">:</td>' +
				'<td>' + (opt.useMinutes ? tpGlobal.minuteTemplate : '<span class="timepicker-minute">00</span>') + '</td> ' +
				(opt.useSeconds ? '<td class="separator">:</td><td>' + tpGlobal.secondTemplate + '</td>' : '') +
				(picker.use24hours ? '' : '<td class="separator"></td>' +
					'<td><button type="button" class="btn btn-primary" data-action="togglePeriod"></button></td>') +
				'</tr>' +
				'<tr>' +
				'<td><a href="#" class="btn" data-action="decrementHours"><span class="' + icons.down + '"></span></a></td>' +
				'<td class="separator"></td>' +
				'<td>' + (opt.useMinutes ? '<a href="#" class="btn" data-action="decrementMinutes"><span class="' + icons.down + '"></span></a>' : '') + '</td>' +
				(opt.useSeconds ?
					'<td class="separator"></td><td><a href="#" class="btn" data-action="decrementSeconds"><span class="' + icons.down + '"></span></a></td>' : '') +
				(picker.use24hours ? '' : '<td class="separator"></td>') +
				'</tr>' +
				'</table>' +
				'</div>' +
				'<div class="timepicker-hours" data-action="selectHour">' +
				'<table class="table-condensed"></table>' +
				'</div>' +
				'<div class="timepicker-minutes" data-action="selectMinute">' +
				'<table class="table-condensed"></table>' +
				'</div>' +
				(opt.useSeconds ? '<div class="timepicker-seconds" data-action="selectSecond"><table class="table-condensed"></table></div>' : '')
			);
		};

		function getTemplate(picker) {
			var opt = picker.options;
			if (opt.pickDate && opt.pickTime) {
				var ret = '';
				ret = '<div class="bootstrap-datetimepicker-widget' + (opt.sideBySide ? ' timepicker-sbs' : '') + ' dropdown-menu" style="z-index:9999 !important;">';
				if (opt.sideBySide) {
					ret += '<div class="row">' +
						'<div class="col-sm-6 datepicker">' + dpGlobal.template + '</div>' +
						'<div class="col-sm-6 timepicker">' + tpGlobal.getTemplate(picker) + '</div>' +
						'</div>';
				} else {
					ret += '<ul class="list-unstyled">' +
						'<li' + (opt.collapse ? ' class="collapse in"' : '') + '>' +
						'<div class="datepicker">' + dpGlobal.template + '</div>' +
						'</li>' +
						'<li class="picker-switch accordion-toggle"><a class="btn" style="width:100%"><span class="' + opt.icons.time + '"></span></a></li>' +
						'<li' + (opt.collapse ? ' class="collapse"' : '') + '>' +
						'<div class="timepicker">' + tpGlobal.getTemplate(picker) + '</div>' +
						'</li>' +
						'</ul>';
				}
				ret += '</div>';
				return ret;
			} else if (opt.pickTime) {
				return (
					'<div class="bootstrap-datetimepicker-widget dropdown-menu">' +
					'<div class="timepicker">' + tpGlobal.getTemplate(picker) + '</div>' +
					'</div>'
				);
			} else {
				return (
					'<div class="bootstrap-datetimepicker-widget dropdown-menu">' +
					'<div class="datepicker">' + dpGlobal.template + '</div>' +
					'</div>'
				);
			}
		}





		var actions = {
			incrementHours: function () {
				checkDate(this, "add", "hours", 1);
			},
			incrementMinutes: function () {
				checkDate(this, "add", "minutes", this.options.minuteStepping);
			},
			incrementSeconds: function () {
				checkDate(this, "add", "seconds", 1);
			},
			decrementHours: function () {
				checkDate(this, "subtract", "hours", 1);
			},
			decrementMinutes: function () {
				checkDate(this, "subtract", "minutes", this.options.minuteStepping);
			},
			decrementSeconds: function () {
				checkDate(this, "subtract", "seconds", 1);
			},
			showPicker: function () {
				this.widget.find('.timepicker > div:not(.timepicker-picker)').hide();
				this.widget.find('.timepicker .timepicker-picker').show();
			},
			showHours: function () {
				this.widget.find('.timepicker .timepicker-picker').hide();
				this.widget.find('.timepicker .timepicker-hours').show();
			},
			showMinutes: function () {
				this.widget.find('.timepicker .timepicker-picker').hide();
				this.widget.find('.timepicker .timepicker-minutes').show();
			},
			showSeconds: function () {
				this.widget.find('.timepicker .timepicker-picker').hide();
				this.widget.find('.timepicker .timepicker-seconds').show();
			},
			selectHour: function (e) {
				var period = this.widget.find('.timepicker [data-action=togglePeriod]').text();
				var hour = parseInt($(e.target).text(), 10);
				if (period == "PM") hour += 12
				this.date.hours(hour);
				actions.showPicker.call(this);
			},
			selectMinute: function (e) {
				this.date.minutes(parseInt($(e.target).text(), 10));
				actions.showPicker.call(this);
			},
			selectSecond: function (e) {
				this.date.seconds(parseInt($(e.target).text(), 10));
				actions.showPicker.call(this);
			}
		};



		function place(picker) {
			var position = 'absolute',
				offset = picker.component ? picker.component.offset() : picker.element.offset(), $window = $(window);
			picker.width = picker.component ? picker.component.outerWidth() : picker.element.outerWidth();
			offset.top = offset.top + picker.element.outerHeight();

			var placePosition = 'bottom';
			if (picker.options.direction === 'up') {
				placePosition = 'top';
			} else if (picker.options.direction === 'auto') {
				if (offset.top + picker.widget.height() > $window.height() + $window.scrollTop() && picker.widget.height() + picker.element.outerHeight() < offset.top) {
					placePosition = 'top';
				}
			}

			if (placePosition === 'top') {
				offset.top -= picker.widget.height() + picker.element.outerHeight() + 15;
				picker.widget.addClass('top').removeClass('bottom');
			} else {
				offset.top += 1;
				picker.widget.addClass('bottom').removeClass('top');
			}

			if (picker.options.width) {
				picker.widget.width(picker.options.width);
			}

			if (picker.options.orientation === 'left') {
				picker.widget.addClass('left-oriented');
				offset.left = offset.left - picker.widget.width() + 20;
			}

			var isInFixed = picker.element.parents().is(function () {
				return ($(this).css('position') == 'fixed');
			});
			if (isInFixed) {
				position = 'fixed';
				offset.top -= $window.scrollTop();
				offset.left -= $window.scrollLeft();
			}

			if ($window.width() < offset.left + picker.widget.outerWidth()) {
				offset.right = $window.width() - offset.left - picker.width;
				offset.left = 'auto';
				picker.widget.addClass('pull-right');
			} else {
				offset.right = 'auto';
				picker.widget.removeClass('pull-right');
			}

			picker.widget.css({
				position: position,
				top: offset.top,
				left: offset.left,
				right: offset.right
			});
		}

		function notifyChange(picker, oldDate, eventType) {
			if (moment(picker.date).isSame(moment(oldDate))) { return; }

			picker.element.trigger({
				type: 'dp.change',
				date: moment(picker.date),
				oldDate: moment(oldDate)
			});

			if (eventType !== 'change') {
				picker.element.trigger({ type: 'change', fromPicker: true });
			}
		}

		function update(picker, newDate) {
			var trDate = picker.options.transferInDate;
			var dateStr = newDate;
			if (!dateStr) {
				dateStr = trDate(picker.inputEl.val());
				if (dateStr) {
					picker.date = moment(dateStr, picker.format, picker.options.useStrict);
				}
				if (!picker.date || !picker.date.isValid()) {
					picker.date = moment();
				}
			}
			picker.viewDate = moment(picker.date).startOf("month");
			fillDate(picker);
			fillTime(picker);
		}

		function fillDow(picker) {
			var html = $('<tr>');
			var weekdaysMin = moment.weekdaysMin()
			var dow = moment()._lang._week.dow;
			for (var i = 0; i < 7; i++) {
				var w = (i + dow) % 7;
				html.append('<th class="dow">' + weekdaysMin[w] + '</th>');
			}
			picker.widget.find('.datepicker-days thead').append(html);
		}

		function fillMonths(picker) {
			var html = '', i = 0, monthsShort = moment.monthsShort();
			while (i < 12) {
				html += '<span class="month">' + monthsShort[i++] + '</span>';
			}
			picker.widget.find('.datepicker-months td').append(html);
		}



		function fillDate(picker) {
			if (!picker.options.pickDate) { return; }
			var trYear = picker.options.transferOutYear;
			var year = picker.viewDate.year(),
				month = picker.viewDate.month(),
				startYear = picker.options.minDate.year(),
				startMonth = picker.options.minDate.month(),
				endYear = picker.options.maxDate.year(),
				endMonth = picker.options.maxDate.month(),
				html = [], row, i, months = moment.months();

			picker.widget.find('.datepicker-days .disabled').removeClass('disabled');
			picker.widget.find('.datepicker-months .disabled').removeClass('disabled');
			picker.widget.find('.datepicker-years .disabled').removeClass('disabled');

			picker.widget.find('.datepicker-days th:eq(1)').text(trYear(year) + ' ' + months[month]);

			var prevMonth = moment(picker.viewDate).subtract("months", 1);
			var days = prevMonth.daysInMonth();
			prevMonth.date(days).startOf('week');
			if ((year == startYear && month <= startMonth) || year < startYear) {
				picker.widget.find('.datepicker-days th:eq(0)').addClass('disabled');
			}
			if ((year == endYear && month >= endMonth) || year > endYear) {
				picker.widget.find('.datepicker-days th:eq(2)').addClass('disabled');
			}

			var nextMonth = moment(prevMonth).add(42, "d");
			while (prevMonth.isBefore(nextMonth)) {
				if (prevMonth.weekday() === moment().startOf('week').weekday()) {
					row = $('<tr>');
					html.push(row);
				}
				var clsName = '';
				if (prevMonth.year() < year || (prevMonth.year() == year && prevMonth.month() < month)) {
					clsName += ' old';
				} else if (prevMonth.year() > year || (prevMonth.year() == year && prevMonth.month() > month)) {
					clsName += ' new';
				}
				if (prevMonth.isSame(moment({ y: picker.date.year(), M: picker.date.month(), d: picker.date.date() }))) {
					clsName += ' active';
				}
				if (isInDisableDates(picker, prevMonth) || !isInEnableDates(picker, prevMonth)) {
					clsName += ' disabled';
				}
				if (picker.options.showToday === true) {
					if (prevMonth.isSame(moment(), 'day')) {
						clsName += ' today';
					}
				}
				if (picker.options.daysOfWeekDisabled) {
					for (i in picker.options.daysOfWeekDisabled) {
						if (prevMonth.day() == picker.options.daysOfWeekDisabled[i]) {
							clsName += ' disabled';
							break;
						}
					}
				}
				row.append('<td class="day' + clsName + '">' + prevMonth.date() + '</td>');

				var currentDate = prevMonth.date();
				prevMonth.add(1, "d");

				if (currentDate == prevMonth.date()) {
					prevMonth.add(1, "d");
				}
			}

			var currentYear = picker.date.year();
			picker.widget.find('.datepicker-days tbody').empty().append(html);
			months = picker.widget.find('.datepicker-months').find('th:eq(1)').text(trYear(year)).end().find('span').removeClass('active');
			if (currentYear === year) {
				months.eq(picker.date.month()).addClass('active');
			}
			if (currentYear - 1 < startYear) {
				picker.widget.find('.datepicker-months th:eq(0)').addClass('disabled');
			}
			if (currentYear + 1 > endYear) {
				picker.widget.find('.datepicker-months th:eq(2)').addClass('disabled');
			}
			for (i = 0; i < 12; i++) {
				if ((year == startYear && startMonth > i) || (year < startYear)) {
					$(months[i]).addClass('disabled');
				} else if ((year == endYear && endMonth < i) || (year > endYear)) {
					$(months[i]).addClass('disabled');
				}
			}

			html = '';
			year = parseInt(year / 10, 10) * 10;
			var yearCont = picker.widget.find('.datepicker-years').find('th:eq(1)').text(trYear(year) + '-' + trYear(year + 9)).end().find('td');
			picker.widget.find('.datepicker-years').find('th').removeClass('disabled');
			if (startYear > year) {
				picker.widget.find('.datepicker-years').find('th:eq(0)').addClass('disabled');
			}
			if (endYear < year + 9) {
				picker.widget.find('.datepicker-years').find('th:eq(2)').addClass('disabled');
			}
			year -= 1;
			for (i = -1; i < 11; i++) {
				html += '<span class="year' + (i === -1 || i === 10 ? ' old' : '') + (currentYear === year ? ' active' : '') + ((year < startYear || year > endYear) ? ' disabled' : '') + '">' + trYear(year) + '</span>';
				year += 1;
			}
			yearCont.html(html);
		}

		function fillHours(picker) {
			var table = picker.widget.find('.timepicker .timepicker-hours table'), html = '', current, i, j;
			table.parent().hide();
			if (picker.use24hours) {
				current = 0;
				for (i = 0; i < 4; i += 1) {
					html += '<tr>';
					for (j = 0; j < 6; j += 1) {
						html += '<td class="hour">' + padLeft(current.toString()) + '</td>';
						current++;
					}
					html += '</tr>';
				}
			}
			else {
				current = 1;
				for (i = 0; i < 3; i += 1) {
					html += '<tr>';
					for (j = 0; j < 4; j += 1) {
						html += '<td class="hour">' + padLeft(current.toString()) + '</td>';
						current++;
					}
					html += '</tr>';
				}
			}
			table.html(html);
		}

		function fillMinutes(picker) {
			var table = picker.widget.find('.timepicker .timepicker-minutes table'), html = '', current = 0, i, j, step = picker.options.minuteStepping;
			table.parent().hide();
			if (step == 1) step = 5;
			for (i = 0; i < Math.ceil(60 / step / 4); i++) {
				html += '<tr>';
				for (j = 0; j < 4; j += 1) {
					if (current < 60) {
						html += '<td class="minute">' + padLeft(current.toString()) + '</td>';
						current += step;
					} else {
						html += '<td></td>';
					}
				}
				html += '</tr>';
			}
			table.html(html);
		}

		function fillSeconds(picker) {
			var table = picker.widget.find('.timepicker .timepicker-seconds table'), html = '', current = 0, i, j;
			table.parent().hide();
			for (i = 0; i < 3; i++) {
				html += '<tr>';
				for (j = 0; j < 4; j += 1) {
					html += '<td class="second">' + padLeft(current.toString()) + '</td>';
					current += 5;
				}
				html += '</tr>';
			}
			table.html(html);
		}


		function fillTime(picker) {
			if (!picker.date) { return; }

			var timeComponents = picker.widget.find('.timepicker span[data-time-component]'),
				hour = picker.date.hours(),
				period = 'AM';
			if (!picker.use24hours) {
				if (hour >= 12) { period = 'PM'; }
				if (hour === 0) { hour = 12; }
				else if (hour != 12) { hour = hour % 12; }
				picker.widget.find('.timepicker [data-action=togglePeriod]').text(period);
			}
			timeComponents.filter('[data-time-component=hours]').text(padLeft(hour));
			timeComponents.filter('[data-time-component=minutes]').text(padLeft(picker.date.minutes()));
			timeComponents.filter('[data-time-component=seconds]').text(padLeft(picker.date.second()));
		}


		function clickMenu(picker, target) {
			picker.unset = false;

			var trYear = picker.options.transferOutYear;
			var oldDate = moment(picker.date);
			if (target.is('.month')) {
				var month = target.parent().find('span').index(target);
				picker.viewDate.month(month);
			} else {
				var year = parseInt(target.text(), 10) || 0;
				year -= trYear(0);
				picker.viewDate.year(year);
			}
			if (picker.viewMode === picker.minViewMode) {
				picker.date = moment({
					y: picker.viewDate.year(),
					M: picker.viewDate.month(),
					d: picker.viewDate.date(),
					h: picker.date.hours(),
					m: picker.date.minutes(),
					s: picker.date.seconds()
				});
				notifyChange(picker, oldDate, e.type);
				set(picker);
			}
			showMode(picker, -1);
			fillDate(picker);
		}

		function clickDay(picker, target) {
			picker.unset = false;
			var oldDate = moment(picker.date);
			var day = parseInt(target.text(), 10) || 1;
			var month = picker.viewDate.month();
			var year = picker.viewDate.year();
			if (target.is('.old')) {
				if (month === 0) {
					month = 11;
					year -= 1;
				} else {
					month -= 1;
				}
			} else if (target.is('.new')) {
				if (month == 11) {
					month = 0;
					year += 1;
				} else {
					month += 1;
				}
			}
			picker.date = moment({
				y: year,
				M: month,
				d: day,
				h: picker.date.hours(),
				m: picker.date.minutes(),
				s: picker.date.seconds()
			});

			picker.viewDate = moment({
				y: year, M: month, d: Math.min(28, day)
			});
			fillDate(picker);
			set(picker);
			notifyChange(picker, oldDate, 'click');
		}


		function change(picker) {
			var trDate = picker.options.transferInDate;
			var oldDate = moment(picker.date);
			var newDate = moment(trDate(picker.inputEl.val()), picker.format, picker.options.useStrict);

			if (newDate.isValid() && !isInDisableDates(picker, newDate) && isInEnableDates(picker, newDate)) {
				update(picker);
				picker.setValue(newDate);
				notifyChange(picker, oldDate, 'change');
				set(picker);
			}
			else {
				picker.viewDate = oldDate;
				notifyChange(picker, oldDate, 'change');
				picker.element.trigger({ type: 'dp.error', date: moment(newDate) });
				picker.unset = true;
			}
		}

		function showMode(picker, dir) {
			if (dir) {
				picker.viewMode = Math.max(picker.minViewMode, Math.min(2, picker.viewMode + dir));
			}
			var filterClass = '.datepicker-' + dpGlobal.modes[picker.viewMode].clsName;
			picker.widget.find('.datepicker > div').hide().filter(filterClass).show();
		}



		function attachDatePickerEvents(picker) {
			var opt = picker.options;
			picker.widget.on('mousedown', function (e) { return false; });


			picker.widget.on('click', '.datepicker th', function (e) {
				e.stopPropagation(); e.preventDefault();

				var target = $(this);
				if (target.is('.disabled')) { return; }

				switch (target.attr('class')) {
					case 'switch':
						showMode(picker, 1);
						break;
					case 'prev':
					case 'next':
						var step = dpGlobal.modes[picker.viewMode].navStep;
						if (target.attr('class') == 'prev') { step = step * -1; }
						picker.viewDate.add(step, dpGlobal.modes[picker.viewMode].navFnc);
						fillDate(picker);
						break;
				}
			});


			picker.widget.on('click', '.datepicker span', function (e) {
				e.stopPropagation(); e.preventDefault();

				var target = $(this);
				if (target.is('.disabled')) { return; }

				clickMenu(picker, target)
			});


			picker.widget.on('click', '.datepicker td.day', function (e) {
				e.stopPropagation(); e.preventDefault();

				var target = $(this);
				if (target.is('.disabled')) { return; }

				clickDay(picker, target);
			});



			picker.widget.on('click', '[data-action]', function (e) {
				e.stopPropagation(); e.preventDefault();

				var oldDate = moment(picker.date);
				var action = $(e.currentTarget).data('action');
				actions[action].apply(picker, arguments);

				if (!picker.date) { picker.date = moment({ y: 1970 }); }
				set(picker);
				fillTime(picker);
				notifyChange(picker, oldDate, e.type);
			});


			if (opt.pickDate && opt.pickTime) {
				picker.widget.on('click.togglePicker', '.accordion-toggle', function (e) {
					e.stopPropagation();
					var $this = $(this);
					var $parent = $this.closest('ul');
					var expanded = $parent.find('.in');

					if (expanded && expanded.length) {
						var collapseData = expanded.data('collapse');
						if (collapseData && collapseData.date - transitioning) { return; }

						expanded.collapse('hide');
						$parent.find('.collapse:not(.in)').collapse('show');
						$this.find('span').toggleClass(opt.icons.time + ' ' + opt.icons.date);
						picker.element.find('.input-group-addon span').toggleClass(opt.icons.time + ' ' + opt.icons.date);
					}
				});
			}
			if (picker.isInput) {
				picker.element.on({
					'focus.datetimepicker': $.proxy(picker.show, picker),
					'click.datetimepicker': $.proxy(picker.show, picker),
					'blur.datetimepicker': $.proxy(picker.hide, picker),
					'change.datetimepicker': function (e) {
						if (!e.fromPicker) { change(picker); }
					}
				});
			} else {
				picker.element.on('change.datetimepicker', 'input', function (e) {
					if (!e.fromPicker) { change(picker); }
				});

				if (picker.component) {
					picker.component.on('click.datetimepicker', $.proxy(picker.show, picker));
				} else {
					picker.element.on('click.datetimepicker', $.proxy(picker.show, picker));
				}
			}
		}



		function detachDatePickerEvents(picker) {
			picker.widget.off();
			picker.element.off('.datetimepicker');
			if (picker.component) {
				picker.component.off('.datetimepicker');
			}
		}



		function set(picker) {
			var formatted = '';
			if (!picker.unset) { formatted = moment(picker.date).format(picker.format); }

			var year = picker.date.year();
			var trYear = picker.options.transferOutYear;
			picker.inputEl.val(formatted.replace(year, trYear(year)));
			picker.element.data('date', formatted);
			picker.element.trigger({ type: 'change', fromPicker: true });

			if (!picker.options.pickTime) { picker.hide(); }
		}

		function checkDate(picker, direction, unit, amount) {
			var newDate;
			if (direction == "add") {
				newDate = moment(picker.date);
				if (newDate.hours() == 23) newDate.add(amount, unit);
				newDate.add(amount, unit);
			}
			else {
				newDate = moment(picker.date).subtract(amount, unit);
			}
			if (isInDisableDates(picker, moment(newDate.subtract(amount, unit))) || isInDisableDates(picker, newDate)) {
				newDate = newDate.format(picker.format);
				picker.element.trigger({ type: 'dp.error', date: moment(newDate) });
				return;
			}

			if (direction == "add") {
				picker.date.add(amount, unit);
			}
			else {
				picker.date.subtract(amount, unit);
			}
			picker.unset = false;
		}

		function isInDisableDates(picker, date) {
			if (date.isAfter(picker.options.maxDate) || date.isBefore(picker.options.minDate)) { return true; }
			if (picker.options.disabledDates === false) { return false; }
			return (picker.options.disabledDates[moment(date).format("YYYY-MM-DD")] === true);
		}

		function isInEnableDates(picker, date) {
			if (picker.options.enabledDates === false) { return true; }
			return (picker.options.enabledDates[moment(date).format("YYYY-MM-DD")] === true);
		}

		function indexGivenDates(givenDatesArray) {
			var givenDatesIndexed = {};
			var givenDatesCount = 0;
			for (var i = 0; i < givenDatesArray.length; i++) {
				var dDate = moment(givenDatesArray[i]);
				if (dDate.isValid()) {
					givenDatesIndexed[dDate.format("YYYY-MM-DD")] = true;
					givenDatesCount++;
				}
			}
			if (givenDatesCount > 0) { return givenDatesIndexed; }

			return false;
		}

		function padLeft(string) {
			string = string.toString();
			return (string.length >= 2 ? string : '0' + string);
		}



		// ReSharper disable once InconsistentNaming

		var dpgId = 0;

		var icons = {
			time: 'glyphicon glyphicon-time',
			date: 'glyphicon glyphicon-calendar',
			up: 'glyphicon glyphicon-chevron-up',
			down: 'glyphicon glyphicon-chevron-down'
		};



		function DateTimePicker(element, options) {

			this.options = $.extend({}, $.fn.datetimepicker.defaults, options);
			this.options.icons = $.extend({}, icons, this.options.icons);

			if (!(this.options.pickTime || this.options.pickDate)) {
				throw new Error('Must choose at least one picker');
			}

			moment.lang(this.options.language);
			this.element = $(element);

			this.id = dpgId++;
			this.date = moment();
			this.unset = false;
			this.isInput = this.element.is('input');
			this.inputEl = this.isInput ? this.element : this.element.find('input');
			this.component = false;

			if (this.element.hasClass('input-group')) {
				if (this.element.find('.datepickerbutton').size() == 0) {
					this.component = this.element.find("[class^='input-group-']");
				}
				else {
					this.component = this.element.find('.datepickerbutton');
				}
			}

			var longDateFormat = moment()._lang._longDateFormat;

			/* Y 跟 D 大小寫互換 */
			this.format = this.options.format.replace(/[yd]/gi, function (c) {
				switch (c) {
					case 'Y': return 'y';
					case 'y': return 'Y';
					case 'D': return 'd';
					case 'd': return 'D';
					default: return c;
				}
			});
			if (!this.format) {
				this.format = (this.options.pickDate ? longDateFormat.L : '');
				if (this.options.pickDate && this.options.pickTime) {
					this.format += ' ';
				}

				this.format += (this.options.pickTime ? longDateFormat.LT : '');
				if (this.options.useSeconds) {
					if (~longDateFormat.LT.indexOf(' A')) {
						this.format = this.format.split(" A")[0] + ":ss A";
					}
					else {
						this.format += ':ss';
					}
				}
			}
			this.use24hours = this.format.toLowerCase().indexOf("a") < 1;

			var icon = false;
			if (this.component) { icon = this.component.find('span'); };

			if (this.options.pickTime && icon) {
				icon.addClass(this.options.icons.time);
			}
			if (this.options.pickDate && icon) {
				icon.removeClass(this.options.icons.time);
				icon.addClass(this.options.icons.date);
			}

			this.widget = $(getTemplate(this)).appendTo('body');

			if (this.options.useSeconds && !this.use24hours) {
				this.widget.width(300);
			}

			this.minViewMode = this.options.minViewMode || 0;
			if (typeof this.minViewMode === 'string') {
				switch (this.minViewMode) {
					case 'months': this.minViewMode = 1; break;
					case 'years': this.minViewMode = 2; break;
					default: this.minViewMode = 0; break;
				}
			}

			this.viewMode = this.options.viewMode || 0;
			if (typeof (this.viewMode) == 'string') {
				switch (this.viewMode) {
					case 'months': this.viewMode = 1; break;
					case 'years': this.viewMode = 2; break;
					default: this.viewMode = 0; break;
				}
			}

			this.options.disabledDates = indexGivenDates(this.options.disabledDates);
			this.options.enabledDates = indexGivenDates(this.options.enabledDates);

			this.startViewMode = this.viewMode;
			this.setMinDate(this.options.minDate);
			this.setMaxDate(this.options.maxDate);

			fillDow(this);
			fillMonths(this);
			fillHours(this);
			fillMinutes(this);
			fillSeconds(this);
			update(this);
			showMode(this);
			attachDatePickerEvents(this);

			if (this.options.defaultDate !== "" && this.inputEl.val() == "") {
				this.setValue(this.options.defaultDate);
			}
			if (this.options.minuteStepping !== 1) {
				var rInterval = this.options.minuteStepping;
				this.date.minutes((Math.round(this.date.minutes() / rInterval) * rInterval) % 60).seconds(0);
			}

		};




		DateTimePicker.prototype = {

			destroy: function () {
				detachDatePickerEvents(this);
				this.hide();
				this.widget.remove();
				this.element.removeData('DateTimePicker');
				if (this.component) {
					this.component.removeData('DateTimePicker');
				}
			},

			show: function (e) {
				if (e) { e.stopPropagation(); e.preventDefault(); }
				if (this.widget.hasClass("picker-open")) { return; }

				if (this.options.useCurrent && this.inputEl.val() == '') {
					if (this.options.minuteStepping !== 1) {
						var mDate = moment(),
							rInterval = this.options.minuteStepping;
						mDate.minutes((Math.round(mDate.minutes() / rInterval) * rInterval) % 60)
							.seconds(0);
						this.setValue(mDate.format(this.format));
					} else {
						this.setValue(moment().format(this.format));
					}
				}

				this.widget.show();
				this.widget.addClass("picker-open");
				this.height = this.component ? this.component.outerHeight() : this.element.outerHeight();
				this.element.trigger({
					type: 'dp.show',
					date: moment(this.date)
				});

				place(this);
				$(window).on('resize.datetimepicker' + this.id, function () {
					place(this);
				});
				if (!this.isInput) {
					$(document).on('mousedown.datetimepicker' + this.id, $.proxy(this.hide, this));
				}

			},

			hide: function (event) {
				if (event && $(event.target).is(this.element.attr("id"))) { return; }

				var collapse = this.widget.find('.collapse');
				for (var i = 0; i < collapse.length; i++) {
					var collapseData = collapse.eq(i).data('collapse');
					if (collapseData && collapseData.date - transitioning) { return; }
				}

				this.widget.hide();
				this.widget.removeClass("picker-open");
				this.viewMode = this.startViewMode;
				showMode(this);
				this.element.trigger({
					type: 'dp.hide',
					date: moment(this.date)
				});

				$(window).off('resize.datetimepicker' + this.id);
				$(document).off('mousedown.datetimepicker' + this.id);
			},

			disable: function () {
				var input = this.element.find('input');
				if (input.prop('disabled')) { return; }

				input.prop('disabled', true);
				detachDatePickerEvents(this);
			},

			enable: function () {
				var input = this.element.find('input');
				if (!input.prop('disabled')) { return; }

				input.prop('disabled', false);
				attachDatePickerEvents(this);
			},

			setValue: function (newDate) {
				if (!newDate) {
					this.unset = true;
					set(this);
				} else {
					this.unset = false;
				}
				if (!moment.isMoment(newDate)) {
					newDate = moment(newDate, this.format);
				}
				if (newDate.isValid()) {
					this.date = newDate;
					set(this);
					this.viewDate = moment({ y: this.date.year(), M: this.date.month() });
					fillDate(this);
					fillTime(this);
				}
				else {
					this.element.trigger({ type: 'dp.error', date: moment(newDate) });
				}
			},

			getDate: function () {
				return (this.unset ? null : this.date);
			},

			setDate: function (date) {
				var oldDate = moment(this.date);
				this.setValue(date || null);
				notifyChange(this, oldDate, "function");
			},

			setDisabledDates: function (dates) {
				this.options.disabledDates = indexGivenDates(dates);
				if (this.viewDate) { update(this); }
			},
			setEnabledDates: function (dates) {
				this.options.enabledDates = indexGivenDates(dates);
				if (this.viewDate) { update(this); }
			},

			setMaxDate: function (date) {
				if (!date) { return; }
				this.options.maxDate = moment(date);
				if (this.viewDate) { update(this); }
			},

			setMinDate: function (date) {
				if (!date) { return; }
				this.options.minDate = moment(date);
				if (this.viewDate) { update(this); }
			}

		};





		$.fn.datetimepicker = function (options) {
			return this.each(function () {
				var $this = $(this), data = $this.data('DateTimePicker');
				if (!data) $this.data('DateTimePicker', new DateTimePicker(this, options));
			});
		};

		$.fn.datetimepicker.defaults = {
			pickDate: true,
			pickTime: true,
			useMinutes: true,
			useSeconds: false,
			useCurrent: true,
			minuteStepping: 1,
			minDate: new moment({ y: 1900 }),
			maxDate: new moment().add(100, "y"),
			showToday: true,
			collapse: true,
			language: "en",
			defaultDate: "",
			disabledDates: false,
			enabledDates: false,
			icons: {},
			useStrict: false,
			direction: "auto",
			sideBySide: false,
			daysOfWeekDisabled: false,
			transferOutYear: function (year) { return year; },
			transferInDate: function (dateStr) { return dateStr; }
		};

	}));
