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