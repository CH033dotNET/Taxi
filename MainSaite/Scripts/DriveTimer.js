
$(document).ready(function () {
    var countTimer = 1;
    var payroll = 0;
    var pointTarif = 1;
    var drievingTime;

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
    });

    // Local Time
    function ShowDate() {
        $(".timeTable").text(new Date().toDateString().toString() + " " + new Date().toLocaleTimeString().toString());
    }


    function DriveTime() {
        payroll += countTimer * pointTarif;
        $(".driveCounter").text(payroll);
    }

    // Start or finish drive a client
    function Switch() {
        if (switcher.hasClass("Drive")) {
            switcher.removeClass("Drive").addClass("Dest").text("Destination").append("<i class='fa fa-tachometer fa-lg'></i>");
            drievingTime = setInterval(DriveTime, 1000);
        }
        else {
            switcher.removeClass("Dest").addClass("Drive");
            switcher.text("Drive").append("<i class='fa fa-tachometer fa-lg'></i>");
            clearInterval(drievingTime); 
        }
    }

    //
    function DefaultTarif() {
        $(":checkbox:first").attr('checked', true);
    }
})