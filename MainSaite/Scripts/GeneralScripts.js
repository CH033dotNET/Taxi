var intervalId;

function ShowSupportChat() {
	$.ajax({
		type: "POST",
		url: "/Support/GetSupporter"
	}).done(function (support) {
		$('#chat .name').html(support.Name);
		$('#chat .avatar').attr('src', '/Images/' + support.Photo);
		$('#chat').data('supportId', support.Id);

		$('#chat').show(500);
		$('#chat').draggable();
		intervalId = setInterval(DisplayMessages, 2000);
	});
	
}

function HideSupportChat() {
	$('#chat').hide(500);
	clearInterval(intervalId);
}

function DisplayMessages() {
	$.ajax({
		type: "POST",
		url: "/Support/GetMessages",
		data : {
			id: $('#chat').data('supportId')
		}
	}).done(function (messages) {
		$('#chat .body').html('');

		for (var i in messages) {
			// Append message to end
			if (messages[i].Sender.Id != $('#chat').data('supportId')) {
				var message = '<div class="question message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			} else {
				var message = '<div class="answer message">' + messages[i].Message + '</div>';
				$('#chat .body').append(message);
			}
		}
	});
}

function SendMessage() {
	$.ajax({
		type: "POST",
		url: "/Support/SendMessage",
		data: {
			message: $('#chat .bottom .message').val(),
			toUserID: $('#chat').data('supportId')
		}
	}).done(function () {
		$('#chat .bottom .message').val('');
	});
}


//Mobile buttons

$(document).ready(function () {

	var url = window.location.pathname;
	var menuTabs = $("#mainMenu").children().length;

	if (menuTabs > 0) {
		if ($(window).width() <= 768) {
			$("#dropdownMenuBtn").css("display", "block");
		}

		$('#mobileBtnsList a[href="/Account/Authentification"] img').attr("src", "/Content/Picture/SmartPhonePictures/userLogOutLogo.png");

		$('#mobileBtnsList a[href="/Account/Authentification"]').attr("href", "/Account/LogOut");

	}

	$(document).on('click', '#showChat', function () {
		ShowSupportChat();
	});

	$("#passwordInput").val("password");

	if ($(window).width() <= 768) {
		$('ul.nav a[href="' + url + '"]').parent().addClass('active');

		$('#mobileBtnsList a[href="' + url + '"]').addClass('active');
	}

});

//////////////////////////////
/// Tariffs page - START
//////////////////////////////

function CreateNewTariff() {
	$('#exampleModalLabel').html('New tariff');

	$('#Id').val(-1);
	$('#Name').val('');
	$('#Description').val('');
	$('#PriceInCity').val('');
	$('#PriceOutCity').val('');
	$('#PricePreOrder').val('');
	$('#PriceRegularCar').val('');
	$('#PriceMinivanCar').val('');
	$('#PriceLuxCar').val('');
	$('#PriceCourierOption').val('');
	$('#PricePlateOption').val('');
	$('#PriceClientCarOption').val('');
	$('#PriceSpeakEnglishOption').val('');
	$('#PricePassengerSmokerOption').val('');
}

function GetTariffData(id) {
	$.ajax({
		type: "POST",
		url: "/TariffEx/GetTariffData",
		data: {
			id: id
		}
	}).done(function (tariff) {
		$('#Id').val(tariff.Id);
		$('#Name').val(tariff.Name);
		$('#Description').val(tariff.Description);
		$('#PriceInCity').val(tariff.PriceInCity);
		$('#PriceOutCity').val(tariff.PriceOutCity);
		$('#PricePreOrder').val(tariff.PricePreOrder);
		$('#PriceRegularCar').val(tariff.PriceRegularCar);
		$('#PriceMinivanCar').val(tariff.PriceMinivanCar);
		$('#PriceLuxCar').val(tariff.PriceLuxCar);
		$('#PriceCourierOption').val(tariff.PriceCourierOption);
		$('#PricePlateOption').val(tariff.PricePlateOption);
		$('#PriceClientCarOption').val(tariff.PriceClientCarOption);
		$('#PriceSpeakEnglishOption').val(tariff.PriceSpeakEnglishOption);
		$('#PricePassengerSmokerOption').val(tariff.PricePassengerSmokerOption);

		$('#exampleModalLabel').html(tariff.Name);
	});
}

function SaveTariff() {
	$.ajax({
		type: "POST",
		url: "/TariffEx/SaveTariff",
		data: {
			Id: $('#Id').val(),
			Name: $('#Name').val(),
			Description: $('#Description').val(),
			PriceInCity: $('#PriceInCity').val(),
			PriceOutCity: $('#PriceOutCity').val(),
			PricePreOrder: $('#PricePreOrder').val(),
			PriceRegularCar: $('#PriceRegularCar').val(),
			PriceMinivanCar: $('#PriceMinivanCar').val(),
			PriceLuxCar: $('#PriceLuxCar').val(),
			PriceCourierOption: $('#PriceCourierOption').val(),
			PricePlateOption: $('#PricePlateOption').val(),
			PriceClientCarOption: $('#PriceClientCarOption').val(),
			PriceSpeakEnglishOption: $('#PriceSpeakEnglishOption').val(),
			PricePassengerSmokerOption: $('#PricePassengerSmokerOption').val()
		}
	}).done(function () {
		location.reload();
	});
}

function DeleteTariff(id) {
	$.ajax({
		type: "POST",
		url: "/TariffEx/DeleteTariff",
		data: {
			id: id
		}
	}).done(function () {
		location.reload();
	});
}

//////////////////////////////
/// Tariffs page - END
//////////////////////////////


//////////////////////////////
/// News page - START
//////////////////////////////

function DeleteArticle(id) {
	$.ajax({
		type: "POST",
		url: "/News/DeleteArticle",
		data: {
			id: id
		}
	}).done(function () {
		location.reload();
	});
}

function SaveArticle(id) {
	$.ajax({
		type: "POST",
		url: "/News/SaveArticle",
		data: {
			id: id,
			title: $('#Title').val(),
			article: editor.getHTML()
		}
	}).done(function () {
		location.href = "/News/Index/";
	});
}

//////////////////////////////
/// News page - END
//////////////////////////////