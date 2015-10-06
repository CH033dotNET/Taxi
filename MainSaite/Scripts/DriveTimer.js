
$(document).ready(function () {
    var countTimer = 1;
    var payroll = 0;
    var pointTarif = 1;
    var drievingTime;

    DefaultTarif();
    ShowDate();
    setInterval(ShowDate, 1000);

    //start drive
    var switcher = $(".btn").on("click", Switch);
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
            switcher.removeClass("Drive").val("Destination");
            drievingTime = setInterval(DriveTime, 1000);
        }
        else {
            switcher.addClass("Drive");
            switcher.val("Drive");
            clearInterval(drievingTime);
        }
    }

    //
    function DefaultTarif() {
        $(":checkbox:first").attr('checked', true);
    }
})