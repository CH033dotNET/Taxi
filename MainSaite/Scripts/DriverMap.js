var map;
var myLatLng = { lat: 48.3214409, lng: 25.9638791 };
var markers = new Array();
var dimine = function () {
    alert("GOGOGOG");
}
var Redcar;
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


function mainInit() {
    mapInit();
    $.ajax({
        type: "POST",
        url: "GetLoc",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var val = data[i];
                markers['DriverN'+val.id] = adddriver(val.latitude, val.longitude);
                var tr = $('<tr/>', {id:'DriverN'+val.id }).append(
                        $('<td/>', { text: val.name }),
                        $('<td/>', { text: new Date(+val.startedTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + val.id + 'start' }),
                        $('<td/>', { text: new Date(+val.updateTime.match(/\d+/)[0]).toLocaleString(), id: 'DriverN' + val.id + 'up'}));
                //tr.marker = adddriver(val.latitude, val.longitude);
                tr.click(onclic);
                var table = $('#DrvsCont').append(
                    tr
                );
                
                /*var td = tr.append(
                        $('<td/>', { text: val.name })
                );*/
                //td.hover(inhov, outhov);
            }
        },
        error: function (error) {
            alert(error);
        }
    });
    hubInit();
}

$(document).ready(mainInit);
///hub
//using to ...
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
        markers['DriverN' + val.id] = adddriver(val.latitude, val.longitude);
        var tr = $('<tr/>', { id: 'DriverN' + val.id }).append(
                $('<td/>', { text: val.name }),
                $('<td/>', { text: new Date(val.startedTime).toLocaleString(), id: 'DriverN' + val.id + 'start' }),
                $('<td/>', { text: new Date(val.updateTime).toLocaleString(), id: 'DriverN' + val.id + 'up' }));
        tr.click(onclic);
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
function onclic(data)
{
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
        $(this).addClass('bold');
        Redcar = $(this);
    }
}

function adddriver(myLat, myLng) {
    return marker = new google.maps.Marker({
        position: { lat: myLat, lng: myLng },
        map: map,
        title: 'Hello World!',
        icon: {
            url: imagePath+'/cab.png'
        }
    });
}