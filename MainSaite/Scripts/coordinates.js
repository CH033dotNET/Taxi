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
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.TimeStart = storage.getItem("StartWorkTime");

    $.ajax({
        url: '/Driver/WorkStateChange',
        method: 'POST',
        data: dataObj,
        success: function (success) {
            storage.removeItem("StartWorkTime");
            location.reload(true);
        }

    });

}

function getEndCoord(position) {
    timeNow = moment().format('YYYY/MM/DD HH:mm:ss');
    if (storage.getItem("StopWorkTime") === null)
        storage.setItem("StopWorkTime", timeNow);

    var dataObj = {};
    dataObj.Id = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.TimeStop = storage.getItem("StopWorkTime");

    $.ajax({
        url: '/Driver/WorkStateEnded',
        method: 'POST',
        data: dataObj,
        success: function (success) {
            storage.removeItem("StopWorkTime");
            location.reload(true);

        }

    });

}

function setBeginlocation() {
    if (navigator.geolocation) {
        navigator.geolocation.watchPosition(getBeginCoord);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}
function setEndlocation() {
    if (navigator.geolocation) {
        navigator.geolocation.watchPosition(getEndCoord);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}