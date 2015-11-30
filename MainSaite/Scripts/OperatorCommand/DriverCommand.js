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

    //Show message from operators
   operatorHub.client.showMessage = function (message) {
       // Own function
       swal(LocStrings.message, message, 'success');
   };
   
    //Add new order to table
    operatorHub.client.newDriverOrders = function (order)
    {
        var content = $("#DrOrder");
        var source = $("#template-article").html();
        var template = Handlebars.compile(source);
        var wrapper = { newOrder: order };
        var html = template(wrapper);
        content.append(html);
    }

    //remove order from table
    operatorHub.client.removeAwaitOrders = function (orderId) {
        $('#submitButton' + orderId).closest('tr').remove();
    }

    //driver can go, show modal form confirm Driver Request
    operatorHub.client.confirmDrRequest = function ()
    {
        $('.successDriverOrder').click();
    }


    //driver can't go, show modal form denied Driver Request
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


        //Send message to operators
        $('#showform').click(function () {
            swal({
                title: LocStrings.InputYourMessage,
                html: '<p><textarea id="input-field" style="width: 100%; height: 75px "> </textarea>',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
                driverHub.server.sendToOperators($('#input-field').val(), $('#txtUserName').val());
                swal(LocStrings.MessageHasBeenSent, '', 'success');
            });
        })
    });
});


//get orders from db
function GetOrders() {
    var content = $("#DrOrder");
    $.ajax({
        type: 'POST',
        url: "../Driver/GetDriverOrders/",
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
            url: "./Driver/GetCurrentOrder/",
            data: { orderId: currentOrderId },
            dataType: 'json',
            success: function (data) {

                $(".submitButton").attr("disabled", "disabled");

                data.WaitingTime = time;
                data.DriverId = driverUserId;
                data.DriverName = $('#txtUserName').val();

                isOrdered = true;

                //Send assigned order to operators
                driverHub.server.assignedOrder(data);
            }
        });
    }
});
