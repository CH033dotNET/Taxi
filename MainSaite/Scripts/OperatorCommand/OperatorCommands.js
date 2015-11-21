var operatorHub;
var driverHub;


$(function () {
    GetOrders();
    GetDrRequest();
    GetAwaitOrders();

    driverHub = $.connection.DriverHub;
    operatorHub = $.connection.OperatorHub;

    //Show message from Driver
    driverHub.client.showMessage = function (message, userName) {
    // Own function 
        swal('Message from '+userName+':', message, 'success');
    };

	// function that takes an object as an input parameter from a hub call and appends a waitingOrder table with its data.
    operatorHub.client.addWaitingOrder = function (newWaitOrder)
    {
        var waitingOrders = $('#waitingOrdersContent');
        var source = $('#waitingOrderTemplate').html();
        var template = Handlebars.compile(source);
        var wrapper = { waitingOrder: newWaitOrder };
        var html = template(wrapper);
        waitingOrders.append(html);
        setOrderExpired(newWaitOrder.Id); // set waiting order as expired after 5 minutes

        //delete waiting order from New orders table
        $('#submitOrdering' + newWaitOrder.Id).closest('tr').remove();
    }

    // delete order from confirmed table
    operatorHub.client.deleteDrRequest = function (OrderId)
    {
        $('#deniedDrRequest' + OrderId).closest('tr').remove();
    }

    //delete Denied Client Order from New Order table
    operatorHub.client.deleteDeniedOrder = function (orderId) {
        $('#deniedOrdering' + orderId).closest('tr').remove();
    }

    //delete Await Order from New Order table
    operatorHub.client.removeAwaitOrders = function (orderId) {
        $('#waitingOrders' + orderId).closest('tr').remove();
    }

    //delete Order from confirmed table
    operatorHub.client.removeAwaitOrderFromOperators = function (orderId) {
        $('#submitDrRequest' + orderId).closest('tr').remove();
    }


    //Get new order from client, add to first table
    operatorHub.client.newOrderFromClient = function (newOrder) {
        var content = $('#orderContent');
        var source = $("#orderTemplate").html();
        var template = Handlebars.compile(source);
        var wrapper = { object: newOrder };
        var html = template(wrapper);
        content.append(html);

    }



    //Add new assigned order from driver
    driverHub.client.assignedDrOrder = function (newRequest) {
        var content = $('#driverRequest');
        var source = $("#driverRequestTemplate").html();
        var template = Handlebars.compile(source);
        var wrapper = { request: newRequest };
        var html = template(wrapper);
        content.append(html);
    }
    //Open connection

    $.connection.hub.start().done(function () {
        // LogIn

        var operatorRoleId = 2;
        var operatorUserId = $('#currentUserId').val();


        driverHub.server.connectUser(operatorRoleId, operatorUserId);
        operatorHub.server.connectUser(operatorRoleId, operatorUserId);

        //Broadcast message to all drivers
        $('#showform').click(function () {
            swal({
                title: 'Input Your message:',
                html: '<p><input id="input-field">',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
                operatorHub.server.sendToDrivers($('#input-field').val());
                swal('Your message has been sent', '', 'success');
            });
        })


    });
});


$(document).on("click", ".ordrerAction", function () {
    var OrderId = $(this).attr('data-orderid');
    var Status = $(this).attr('data-status');
    $.ajax({
        url: '/Order/SetOrderStatus/',
        data: {
            orderId: OrderId,
            status: Status
        },
        success: function (data) {
            switch (Status) {
                case "1": {

                    //Send order to driver's table
                    operatorHub.server.orderForDrivers(data);

                    //send wait order to all operators and delete from newOrdertable
                    operatorHub.server.waitingOrderOp(data);

                    break;
                }
                case "2": {
                    //denied Client Order
                    operatorHub.server.deniedClientOrder(data.Person.UserId);

                    //delete denied order from table
                    operatorHub.server.deleteDeniedClientOrder(OrderId);

                    break;
                }

                case "4": {
                    //Submit driver request
                    var goodDriverId = $('#submitDrRequest'+OrderId).parent('td').prev('td').text();
                    var waitingTime = $('#submitDrRequest' + OrderId).parent('td').prev('td').prev('td').text();

                    //Set order to current driver
                    $.ajax({
                        url: "/Order/SetOrderToDriver/",
                        data: { orderId: OrderId, waitingTime: waitingTime, DriverId: goodDriverId },
                        dataType: 'json',
                        success: function (data) {
                            operatorHub.server.confirmClientOrder(data.WaitingTime, data.Latitude, data.Longitude);
                        }
                    });

                    //send confirmRequest to selected driver
                    operatorHub.server.confirmRequest(goodDriverId);

                    //remove current order from drivers
                    operatorHub.server.removeAwaitOrder(OrderId);

                    //remove current order from operators
                    operatorHub.server.removeAwaitOrderFromOperators(OrderId);

                    //Remove current order from awaiting table operators
                    operatorHub.server.removeAwaitOrders(OrderId);

                    //deny for others drivers
                    $('.deny'+OrderId).click();
                    break;
                }

                case "5": {

                    //Send message to client "No free Cars"
                    operatorHub.server.noFreeCarClientOrder(data.Person.UserId);

                    //Show modal window for driver "denied", if he confirmed current order
                    $('.deny' + OrderId).click();

                    //remove current order from operators (confirmed by drivers)
                    operatorHub.server.removeAwaitOrderFromOperators(OrderId);

                    //Remove current order from awaiting table operators
                    operatorHub.server.removeAwaitOrders(OrderId);

                    //Remove current order from table drivers
                    operatorHub.server.removeAwaitOrder(OrderId);

                    break;
                }

                case "6": {
                    driverId = $('#submitDrRequest' + OrderId).parent('td').prev('td').text();

                    //deny driver request and delete from confirmed table
                    operatorHub.server.deniedRequest(driverId, OrderId);
                    break;
                }
                default: break;
            }
        }
    });
});


