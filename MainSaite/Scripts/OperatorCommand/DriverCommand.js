﻿var currentOrderId;
var intervalID;
var isConfirmIntervalId;
var isOrdered = false;
var driverHub;

$(function () {
    GetOrders();
    var chat = $.connection.messagesHub;
    var operatorHub = $.connection.OperatorHub;
   driverHub = $.connection.DriverHub;
    chat.client.showMessage = function (message) {
        // Own function 
        swal('New message from operator!', message, 'success');

    };
   
    operatorHub.client.newDriverOrders = function (order)
    {
        var content = $("#DrOrder");
        var source = $("#template-article").html();
        var template = Handlebars.compile(source);
        var wrapper = { newOrder: order };
        var html = template(wrapper);
        content.append(html);
    }
    operatorHub.client.confirmDrRequest = function ()
    {
        debugger;
        $('.successDriverOrder').click();
    }
    //Open connection
    $.connection.hub.start().done(function () {
        // LogIn
        var roleId = $("#txtRoleId").val();
        var driverRoleId = 1;
        var driverId = $('#currentUserId').val();
         chat.server.connect(roleId);
         driverHub.server.privateDriverLine(driverId);
         operatorHub.server.connectUser(driverRoleId);
        $('#showform').click(function () {
            swal({
                title: 'Input Your message:',
                html: '<p><input id="input-field">',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
                chat.server.sendToOperators($('#input-field').val(), $('#txtUserName').val());
                swal('Your message has been sent', '', 'success');
            });
        })
    });
});

function GetOrders() {
    var content = $("#DrOrder");
    $.ajax({
        type: 'POST',
        url: "/Driver/GetDriverOrders/",
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


function saveOrderId(e) {
    currentOrderId = $(e).attr('data-orderid');
}

function Assign() {
    var time = $("#timetotravel").val();
    var innerId = currentOrderId;
    if (time != "" && time.trim().length != 0) {
        $.ajax({
            url: "/Driver/GetOrder/",
            data: { orderId: innerId, waitingTime: time },
            dataType: 'json',
            success: function (data) {
                $(".submitButton").attr("disabled", "disabled");
                isOrdered = true;
                driverHub.server.assignedOrder(data);
            }
        });
    }
}

function OrderConfirmStatus()
{
    $.ajax({
        type: 'POST',
        url: "/Driver/GetCurrentOrder/",
        dataType: 'json',
        data: { orderId: currentOrderId },
        success: function (data) {
            switch (data) {
                case 1: break;
                case 2:
                    {
                        $('.deniedDriverOrder').click();
                        isOrdered = false;
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                default: break;
            }
        }
    });

}