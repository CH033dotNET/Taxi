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
        getOrderRed(newWaitOrder.Id);

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


    

	// function that takes an awating order id as parameter and mark a row containing this order as expired.
    function getOrderRed(id) {
    	var rowToPaint = document.getElementById(id);
    	if (rowToPaint.className == null || rowToPaint.className == undefined) {
    		setTimeout(function () { rowToPaint.className = "expiredOrderClass"; }, 300000)
    	}
    	else {
    		setTimeout(function () { rowToPaint.className = rowToPaint.className + " expiredOrderClass"; }, 300000)
    	}
    }
	//! Don`t touch this. +_+
    function checkExpiredOrders() {
    	var timeNow = new Date();
    	var awaitingOrders = $('#waitingOrdersContent'); // array of awating orders here. Need glodal variable to store current lists
    	for (var i = 0; i < awaitingOrders.length; i++) {
    		if (awaitingOrders[i].OrderTime != timeNow) {
    			var orderTime = awaitingOrders[i].OrderTime;
    			var dateDiff = Math.abs(orderTime - timeNow);
    			alert(dateDiff);
    		}
    	}
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
        }
    });
}
