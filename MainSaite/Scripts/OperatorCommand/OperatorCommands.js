var operatorHub;
var driverHub

$(function () {
    GetOrders();
    GetDrRequest();
    GetAwaitOrders();

    driverHub = $.connection.DriverHub;
    operatorHub = $.connection.OperatorHub;

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

    operatorHub.client.removeNewOrders = function (removeOrder)
    {
       // Remove neworder after sending
     }
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

function setOrderStatus(e) {
        var OrderId = $(e).attr('data-orderid');
        var Status = $(e).attr('data-status');
        $.ajax({
            url: '/Order/SetOrderStatus/',
            data: {
                orderId: OrderId,
                status: Status
            },
            success: function (data) {
                debugger;
                console.log(data);
                switch (Status) {
                       case "1": {
                           operatorHub.server.orderForDrivers(data);
                           operatorHub.server.waitingOrderOp(data);
                          // operatorHub.server.removeNewOrder(data);
                           break;
                       }
                    case "2": {/*Here we should provide Denied functions*/ break; }
                    case "4": { operatorHub.server.confirmRequest(data.DriverId); break; }
                    default: break;
                }
            }
        });
    }

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
