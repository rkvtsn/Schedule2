﻿$(document).ready(function () {
	$("#colorSwitcher").click(function () {
		$.ajax({
			type: "POST",
			contentType: "application/json;charset=utf-8",
			url: "/Default/ChangeTheme",
			data: '{"id": "changeMe"}',
			dataType: "json",
			success: changeTheme
		});
	});
});

function changeTheme(data) {
	$("body").fadeOut(function () {
		if (data == "True")
			$("body").addClass("bw");
		else
			$("body").removeClass();
	}).fadeIn();
}