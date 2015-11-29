var currentOrderId;
var intervalID;
var isConfirmIntervalId;
var isOrdered = false;
var driverHub;
var operatorHub;

$(function () {
    GetOrders();

   //operatorHub = $.connection.OperatorHub;
	driverHub = $.connection.DriverHub;



   //Show message from operators
	driverHub.client.showMessage = function (data) {
        // Own function 
		swal('New message from operator!', data, 'success');

    };
   
    //Add new order to table
	driverHub.client.newDriverOrders = function (order)
    {
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
    $.connection.hub.start().done(function () {
        // LogIn
        var driverRoleId = 1;
        var driverUserId = $('#currentUserId').val();

        //driverHub.server.connectUser(driverUserId);
        //driverHub.server.connectUser(driverRoleId, driverUserId);
        //operatorHub.server.connectUser(driverRoleId, driverUserId);


        //Send message to operators
        $('#showform').click(function () {
            swal({
                title: 'Input Your message:',
                html: '<p><textarea id="input-field" style="width: 100%; height: 75px "> </textarea>',
                showCancelButton: true,
                closeOnConfirm: false
            }, function () {
            	$.ajax({
            		type: 'POST',
            		url: "/Orders/SendToOperators/",
            		data: { 
            			message: $('#input-field').val(),
            			username: $('#txtUserName').val() },
            		dataType: 'json',
            		success: function () { console.log('yyyeah'); },
            		error: function () { console.log('nooo'); }
            	});
              //  driverHub.server.sendToOperators($('#input-field').val(), $('#txtUserName').val()); //////!!!!!
                swal('Your message has been sent', '', 'success');
            });
        })
    });
});


//get orders from db
function GetOrders() {
    var content = $("#DrOrder");
    $.ajax({
        type: 'POST',
        url: "/Orders/GetDriverOrders/",
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
            url: "/Orders/GetCurrentOrder/",
            data: { orderId: currentOrderId },
            dataType: 'json',
            success: function (data) {

                $(".submitButton").attr("disabled", "disabled");

                data.WaitingTime = time;
                data.DriverId = driverUserId;
                data.DriverName = drName;
                isOrdered = true;

                $.ajax({
                	url: "/Orders/AssignCurrentOrder/",
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
