
var switcher;
var pointTarif;
var Toogle;

$(document).ready(function () {

    var payroll = 0;
    pointTarif = 1;

    switcher = $("#double").on("click", Switch);

    ButtonStatus();
    DefaultTarif();
    ShowDate();
    setInterval(ShowDate, 1000);
    //start drive

    //CheckBox

    $(":checkbox").on("click", function () {
        $(":checkbox").prop('checked', false);
        $(this).prop('checked', true);
        pointTarif = ($(this).val());
        var tempor = ($(this).attr("temp"));
        if (switcher.hasClass("Drive")) {
            $("#clientPrice").text(tempor);
        }
    });
});

// Local Time
function ShowDate() {
    $(".timeTable").text(new Date().toDateString().toString() + " " + new Date().toLocaleTimeString().toString());
}

function DefaultTarif() {
    $(":checkbox:first").attr('checked', true);

}

// Start or finish drive a client
function Switch() {
    if (switcher.hasClass("Drive")) {
        switcher.removeClass("Drive").addClass("Dest").text("Destination").append("<i class='fa fa-tachometer fa-lg'></i>");
        var dataObj = {}
        dataObj.UserId = document.getElementById('Id').value
        dataObj.Tarifid = pointTarif;
        dataObj.AddedTime = new Date().toISOString();
        $.ajax({
            url: "/ClientService/StartTrip",
            method: "POST",
            data: dataObj,
            success: function (result) { }
        });
        Toogle = setInterval(StartService, 2000);

    }
    else {

        switcher.removeClass("Dest").addClass("Drive");
        switcher.text("Drive").append("<i class='fa fa-tachometer fa-lg'></i>");
        clearInterval(Toogle);
        EndService();
    }
}

function ButtonStatus() {
    $.ajax({
        url: "/ClientService/IsInTheWay",
        method: "GET",
        success: function (result) {
            if (result === "True") {
                if (switcher.hasClass("Drive")) {
                    switcher.removeClass("Drive").addClass("Dest").text("Destination").append("<i class='fa fa-tachometer fa-lg'></i>");
                    Toogle = setInterval(StartService, 2000);
                }
            }
            else {
                if (switcher.hasClass("Dest")) {
                    switcher.removeClass("Dest").addClass("Drive");
                    switcher.text("Drive").append("<i class='fa fa-tachometer fa-lg'></i>");
                    clearInterval(Toogle);
                    EndService();
                }
            }
        }
    });
}

///ajax object 
function getBeginCoordinate(position) {

    var dataObj = {}
    dataObj.UserId = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.Tarifid = pointTarif;
    dataObj.AddedTime = new Date().toISOString();
    $.ajax({
        url: "/ClientService/DrivingClient",
        method: "POST",
        data: dataObj,
        success: function (success) {
            $("#clientPrice").html(success);
        },
        error: function (e) { }
    });
}

function getEndCoordinate(position) {

    var dataObj = {}
    dataObj.UserId = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.Tarifid = pointTarif;
    dataObj.AddedTime = new Date().toISOString();

    $.ajax({
        url: "/ClientService/DropClient",
        method: "POST",
        data: dataObj,
        beforeSend: function () {
            var answer = confirm("Are you done?");
            if (answer) {
                //  Switch();
                return true;
            }
            return false;
        },
        success: function (success) {
            $("#clientPrice").html(success);
        },

        error: function (e) { }
    })
}

function StartService() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(getBeginCoordinate);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}

function EndService() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(getEndCoordinate);
    }
    else {
        alert("Geolocation is not supported by this browser.");
    }
}