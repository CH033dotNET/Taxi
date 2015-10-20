

$(document).ready(function () {

    var payroll = 0;
    var pointTarif = 1;
    var Toogle;
    
   
    DefaultTarif();
    ShowDate();
    setInterval(ShowDate, 1000);
  
    //start drive
    var switcher = $("#double").on("click", Switch);
    //CheckBox

    $(":checkbox").on("click", function () {
        $(":checkbox").prop('checked', false);
        $(this).prop('checked', true);
        pointTarif = ($(this).val());
        var tempor = ($(this).attr("temp"));
        if(switcher.hasClass("Drive"))
        {
            $("#clientPrice").text(tempor);
        }
    });

    // Local Time
    function ShowDate() {
        $(".timeTable").text(new Date().toDateString().toString() + " " + new Date().toLocaleTimeString().toString());
    }


    

    // Start or finish drive a client
    function Switch() {
        if (switcher.hasClass("Drive")) {
            switcher.removeClass("Drive").addClass("Dest").text("Destination").append("<i class='fa fa-tachometer fa-lg'></i>");
            Toogle = setInterval(StartService, 2000);

        }
        else {

            switcher.removeClass("Dest").addClass("Drive");
            switcher.text("Drive").append("<i class='fa fa-tachometer fa-lg'></i>");
            clearInterval(Toogle);
            EndService();
        }
    }


    function DefaultTarif() {
        $(":checkbox:first").attr('checked', true);
        
    }

    ///ajax object 
    function getBeginCoordinate(position) {
      
        var dataObj = {}
        dataObj.UserId = document.getElementById('Id').value;
        dataObj.Latitude = position.coords.latitude;
        dataObj.Longitude = position.coords.longitude;
        dataObj.Accuracy = position.coords.accuracy;
        dataObj.Tarifid = pointTarif;
        dataObj.AddedTime = new Date().toLocaleString();
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
        dataObj.Accuracy = position.coords.accuracy;
        dataObj.Tarifid = pointTarif;
        dataObj.AddedTime = new Date().toLocaleString();

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



})