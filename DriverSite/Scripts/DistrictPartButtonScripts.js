function buttonChangeOnClick() {
	var isValue = document.getElementById('workshift-button').value;
	if (isValue == "Start workshift") {
		$("#workshift-button").attr("change-btn-id", "work_end"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-warning'); // changing buttons class
		$("#workshift-button").prop('value', 'End workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { buttonChangeOnClick(); }; // changing onclick property value
		setBeginlocation();
		setDriverStatus(3);
		changeStatusDisplay(1);
	}
	else if (isValue == "End workshift") {
		$("#workshift-button").attr("change-btn-id", "work_start"); // changing custom attribute value
		$("#workshift-button").attr('class', 'btn btn-success'); // changing custom attribute value
		$("#workshift-button").prop('value', 'Start workshift'); // changing buttons text
		document.getElementById('workshift-button').onclick = function () { checkMainCars(); }; // changing onclick property value
		setEndlocation();
		setDriverStatus(0);
		changeStatusDisplay(0);
	};
}