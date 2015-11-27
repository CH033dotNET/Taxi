var UserAddress =
    {
        AddressId: '',
        City: '',
        Street: '',
        Number: '',
        Comment: ''
    };


$(document).ready(function () {
    getAddresses();
});


$(document).on("click", ".edit", function () {
    UserAddress.AddressId = $(this).attr('data-idaddress');

    $('#myModal').modal('show');

    if (UserAddress.AddressId !== -1)
    {
        UserAddress.City = $('#city'+UserAddress.AddressId).text();
        UserAddress.Street = $('#street' + UserAddress.AddressId).text();
        UserAddress.Number = $('#number' + UserAddress.AddressId).text();
        UserAddress.Comment = $('#comment' + UserAddress.AddressId).text();
    }

    else {
        UserAddress.City = '';
        UserAddress.Street = '';
        UserAddress.Number = '';
        UserAddress.Comment = '';
    }

    $('#newCity').val(UserAddress.City);
    $('#newStreet').val(UserAddress.Street);
    $('#newNumber').val(UserAddress.Number);
    $('#newComment').val(UserAddress.Comment);
})



 
$(document).on("click", ".addupd", function () {

    UserAddress.City = $('#newCity').val();
    UserAddress.Street = $('#newStreet').val();
    UserAddress.Number = $('#newNumber').val();
    UserAddress.Comment = $('#newComment').val();

    if (UserAddress.AddressId != -1)
    {
        $.ajax({
            url: '/Address/UpdAddress/',
            data: UserAddress,
            success: function () {
                getAddresses();
            }
        });
    }
    else {
        UserAddress.AddressId = null;
        $.ajax({
            url: '/Address/AddAddress/',
            data: UserAddress,
            success: function () {
                getAddresses();
            }
        });
    }

    $('#myModal').modal('toggle');

});


$(document).on("click", ".del", function () {
    AddressId = $(this).attr('data-idaddress');
    swal({
        title: LocStrings.AreYouSure, text: "",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: LocStrings.Yes,
        cancelButtonText: LocStrings.No,
        closeOnConfirm: false
    }, function () {

        $.ajax({
            url: '/Address/DelAddress/',
                data: {
                    addressId: AddressId
                },
        
                success: function () {
                                         getAddresses();
                                         swal(LocStrings.YourAddressHasBeenDeleted,"", "success");
                                      }
                 });
            });
})

function getAddresses() {
    $.ajax({
        type: 'POST',
        url: "/Address/GetFavoriteAddresses/",
        dataType: 'json',
        success: function (data) {
            var content = $("#favorite");
            var source = $("#favorite-template").html();
            var template = Handlebars.compile(source);
            var wrapper = { addresses: data };
            var html = template(wrapper);
            content.html(html);
        }
    });
};

