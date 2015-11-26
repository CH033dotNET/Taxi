$(document).ready(function () {
    getVipClients();
});

$(document).on("click", ".set", function () {
    ClientId = $(this).attr('data-idclient');
    $.ajax({
        url: '/Settings/SetClientVip/',
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
        url: '/Settings/DelClientVip/',
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
        url: "/Settings/GetVipClients/",
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

