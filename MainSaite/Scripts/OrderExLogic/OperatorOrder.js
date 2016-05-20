$(document).ready(function () {
	var mainHub = $.connection.MainHub;

	var newOrders = [];
	var approvedOrders = [];
	var deniedOrders = [];
	var inProgressOrders = [];

	//init jqueryUI
	$('#newOrder').sortable({
		connectWith: '#approvedOrder, #deniedOrder',
		placeholder: 'placeholder',
		start: function (e, ui) {
			ui.placeholder.height(ui.item.height() + 17);
			ui.placeholder.width(ui.item.width() + 17);
		}
	});

	$('#approvedOrder').sortable({
		placeholder: 'placeholder',
		start: function (e, ui) {
			ui.placeholder.height(ui.item.height() + 17);
			ui.placeholder.width(ui.item.width() + 17);
		},
		receive: function (e, ui) {
			var id = ui.item.attr('data-id');
			$.ajax({
				type: "POST",
				url: "/OrderEx/ApproveOrder/",
				data: { id: id },
				success: function (result) {
					if (result) {
						mainHub.server.OrderApproved(id);
					}
					else {
						alert("something wrong");
					}
				}
			});
		}
	});

	$('#deniedOrder').sortable({
		placeholder: 'placeholder',
		start: function (e, ui) {
			ui.placeholder.height(ui.item.height() + 17);
			ui.placeholder.width(ui.item.width() + 17);
		},
		receive: function (e, ui) {
			var id = ui.item.attr('data-id');
			$.ajax({
				type: "POST",
				url: "/OrderEx/DenyOrder/",
				data: { id: id },
				success: function (result) {
					if (result) {
						mainHub.server.denyOrder(id);
					}
					else {
						alert("something wrong");
					}
				}
			});
		}
	});

	$('#orderBoard').sortable({
		handle: '.header',
		items : '.col',
		axis: 'x',
		placeholder: 'hor-placeholder',
		start: function (e, ui) {
			ui.placeholder.height(ui.item.height() + 17);
			ui.placeholder.width(ui.item.width() + 17);
		}
	});


	//interface functions
	$(window).on('load resize', function () {
		$('#orderBoard').height($(this).height() - 200);
		$('.content').height($(this).height() - 270);
	});


	$('.header').hover(function () {
		$(this).animate({
			color: 'black'
		},100);
		$(this).parent().css({
			'border': '1px solid rgba(0, 0, 0, 0.20)',
		});
	},
	function () {
		$(this).animate({
			color: 'rgba(0, 0, 0, 0.5)'
		}, 100);
		$(this).parent().css({
			'border': '1px solid transparent',
		});
	});

	//functions for feeling orders

	$.ajax({
		method: 'POST',
		url: '/OrderEx/GetOperatorOrders',
		success: function(data){
			newOrders = data.NewOrders;
			approvedOrders = data.ApprovedOrders;
			deniedOrders = data.DeniedOrders;
			inProgressOrders = data.InProgressOrders;
			addOrdersTo(newOrders, $('#newOrder'));
			addOrdersTo(approvedOrders, $('#approvedOrder'));
			addOrdersTo(deniedOrders, $('#deniedOrder'));
			addOrdersTo(inProgressOrders, $('#inProgressOrder'));
		}
	});

	function addOrdersTo(orders, elementTo) {
		orders.forEach(function (item) {
			createOrderElement(elementTo, item);
		});
	}

	function createOrderElement(element, order) {
		var orderElement = document.createElement('div');
		$(orderElement).addClass('order');
		$(orderElement).attr('data-id', order.Id);
		var address = document.createElement('span');
		$(address).addClass('orderAddress');
		$(address).append(order.Address)
		$(orderElement).append(address);
		$(element).append(orderElement);
	}

	function checkCount(element, count) {
		if (element.length == count) {
			element
		}
	}

	//client hub functions
	mainHub.client.addOrder = function (order) {
		newOrders.push(order);
		createOrderElement($('#newOrder'), order);
	};

	mainHub.client.approveOrder = function (id) {
		var order = newOrders.find(function (item) {
			return item.Id == id;
		});
		newOrders.splice(newOrders.indexOf(order), 1);
		approvedOrders.push(order);
		var orderElement = $('[data-id = "' + id + '"]');
		$('#approvedOrder').append(orderElement);
	};

	mainHub.client.denyOrder = function (id) {
		var order = newOrders.find(function (item) {
			return item.Id == id;
		});
		newOrders.splice(newOrders.indexOf(order), 1);
		deniedOrders.unshift(order);
		if (deniedOrders.length > 50) {
			var id = deniedOrders.pop().Id;
			$('deniedOrder').remove($('[data-id = "' + id + '"]'));
		}
		var orderElement = $('[data-id = "' + id + '"]');
		$('#deniedOrder').prepend(orderElement);
	};

	mainHub.client.confirmOrder = function (id) {
		var order = approvedOrders.find(function (item) {
			return item.Id == id;
		});
		approvedOrders.splice(newOrders.indexOf(order), 1);
		inProgressOrders.push(order);
		var orderElement = $('[data-id = "' + id + '"]');
		$('#inProgressOrder').append(orderElement);
	};


	$.connection.hub.start().done(function () {

		//connect to hub group
		mainHub.server.connect("Operator");

	});
});