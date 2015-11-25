$(document).ready(function () {

    // Validate for image
    var filesExt = ['jpg', 'gif', 'png'];
    $('input[type=file]').change(function () {
        var parts = $(this).val().split('.');
        if (filesExt.join().search(parts[parts.length - 1]) != -1) {
            $('input[type=submit]').prop("disabled", false);
            $('.error').hide();

        } else {
            $('input[type=submit]').prop("disabled", true);
            $("#error").show();
        }

    }
    );

    //Style for input
    $(":file").filestyle({ input: false, 'buttonText': "Upload image", 'badge': false});

    //Mask for phone
   // $("#phone").mask("(999)-999-99-99");

});