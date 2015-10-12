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

    //Mask for input
    $('input[type=email]').inputmask();

    //Mask for phone
    $("#phone").inputmask("(999)-999-99-99");


    $("#email").inputmask({
        mask: "*{1,20}[.*{1,20}][.*{1,20}][.*{1,20}]@*{1,20}[.*{2,6}][.*{1,2}]",
        greedy: false,
        onBeforePaste: function (pastedValue, opts) {
            pastedValue = pastedValue.toLowerCase();
            return pastedValue.replace("mailto:", "");
        },
        definitions: {
            '*': {
                validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~\-]",
                cardinality: 1,
                casing: "lower"
            }
        }
    });
});