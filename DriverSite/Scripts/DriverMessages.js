var currentOrderId;
var intervalID;
var isConfirmIntervalId;
var isOrdered = false;

var driverHub;
var operatorHub;

var counter;

var fromTo = {
    CurrentLatitude: 0,
    CurrentLongitude: 0,
    OrderId: 0,
    LatitudePeekPlace: 0,
    LongitudePeekPlace: 0,
    LatitudeDropPlace: 0,
    LongitudeDropPlace: 0,
    isEnded:0,
};

$(document).on("click", "#double", function () {

	moveToDestination();

});

$(function () {

    GetOrders();

	driverHub = $.connection.DriverHub;


   //Show message from operators
	driverHub.client.showMessage = function (data) {
        // Own function 
	    swal(LocStrings.message, data, 'success');

    };
   
    //Add new order to table
	driverHub.client.newDriverOrders = function (order){
        var content = $("#DrOrder");
        var source = $("#template-article").html();
        var template = Handlebars.compile(source);
        var wrapper = { newOrder: order };
        var html = template(wrapper);
        content.append(html);
    }

    //remove order from table
	driverHub.client.removeAwaitOrders = function (Id) {
        $('#submitButton' + Id).closest('tr').remove();
    }

    //driver can go, show modal form confirm Driver Request
	driverHub.client.confirmDrRequest = function (data)
	{
		if (data == drID)
		{
			$('.successDriverOrder').click();
			moveToClient(fromTo);
		}
    }


    //driver can't go, show modal form denied Driver Request
	driverHub.client.deniedDrRequest = function (data)
	{
	    if (data == drID) {
			$(".submitButton").removeAttr('disabled');
			$('.deniedDriverOrder').click();
			isOrdered = false;
		}
    }

    //Open connection
   /* $.connection.hub.start().done(function () {
        // LogIn

    });*/
});

var initHubDriverMessage = function () {
    var driverRoleId = 1;
    var driverUserId = $('#currentUserId').val();

    //driverHub.server.connectUser(driverUserId);
    //driverHub.server.connectUser(driverRoleId, driverUserId);
    //operatorHub.server.connectUser(driverRoleId, driverUserId);


    //Send message to operators
    $('#showform').click(function () {
        swal({
            title: LocStrings.InputYourMessage,
            html: '<p><textarea id="input-field" style="width: 100%; height: 75px "> </textarea>',
            showCancelButton: true,
            cancelButtonText: LocStrings.Cancel,
            closeOnConfirm: false
        }, function () {
            $.ajax({
                type: 'POST',
                url: sendToOperators,
                data: {
                    message: $('#input-field').val(),
                    username: $('#txtUserName').val()
                },
                dataType: 'json',
                success: function () {},
                error: function () {}
            });
            //  driverHub.server.sendToOperators($('#input-field').val(), $('#txtUserName').val()); //////!!!!!
            swal(LocStrings.MessageHasBeenSent, '', 'success');
        });
    })
}


//get orders from db
function GetOrders() {
    var content = $("#DrOrder");
    $.ajax({
        type: 'POST',
        url: getCurrentDrvOrders,
        dataType: 'json',
        success: function (data) {
            var source = $("#template-article").html();
            var template = Handlebars.compile(source);
            var wrapper = { orders: data };
            var html = template(wrapper);
            content.html(html);
            if (isOrdered) {
                $(".submitButton").attr("disabled", "disabled");
            }
        }
    });
}


$(document).on("click", ".sub", function () {
    currentOrderId = $(this).attr('data-orderid');
});


$(document).on("click", ".assign", function () {
    var time = $("#timetotravel").val();
    var driverUserId = $('#currentUserId').val();

    if (time != "" && time.trim().length != 0) {
        $.ajax({
            url: getCurrentOrder,
            data: { orderId: currentOrderId },
            dataType: 'json',
            success: function (data) {

                fromTo = {
                    CurrentLatitude: currentPosition.lat,
                    CurrentLongitude: currentPosition.lng,
                    OrderId: data.Id,
                    LatitudePeekPlace: data.LatitudePeekPlace,
                    LongitudePeekPlace: data.LongitudePeekPlace,
                    LatitudeDropPlace: data.LatitudeDropPlace,
                    LongitudeDropPlace: data.LongitudeDropPlace
                };

                $(".submitButton").attr("disabled", "disabled");

                data.WaitingTime = time;
                data.DriverId = driverUserId;
                data.DriverName = drName;
                isOrdered = true;

                $.ajax({
                    url: AssignCurrentOrder,
                	data: data,
                	dataType: 'json',
                	success: function () { }
                });

                //Send assigned order to operators
              //  driverHub.server.assignedOrder(data); /////////////!!!!!
            }
        });
    }
});




function moveToClient(fromTo) {

    fromTo.isEnded = 1;
    var Dstlat = fromTo.LatitudePeekPlace;
    var Dstlng = fromTo.LongitudePeekPlace;

    var Srclat = fromTo.CurrentLatitude;
    var Srclng = fromTo.CurrentLongitude;

    var stepLat = (Dstlat - Srclat) / 10;
    var stepLng = (Dstlng - Srclng) / 10;

    counter = 0;
    intervalId = setInterval(function () { runDriver(stepLat, stepLng) }, 1000);
}


function runDriver(stepLat, stepLng) {
    if (counter < 10) {
        fromTo.CurrentLatitude += stepLat;
        fromTo.CurrentLongitude += stepLng;

        var dataCoord =
            {
                Latitude: fromTo.CurrentLatitude,
                Longitude: fromTo.CurrentLongitude,
                Accuracy: '10',
                AddedTime: new Date().toISOString(),
                OrderId: fromTo.OrderId,
                UserId: drID,
                TarifId: '1'
            }

        counter++;

        $.ajax({
            url: SetCoordinates,
            data: dataCoord,
            dataType: 'json',
            success: function () { }
        });
    }


    else {
        clearInterval(intervalId);
        counter = 0;
    }

}

function moveToDestination() {


    var Dstlat = fromTo.LatitudeDropPlace;
    var Dstlng = fromTo.LongitudeDropPlace;

    var Srclat = fromTo.CurrentLatitude;
    var Srclng = fromTo.CurrentLongitude;

    if (fromTo.isEnded == 1)
    {
        fromTo.isEnded = 0;
        var stepLat = (Dstlat - Srclat) / 10;
        var stepLng = (Dstlng - Srclng) / 10;

        counter = 0;
        intervalId = setInterval(function () { runDriverToDst(stepLat, stepLng) }, 1000);
    }

}


function runDriverToDst(stepLat, stepLng) {
    if (counter < 10) {
        fromTo.CurrentLatitude += stepLat;
        fromTo.CurrentLongitude += stepLng;

        var dataCoord =
            {
                Latitude: fromTo.CurrentLatitude,
                Longitude: fromTo.CurrentLongitude,
                Accuracy: '10',
                AddedTime: new Date().toISOString(),
                OrderId: fromTo.OrderId,
                UserId: drID,
                TarifId: '1'
            }

        counter++;

        $.ajax({
            url: SetCoordinates,
            data: dataCoord,
            dataType: 'json',
            success: function () { }
        });
    }


    else {
        clearInterval(intervalId);
        counter = 0;
    }

}