var currentOrderId;

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


function getOrdedId(e) {
    currentOrderId = $(e).attr('data-orderid');
}

function Assign() {
    var time = $("#timetotravel").val();
    alert("Підтверджено, час до клієнтаа:" + time + " хв");

        $.ajax({
            url: '/Driver/GetOrder/',
            data: { orderId : currentOrderId},
            success: function (data) {
                GetOrders();
            }
        });
}