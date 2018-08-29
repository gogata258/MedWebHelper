function updateDates(dates) {
	$("#Time").empty();
	dates.forEach(t => $("#Time").append($('<option>', { text: t })));
}

function getDates() {
	$.ajax({
		type: "GET",
		url: "/user/appointments/getavaliableappointments/}",
		data: {
			'doctorId': $("#DoctorId").val(),
			'dateTime': $("#Date").val()
		},
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (response) { updateDates(response); }
	});
}