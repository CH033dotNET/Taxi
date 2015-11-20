var currentOrderId;
var intervalID;
var isConfirmIntervalId;
var isOrdered = false;
var driverHub;
var operatorHub;

$(function () {
    GetOrders();

   operatorHub = $.connection.OperatorHub;
   driverHub = $.connection.DriverHub;
   operatorHub.client.showMessage = function (message) {
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

    operatorHub.client.removeAwaitOrders = function (orderId) {
        $('#submitButton' + orderId).closest('tr').remove();
    }
    operatorHub.client.confirmDrRequest = function ()
    {
        console.log('successful');
        $('.successDriverOrder').click();
    }

    operatorHub.client.deniedDrRequest = function ()
    {
           $(".submitButton").removeAttr('disabled');
           $('.deniedDriverOrder').click();
           isOrdered = false;
    }

    //Open connection
    $.connection.hub.start().done(function () {
        // LogIn
        var driverRoleId = 1;
        var driverUserId = $('#currentUserId').val();

        driverHub.server.connectUser(driverRoleId, driverUserId);
        operatorHub.server.connectUser(driverRoleId, driverUserId);

        $('#showform').click(function () {
            swal({
                title: 'Input Your message:',
                html: '<p><input id="input-field">',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
                driverHub.server.sendToOperators($('#input-field').val(), $('#txtUserName').val());
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
    var driverUserId = $('#currentUserId').val();

    if (time != "" && time.trim().length != 0) {
        $.ajax({
            url: "/Driver/GetCurrentOrder/",
            data: { orderId: currentOrderId},
            dataType: 'json',
            success: function (data) {

                $(".submitButton").attr("disabled", "disabled");

                data.WaitingTime = time;
                data.DriverId = driverUserId;

                isOrdered = true;
                driverHub.server.assignedOrder(data);
                //driverHub.server.reservOrder(innerId);
            }
        });

        $.ajax({
            url: "/Driver/GetOrder/",
            data: { orderId: currentOrderId, waitingTime: time},
            dataType: 'json',
            success: function (data) { }
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