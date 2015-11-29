$(document).ready(function () {
    getVipClients();
});

$(document).on("click", ".set", function () {
    ClientId = $(this).attr('data-idclient');
    $.ajax({
        url: setVIPcl,
        data: {
            clientId: ClientId
        },

        success: function () {
                getVipClients();
        }
    });
})


$(document).on("click", ".del", function () {
    ClientId = $(this).attr('data-idclient');
    $.ajax({
        url: delVIPcl,
        data: {
            clientId: ClientId
        },

        success: function () {
            getVipClients();
        }
    });

})

function getVipClients() {
    $.ajax({
        type: 'POST',
        url: getVIPcl,
        dataType: 'json',
        success: function (data) {
            var content = $("#vip");
            var source = $("#vip-template").html();
            var template = Handlebars.compile(source);
            var wrapper = { clients: data };
            var html = template(wrapper);
            content.html(html);
        }
    });
};

