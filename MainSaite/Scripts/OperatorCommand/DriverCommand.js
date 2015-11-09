var currentOrderId;
var intervalID;
var isConfirmIntervalId;
var isOrdered = false;

$(document).ready(function () {
    setInterval(function () {
        GetOrders();
    }, 2000)
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
        $.ajax({
            url: '/Driver/GetOrder/',
            data: { orderId : currentOrderId, waitingTime : time},
            success: function (data) {
            $(".submitButton").attr("disabled", "disabled");
            GetOrders();
            isOrdered = true;
            isConfirmIntervalId = setInterval(OrderConfirmStatus, 3000);
            }
        });
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
                    $('.deniedDriverOrder').click();
                    clearInterval(isConfirmIntervalId);
                    isOrdered = false;
                    break;
                case 4:
                    $('.successDriverOrder').click();
                    clearInterval(isConfirmIntervalId);
                    break;
                default: break;
            }
        }
    });

}