//get orders from db
function GetOrders() {
    var content = $('#orderContent');
    $.ajax({
        type: 'POST',
        url: "/Order/OrdersData/",
        dataType: 'json',
        success: function (data) {
            var source = $("#orderTemplate").html();
            var template = Handlebars.compile(source);
            var wrapper = { objects: data };
            var html = template(wrapper);
            content.html(html);
        }
    });
}



function GetDrRequest() {
    var content = $('#driverRequest');
    $.ajax({
        type: 'POST',
        url: "/Order/DriversRequest/",
        dataType: 'json',
        success: function (data) {
            var source = $("#driverRequestTemplate").html();
            var template = Handlebars.compile(source);
            var wrapper = { requests: data };
            var html = template(wrapper);
            content.html(html);
        }
    });
}

function GetAwaitOrders() {
    var waitingOrders = $('#waitingOrdersContent');
    $.ajax({
        type: 'POST',
        url: "/Order/GetWaitingOrders/",
        dataType: 'json',
        success: function (data) {
            var source = $('#waitingOrderTemplate').html();
            var template = Handlebars.compile(source);
            var wrapper= { waitingOrders: data };
            var html = template(wrapper);
            waitingOrders.html(html)
            checkExpiredOrders(data); // check all awating orders wether they are expired (Time now > OrderTime for more tha 5 mins)
        }
    });
}

//function that checks array of awating orders wether they are expired or not.
function checkExpiredOrders(OrdersList) {
	var timeNow = moment();
	var awaitingOrders = OrdersList; // array of awating orders here. Need glodal variable to store current lists
	for (var i = 0; i < awaitingOrders.length; i++) {
		if (awaitingOrders[i].OrderTime != timeNow) { // chacking if datetimes are equal or not. If not:
			var dayDiff = timeNow.diff(awaitingOrders[i].OrderTime, "days"); // checking day difference
			var hourDiff = timeNow.diff(awaitingOrders[i].OrderTime, "hours"); // checking hour difference
			var minuteDiff = timeNow.diff(awaitingOrders[i].OrderTime, "minutes"); // checking minute difference
			if (dayDiff >= 1 || hourDiff >= 1) { // if dateNow hour day or hour value is bigger then order`s
				applyExpiredClass(awaitingOrders[i].OrderId);
			}
			else if (minuteDiff >= 5) { // if dateNow day or hour value is equal to order`s but minutes count is bigger
				applyExpiredClass(awaitingOrders[i].OrderId);
			}
			else return false;
			//var timeNowHours = moment(timeNow).hours(); // hours in date-time now
			//var orderTimeHours = moment(awaitingOrders[i].OrderTime).hours(); // hours in orderTime
			//var timeNowMinutes = moment(timeNow).minutes(); // minutes in date-time now
			//var orderTimeMinutes = moment(awaitingOrders[i].OrderTime).minutes(); // hours in orderTime

			//if (timeNowHours > orderTimeHours) { applyExpiredClass(awaitingOrders[i].OrderId) }
			//else {
			//	var timeNowMinutes = moment(timeNow).minutes();
			//	var orderTimeMinutes = moment(awaitingOrders[i].OrderTime).minutes();
			//	var dateDiff = Math.abs(timeNowMinutes - orderTimeMinutes);
			//	if (dateDiff > 5) { applyExpiredClass(awaitingOrders[i].OrderId) }
			//	else return false;
			//}
		}
	}
}

// function that takes an awating order id as parameter and mark a row containing this order as expired.
function setOrderExpired(id) {
	var rowToPaint = document.getElementById(id);
	if (rowToPaint.className == null || rowToPaint.className == undefined) {
		setTimeout(function () {
			var checkRowAgain = document.getElementById(id); // check again if element is available.
			if (checkRowAgain == null) { return false }
			else { rowToPaint.className = "expiredOrderClass"; }
		}, 300000)
	}
	else {
		setTimeout(function () {
			var checkRowAgain = document.getElementById(id); // check again if element is available.
			if (checkRowAgain == null) { return false }
			else { checkRowAgain.className = rowToPaint.className + " expiredOrderClass"; }
		}, 300000)
	}
}

// function that add expired class immediately
function applyExpiredClass(id) {
	if (document.readyState == 'complete') {
		var rowToPaint = document.getElementById(id);
		if (rowToPaint.className == null || rowToPaint.className == undefined) {
			rowToPaint.className = "expiredOrderClass";
		}
		else {
			rowToPaint.className = rowToPaint.className + " expiredOrderClass";
		}
	}
	else alert("not ready");
}
