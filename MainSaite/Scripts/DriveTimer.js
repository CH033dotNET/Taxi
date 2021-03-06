﻿
var switcher;
var pointTarif;
var Toogle;
var CoordinatesJs = [];
var Trips = [];
var startPrice = 0;
var ToogleConn = null;
var conExist = true;


$(document).ready(function () {

    var payroll = 0;
    pointTarif = 1;

    switcher = $("#double").on("click", Switch);

    //  ButtonStatus();
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

function CollStartTrip() {
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
}

// Start or finish drive a client
function Switch() {
    if (switcher.hasClass("Drive")) {
        switcher.removeClass("Drive").addClass("Dest").text("Destination").append("<i class='fa fa-tachometer fa-lg'></i>");

        CollStartTrip();
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
function getBeginCoordinate1(position) {

    var dataObj = {}
    dataObj.UserId = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.Tarifid = pointTarif;
    dataObj.AddedTime = new Date().toISOString();


    CoordinatesJs.push(dataObj);

    PriceCounter.PriceCounter(CoordinatesJs, tarifes);
    if (startPrice == 0)
        startPrice = getStartPriceOfCurrentTarif();
    $('#clientPrice').html((startPrice + PriceCounter.CalcPrice()).toFixed('2'));
}

var getStartPriceOfCurrentTarif = function(){
    var id = $(':checkbox:checked').val();
    var t = tarifes.filter(function(e){
        if(e.id == id)
            return e;
    });

    return t[0].StartPrice;
}

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
    CoordinatesJs.push(dataObj);
    $.ajax({
        url: "/ClientService/DrivingClient",
        method: "POST",
        data: dataObj,
        success: function (success) {
            $("#clientPrice").html(success);
        },
        error: function (e) {
            conExist = false;
        }
    });

}

function getEndCoordinate1(position) {

    var dataObj = {}
    dataObj.UserId = document.getElementById('Id').value;
    dataObj.Latitude = position.coords.latitude;
    dataObj.Longitude = position.coords.longitude;
    //dataObj.Latitude = document.getElementById('hlat').value;
    //dataObj.Longitude = document.getElementById('hlong').value;
    dataObj.Accuracy = position.coords.accuracy;
    dataObj.Tarifid = pointTarif;
    dataObj.AddedTime = new Date().toISOString();
    CoordinatesJs.push(dataObj);

    PriceCounter.PriceCounter(CoordinatesJs, tarifes);
    var price = (startPrice + PriceCounter.CalcPrice()).toFixed('2');
    var min_price = getTarifById(CoordinatesJs[0].Tarifid).MinimalPrice;

    if (price < min_price)
        $("#clientPrice").html(min_price.toFixed('2'));
    else
        $("#clientPrice").html(price);
    Trips.push(CoordinatesJs);
    CoordinatesJs = [];
    startPrice = 0;
    if (ToogleConn == null)
        ToogleConn = setInterval(CheckConnection,5000);
}

var getTarifById = function(id){
    return tarifes.filter(function (e) {
        return e.id == id;
    })[0];
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
        if (!conExist)
            navigator.geolocation.getCurrentPosition(getBeginCoordinate1);
        else
            navigator.geolocation.getCurrentPosition(getBeginCoordinate);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}

function EndService() {
    if (navigator.geolocation) {
        if (conExist)
            navigator.geolocation.getCurrentPosition(getEndCoordinate);
        else
            navigator.geolocation.getCurrentPosition(getEndCoordinate1);
    }
    else {
        alert("Geolocation is not supported by this browser.");
    }
}


//-------------------------------------------------

var PriceCounter = {
    statics: {
        WAITINGCOSTSPEED: 5,
        EARTHRADIUD: 6371,
        ONEDEGREELATITUDE: 111
    },
    currentTarif: null,
    currentTatifId: 0,
    distance: 0,
    timePeriod: 0,
    speed: 0,
    currentPrice: 0,
    preLongDist: 0,
    coordinatesHistory: [],
    tarifes: [],
    PriceCounter: function (coordinatesHistory, tarifes) {
        this.coordinatesHistory = coordinatesHistory;
        this.tarifes = tarifes;
        if (coordinatesHistory.Count > 0) {
            // var item = tarifes.FirstOrDefault(tarif => tarif.id == coordinatesHistory[0].TarifId);
            var item = tarifes.filter(function (e) {
                return e.id == coordinatesHistory[0].TarifId;
            })[0];

            if (item !== undefined) {
                this.currentPrice = item.StartPrice;
            }
        }
    },

    CountOfMinutes: function (start, end) {

        return (new Date(end).getTime() - new Date(start).getTime()) / 60000;
    },

    GetDistance: function (PreLatitude, PreLongitude, CurLatitude, CurLongitude) {
        //the number of kilometres in one degree of Longitude in dependence of Latitude
        this.preLongDist = this.statics.EARTHRADIUD * (Math.PI / 180) * Math.cos(PreLatitude * Math.PI / 180);

        return Math.sqrt((Math.pow(this.statics.ONEDEGREELATITUDE * Math.abs(PreLatitude - CurLatitude), 2)) +
                         (Math.pow(this.preLongDist * Math.abs(PreLongitude - CurLongitude), 2)));
    },

    CalcPrice: function () {
        PriceCounter.currentPrice = 0;
        if (this.coordinatesHistory.length > 1) {
            var iterator = 0;
            var prevCoordinates;

            for (var i = 0; i < this.coordinatesHistory.length; i++) {
                if (iterator == 0) {
                    iterator++;
                    continue;
                }
                prevCoordinates = this.coordinatesHistory[iterator - 1];
                iterator++;

                this.distance = this.GetDistance(prevCoordinates.Latitude, prevCoordinates.Longitude,
                                               this.coordinatesHistory[i].Latitude, this.coordinatesHistory[i].Longitude);
                //timePeriod (min)

                this.timePeriod = this.CountOfMinutes(prevCoordinates.AddedTime, this.coordinatesHistory[i].AddedTime);
                this.speed = this.distance / (this.timePeriod / 60); // km/h

                if (PriceCounter.currentTatifId != PriceCounter.coordinatesHistory[i].Tarifid) {
                    PriceCounter.tarifes.forEach(function (e,index,array) {
                        if( e.id == PriceCounter.coordinatesHistory[i].Tarifid){
                            PriceCounter.currentTarif = e;
                            PriceCounter.currentTatifId = PriceCounter.coordinatesHistory[i].Tarifid;
                            return false;
                        }
                    });
                }

                if (this.speed > this.statics.WAITINGCOSTSPEED) {
                    this.currentPrice += this.timePeriod * this.currentTarif.OneMinuteCost;
                }
                else {
                    var a = PriceCounter.timePeriod * PriceCounter.currentTarif.WaitingCost;
                    PriceCounter.currentPrice += a;
                }

            }
            return PriceCounter.currentPrice;
        }
        else
        {
            return PriceCounter.currentPrice;
        }

    }
}

//------------------------
CheckConnection = function(){
    console.log("Check connection is active");
    console.log($("#clientPrice").text());
    $.ajax({
        url: "/ClientService/CheckConnection",
        method: "POST",
        success: function (success) {
            console.log("We got connection");
            console.log(success.data);
            clearInterval(ToogleConn);

            $.ajax({
                url: "/ClientService/OrderResult",
                method: "POST",
                traditional: true,
                data: JSON.stringify({'Trips':Trips, 'Price': $("#clientPrice").text() }),
                dataType: 'json',
                contentType: 'application/json',
                success: function(){
                    Trips = [];
                    conExist = 1;
                },
                error: function(){
                    setInterval(CheckConnection, 5000);
                }
            });

        },
        error: function (e) {
            console.log("Steel not connection");
        }
    })
}
