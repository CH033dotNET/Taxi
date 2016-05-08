var timeNow;
var storage = window.localStorage;

function getBeginCoord(position) {
	timeNow = moment().format('YYYY/MM/DD HH:mm:ss');
	if (storage.getItem("StartWorkTime") === null)
		storage.setItem("StartWorkTime", timeNow);

	var dataObj = {};
	dataObj.Id = document.getElementById('Id').value;
	dataObj.Latitude = position.coords.latitude;
	dataObj.Longitude = position.coords.longitude;

	//getDistrictIdByCoordinate is in DistrictViewer.js 
	var district = getDistrictIdByCoordinate(new google.maps.LatLng(dataObj.Latitude, dataObj.Longitude));
	dataObj.Accuracy = position.coords.accuracy;
	dataObj.TimeStart = storage.getItem("StartWorkTime");

	$.ajax({
		url: './Driver/WorkStateChange',
		method: 'POST',
		data: dataObj,
		success: function (success) {
			storage.removeItem("StartWorkTime");
		}

	});
	if (district) {
		changeDistrict(district);
	}


}

function getEndCoord(position) {
	timeNow = moment().format('YYYY/MM/DD HH:mm:ss');
	if (storage.getItem("StopWorkTime") === null)
		storage.setItem("StopWorkTime", timeNow);
	timeNow = undefined;
	var dataObj = {};
	dataObj.Id = document.getElementById('Id').value;
	dataObj.Latitude = position.coords.latitude;
	dataObj.Longitude = position.coords.longitude;
	dataObj.Accuracy = position.coords.accuracy;
	dataObj.TimeStop = storage.getItem("StopWorkTime");

	$.ajax({
		url: './Driver/WorkStateEnded',
		method: 'POST',
		data: dataObj,
		success: function (success) {
			storage.removeItem("StopWorkTime");
			$('#DistrictN' + currentDistrict + ">.text").html(joinToLocation);
			hub.server.swap(0, currentDistrict);
			currentDistrict = 0;
		}

	});

}

function setBeginlocation() {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(getBeginCoord);
	} else {
		alert("Geolocation is not supported by this browser.");
	}
}
function setEndlocation() {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(getEndCoord);
	} else {
		alert("Geolocation is not supported by this browser.");
	}
}