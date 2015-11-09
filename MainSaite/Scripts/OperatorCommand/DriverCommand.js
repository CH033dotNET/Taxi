var currentOrderId;
var intervalID;

$(document).ready(function () {
    setInterval(function () { GetOrders();},3000)
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
        },
        error: function (error) {
            alert("error!!!!"+error);
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
            GetOrders();
            }
        });
}