$(document).ready(function () {
    $('#btn_begin').click(function () {
        setBeginlocation();
    });
    $('#btn_end').click(function () {
        setEndlocation();
    });
});

function getBeginCoord(position) {
    var dataObj = {};
    dataObj.Id = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    dataObj.Accuracy = position.coords.accuracy;

    $.ajax({
        url: '/Driver/WorkStateChange',
        method: 'POST',
        data: dataObj,
        success: function (success) { location.reload(true); },
        error: function (e) { }
    });

}

function getEndCoord(position) {
    var dataObj = {};
    dataObj.Id = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    dataObj.Accuracy = position.coords.accuracy;

    $.ajax({
        url: '/Driver/WorkStateEnded',
        method: 'POST',
        data: dataObj,
        success: function (success) { location.reload(true); },
        error: function (e) { }
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