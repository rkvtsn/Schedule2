function autocompleteHandler(element) {
	element.autocomplete({
		source: function (request, response) {
			$.ajax({
				url: '/Schedule/GetSubjects',
				type: "POST",
				dataType: "json",
				data: { letter: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return { label: item, value: item };
					}));
				}
			});
		},
		minLength: 2
	});
}

$(document).ready(function () {
	var index = $("#indexer").val(); // Id - баластовый счётчик, но без него совсем скучно
	var groupId = $("#GroupId").val(); // Индетификатор группы
	$(".add_element").click(function () {
		var weekdayId = ($(this).parent("P").parent(".lesson").parent(".weekday").attr("id"));
		var lessonId = ($(this).parent("P").parent(".lesson").attr("id"));
		var htmlPaste = '<li><label for="ScheduleTableRows_' + index + '__Auditory"> Аудитория </label><input id="ScheduleTableRows_' + index + '__Auditory" name="ScheduleTableRows[' + index + '].Auditory" type="text" value="" /><label for="ScheduleTableRows_' + index + '__SubjectName"> Название дисциплины </label><input type="text" class="subjects" id="ScheduleTableRows_' + index + '__SubjectName" name="ScheduleTableRows[' + index + '].SubjectName" /><label for="ScheduleTableRows_' + index + '__LectorName"> Преподаватель </label><input type="text" class="lectors" id="ScheduleTableRows_' + index + '__LectorName" name="ScheduleTableRows[' + index + '].LectorName" /><input id="ScheduleTableRows_' + index + '__LessonId" name="ScheduleTableRows[' + index + '].LessonId" type="hidden" value="' + lessonId + '" /><input id="ScheduleTableRows_' + index + '__GroupId" name="ScheduleTableRows[' + index + '].GroupId" type="hidden" value="' + groupId + '" /><input id="ScheduleTableRows_' + index + '__WeekdayId" name="ScheduleTableRows[' + index + '].WeekdayId" type="hidden" value="' + weekdayId + '" /></li>';
		autocompleteHandler($(this).parent("P").prev(".lesson_form").append(htmlPaste).find('.subjects'));
		index++;
	});
	$(".remove_element").click(function () { 
		var element = $(this).parent("P").prev(".lesson_form").children("LI").last();
		if (element != null) element.remove();
	});
	$(".weekday h3").toggle( 
		function () {
			$(this).parent(".weekday").children(".lesson").hide();
			$(this).children("SPAN").html("показать");
		}, 
		function () {
			$(this).parent(".weekday").children(".lesson").show();
			$(this).children("SPAN").html("скрыть");
		}
	);
	//$(".remove").click(function () { $(this).parent().remove(); });
	autocompleteHandler($("input.subjects"));
});	