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
            alert(error + "error!!!!");
        }
    });
}



function Assign() {
    var time = $("#timetotravel").val();
    var currentOrderId = $("#orderid").attr('data-orderid');

        $.ajax({
            url: '/Driver/GetOrder/',
            data: { orderId : currentOrderId, waitingTime : time},
            success: function (data) {
                GetOrders();
            }
        });
}