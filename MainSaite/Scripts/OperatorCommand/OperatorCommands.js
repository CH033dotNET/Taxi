$(function () {
    var chat = $.connection.messagesHub;

    chat.client.showMessage = function (message) {
    // Own function 
        alert(message);
    };


    //Open connection
    $.connection.hub.start().done(function () {

    // LogIn
        var roleId = $("#txtRoleId").val();
        chat.server.connect(roleId);


        $('#sendmessage').click(function () {
    // Start Hub Method 
            chat.server.sendToDrivers($('#message').val());
            $('#message').val('');
        });

    });
});


$(document).ready(function () {
    setInterval(function () {
        GetOrders();
        GetDrRequest();
        GetAwaitOrders()
    }, 2000)
});

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
$('.waitingOrders').on('click', function (e) {
    $('.nocarorder').trigger('click');
})

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
            GetOrders();
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
