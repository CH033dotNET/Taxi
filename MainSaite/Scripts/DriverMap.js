var map;
var myLatLng = { lat: 48.3214409, lng: 25.9638791 };
var markers = new Array();
var dimine = function () {
    alert("GOGOGOG");
}
function mapinit() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 48.3214409, lng: 25.8638791 },
        zoom: 8
    })
}

function hubInit() {
    var hub = $.connection.MyHub1;//Подключились к хабу

    hub.client.dimine = dimine;//присобачили функцию клиента

    $.connection.hub.start().done(function () {
        hub.server.Hello(); // вызов функции сервера
    });
}


function mainInit() {
    mapinit();
    $.ajax({
        type: "POST",
        url: "GetLoc",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var val = data[i];
                markers['DriverN'+val.name] = adddriver(val.latitude, val.longitude);
                var tr = $('<tr/>', {id:'DriverN'+val.name }).append(
                        $('<td/>', { text: val.name }));
                //tr.marker = adddriver(val.latitude, val.longitude);
                tr.hover(inhov, outhov);
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

function inhov(data)
{
    markers[(this).id].setIcon(imagePath + '/cabRed.png');
}
function outhov(data)
{
    markers[(this).id].setIcon(imagePath + '/cab.png');
}
function adddriver(myLat, myLng) {
    return marker = new google.maps.Marker({
        position: { lat: myLat, lng: myLng },
        map: map,
        title: 'Hello World!',
        //label: "1",
        icon: {
            url: imagePath+'/cab.png'
        }
    });
}