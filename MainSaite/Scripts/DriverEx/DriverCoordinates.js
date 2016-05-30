var timeNow;
var storage = window.localStorage;

function setBeginlocation() {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) {

			timeNow = moment().format('YYYY/MM/DD HH:mm:ss');
			if (storage.getItem("StartWorkTime") === null)
				storage.setItem("StartWorkTime", timeNow);

			var dataObj = {};
			dataObj.Id = document.getElementById('Id').value;
			dataObj.Latitude = position.coords.latitude;
			dataObj.Longitude = position.coords.longitude;
			dataObj.Accuracy = position.coords.accuracy;
			dataObj.TimeStart = storage.getItem("StartWorkTime");
			$.ajax({
				url: './DriverEx/WorkShiftStarted',
				method: 'POST',
				data: dataObj,
				success: function (success) {
					storage.removeItem("StartWorkTime");
				}
			});
		});
	} else {
		alert("Geolocation is not supported by this browser.");
	}
}
function setEndlocation() {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) {
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
				url: './DriverEx/WorkShiftEnded',
				method: 'POST',
				data: dataObj,
				success: function (success) {
					storage.removeItem("StopWorkTime");
					//$('#DistrictN' + currentDistrict + ">.text").html(joinToLocation);
					//hub.server.swap(0, currentDistrict);
					//currentDistrict = 0;
				}

			});
		});
	} else {
		alert("Geolocation is not supported by this browser.");
	}
}