document.addEventListener("DOMContentLoaded", buttonCheck);

function buttonCheck() {
	$.ajax({
		url: "/Driver/CheckWorkShifts/",
		method: "POST",
		dataType: "JSON",
		data: { Id: WorkerId }
	}).done(function (result) {
		if (result == true) {
			//change button to End Workshift
			//$("#workshift-button").changeClass("btn btn-success btn_end");
			$("#workshift-button").attr("change-btn-id", "work_end"); // changing custom attribute value
			$("#workshift-button").attr('class', 'btn btn-warning'); // changing buttons class
			$("#workshift-button").prop('value', 'End workshift'); // changing buttons text
		}
		else {
			//$("#workshift-button").changeClass("btn btn-success btn_start");
			// change button to StartWorkshift
			$("#workshift-button").attr("change-btn-id", "work_start"); // changing custom attribute value
			$("#workshift-button").attr('class', 'btn btn-success'); // changing custom attribute value
			$("#workshift-button").prop('value', 'Start workshift'); // changing buttons text
		};
	});
}

function buttonChangeOnClick() {
	var isValue = document.getElementById('workshift-button').value;
	if (isValue == "Start workshift") {
		$("#workshift-button").attr("change-btn-id", "work_end"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-warning'); // changing buttons class
		$("#workshift-button").prop('value', 'End workshift'); // changing buttons text
		setBeginlocation();
	}
	else if (isValue == "End workshift") {
		$("#workshift-button").attr("change-btn-id", "work_start"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-success'); // changing custom attribute value
		$("#workshift-button").prop('value', 'Start workshift'); // changing buttons text
		setEndlocation();
	};
}