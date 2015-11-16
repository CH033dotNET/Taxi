﻿var operatorHub;
$(function () {
    GetOrders();
    GetDrRequest();
    GetAwaitOrders()
    var signal = $.connection.messagesHub;
   var driverHub = $.connection.DriverHub;
    operatorHub = $.connection.OperatorHub;

    signal.client.showMessage = function (message, userName) {
    // Own function 
        swal('Message from '+userName+':', message, 'success');
    };

    signal.client.addNewOrderToTable = function (newOrder)
    {
        var content = $('#orderContent');
        var source = $("#orderTemplate").html();
        var template = Handlebars.compile(source);
        var wrapper = {object: newOrder};
        var html = template(wrapper);
        content.append(html);
    }
    operatorHub.client.addWaitingOrder = function (newWaitOrder)
    {
        var waitingOrders = $('#waitingOrdersContent');
        var source = $('#waitingOrderTemplate').html();
        var template = Handlebars.compile(source);
        var wrapper = { waitingOrder: newWaitOrder };
        var html = template(wrapper);
        waitingOrders.append(html);
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
        var operatorId = 2;
        var roleId = $("#txtRoleId").val();
        signal.server.connect(roleId);
        driverHub.server.connectUser(operatorId);
        operatorHub.server.connectUser(operatorId);
        $('#showform').click(function () {
            swal({
                title: 'Input Your message:',
                html: '<p><input id="input-field">',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
                signal.server.sendToDrivers($('#input-field').val());
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
