var map;
var myLatLng = { lat: 48.3214409, lng: 25.9638791 };
var markers = [];

var Redcar;

var pos;


$(document).ready(function () {

	ShowCurCoord();

	mainInit();

});
function mapInit() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 48.3214409, lng: 25.8638791 },
        zoom: 8
    })
}

function hubInit() {

    var hub = $.connection.driversLocationHub;//Подключились к хабу

    hub.client.locationUpdate = locationUpdate;//присобачили функцию клиента
    hub.client.driverStart = driverStart;
    hub.client.driverFinish = driverFinish;

    $.connection.hub.start().done(function () {
       // hub.server.Hello(); // вызов функции сервера
    });
}
//test -----
var ShowCurCoord = function () {

	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(function (position) {
			 pos = {
				lat: position.coords.latitude,
				lng: position.coords.longitude,
			 };
			 var k = 0;
		}, function () {

		});
	}
}

//test--------

function mainInit() {
    mapInit();
    $.ajax({
        type: "GET",
        url: '/Administration/GetLoc/',
        dataType: "json",
        success: function (data) {

        	//test -- 

        	var testObj = {
        		name: "Jack",
        		id: 1,
        		startedTime: data,
        		updateTime: data,
        		latitude: 48.2760595,
        		longitude: 25.9490
        	};
        	AddDriverToTheTable(testObj);

        	var testObj1 = {
        		name: "Nick",
        		id: 2,
        		startedTime: data,
        		updateTime: data,
        		latitude: 48.2547794,
        		longitude: 25.9373924
        	};
        	AddDriverToTheTable(testObj1);

			//test -- 


        	//for (var i = 0; i < data.length; i++) {
        	//	AddDriverToTheTable(data[i]);
            //}
        },
        error: function (error) {
        	alert(error.statusText);
        }
    });
    hubInit();
}

function AddDriverToTheTable(value)
{
	markers['DriverN' + value.id] = AddDriver(value.name, value.latitude, value.longitude);
	var tableRow = $('<tr/>', { id: 'DriverN' + value.id }).append(
			$('<td/>', { text: value.name }),
			$('<td/>', { text: value.startedTime, id: 'DriverN' + value.id + 'start' }),
			$('<td/>', { text: value.updateTime, id: 'DriverN' + value.id + 'up' }));
			//$('<td/>', { text: new Date(+value.startedTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + value.id + 'start' }),
			//$('<td/>', { text: new Date(+value.updateTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + value.id + 'up' }));
	tableRow.click(onClick);
	var table = $('#DrvsCont').append(
		tableRow
	);
}

function locationUpdate(Lat, Lng, Time, ID)
{
    if(markers['DriverN'+ID] !== undefined)
    {
        markers['DriverN' + ID].setPosition(new google.maps.LatLng(Lat, Lng));
        $('#DriverN' + ID + 'up').html(new Date(Time).toLocaleString());
    }
}

function driverStart(val)
{
    if (markers['DriverN' + val.id] === undefined)
    {
        markers['DriverN' + val.id] = AddDriver(val.id, val.latitude, val.longitude);
        var tr = $('<tr/>', { id: 'DriverN' + val.id }).append(
                $('<td/>', { text: val.name }),
                $('<td/>', { text: new Date(val.startedTime).toLocaleString(), id: 'DriverN' + val.id + 'start' }),
                $('<td/>', { text: new Date(val.updateTime).toLocaleString(), id: 'DriverN' + val.id + 'up' }));
        tr.click(onClick);
        var table = $('#DrvsCont').append(tr);
    }
    else
    {
        locationUpdate(val.latitude, val.longitude, val.updateTime, val.id);
        $('#DriverN' + val.id + 'start').html(new Date(val.startedTime).toLocaleString());
    }
}

function driverFinish(ID)
{
    $('#DriverN' + ID).remove();
    markers['DriverN' + ID].setMap(null);
    markers['DriverN' + ID] = undefined;

}
///end hub
function onClick(data)
{
	for (var key in markers) {
		markers[key].setAnimation(null);
	}
    if ($(this).hasClass('bold')) {
        markers[(this).id].setIcon(imagePath + '/cab.png');
        $(this).removeClass('bold');
        Redcar = undefined;
    }
    else
    {
        if (Redcar !== undefined) {
            Redcar.removeClass('bold');
            markers[Redcar.attr('id')].setIcon(imagePath + '/cab.png');
        }
        markers[(this).id].setIcon(imagePath + '/cabRed.png');
        markers[(this).id].setAnimation(google.maps.Animation.BOUNCE);
        $(this).addClass('bold');
        Redcar = $(this);
    }
}

function AddDriver(name, myLat, myLng) {
    return marker = new google.maps.Marker({
        position: { lat: myLat, lng: myLng },
        map: map,
        title: 'Driver: ' + name,
        icon: {
            url: imagePath+'/cab.png'
        }
    });
}