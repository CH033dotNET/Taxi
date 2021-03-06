﻿$(document).ready(function () {
	var mainHub = $.connection.MainHub;

	var districts = [];
	var geocoder = new google.maps.Geocoder();

	var newOrders = [];
	var approvedOrders = [];
	var deniedOrders = [];
	var inProgressOrders = [];
	var finishedOrders = [];

	var orderId;
	//Edit order window
	$(document).on("dblclick", ".order", function () {
		$('#myModal').modal('toggle');
		orderId = $(this).data("id");

		$("#EditOrderA").attr("href", "/OrderEx/GetOrderById/" + orderId);

		$('#EditOrderA').trigger('click');
	});

	$(document).on("click", ".paragraph", function () {

		var cls = $(this).children('span').attr('class');
		var p = $(this).parent().next('div').attr('id');

		if (cls.indexOf("down") > -1) {
			$('#'+p).slideDown(200);
			$(this).children('span').attr('class', 'glyphicon glyphicon-chevron-up');
		}
		if (cls.indexOf("up") > -1) {
			$('#'+p).slideUp(200);
			$(this).children('span').attr('class', 'glyphicon glyphicon-chevron-down');
		}

	});
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
			var order = newOrders.find(function (item) {
				return item.Id == id;
			});
			$.ajax({
				type: "POST",
				url: "/OrderEx/ApproveOrder/",
				data: { id: id },
				success: function (result) {
					if (result) {
						newOrders.splice(newOrders.indexOf(order), 1);
						approvedOrders.push(order);
						checkDistrict(order);
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
			var order = newOrders.find(function (item) {
				return item.Id == id;
			});
			var id = ui.item.attr('data-id');
			$.ajax({
				type: "POST",
				url: "/OrderEx/DenyOrder/",
				data: { id: id },
				success: function (result) {
					if (result) {
						newOrders.splice(newOrders.indexOf(order), 1);
						deniedOrders.push(order);
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
		$('#orderBoard').height($(this).height() -50);
		$('.content').height($(this).height() - 120);
	});


	$('.header').hover(function () {
		$(this).animate({
			color: 'white'
		},100);
		$(this).parent().css({
			'border': '2px solid rgba(255, 255, 255, 0.20)',
		});
	},
	function () {
		$(this).animate({
			color: 'rgba(255, 255, 255, 0.8)'
		}, 100);
		$(this).parent().css({
			'border': '2px solid transparent',
		});
	});

	//district logic

	$.ajax({
		method: 'POST',
		url: '/OrderEx/GetDistricts',
		success: function (result) {
			districts = result.districts;
			districts.forEach(function (item) {
				var path = item.Coordinates.map(function (item) {
					return {
						lat: item.Latitude,
						lng: item.Longitude
					}
				});

				item.Polygon = new google.maps.Polygon({
					paths: path
				});
			});
		}
	});


	function checkDistrict(order) {
		geocoder.geocode({ 'address': order.FullAddressFrom }, function (results, status) {
			if (status === google.maps.GeocoderStatus.OK) {
				var district = districts.find(function (item) {
					return google.maps.geometry.poly.containsLocation(results[0].geometry.location, item.Polygon)
				 });
				 if (!district) {
				 	mainHub.server.OrderApproved(order.Id);
				 }
				 else {
				 	mainHub.server.OrderApproved(order.Id, district.Id);
				 }
			}
		});
	}

	//functions for filling orders

	
		$.ajax({
			method: 'POST',
			url: '/OrderEx/GetOperatorOrders',
			success: function (data) {
				newOrders = data.NewOrders;
				approvedOrders = data.ApprovedOrders;
				deniedOrders = data.DeniedOrders;
				inProgressOrders = data.InProgressOrders;
				finishedOrders = data.FinishedOrders;
				addOrdersTo(newOrders, $('#newOrder'));
				addOrdersTo(approvedOrders, $('#approvedOrder'));
				addOrdersTo(deniedOrders, $('#deniedOrder'));
				addOrdersTo(inProgressOrders, $('#inProgressOrder'));
				addOrdersTo(finishedOrders, $('#doneOrder'));
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
		$(address).append(order.FullAddressFrom);
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
	mainHub.client.orderUpdated = function (id) {
		$.ajax({
			method: 'POST',
			url: '/OrderEx/GetOrderAddressByID',
			data: { id: id },
			success: function (data) {
				$('#newOrder .order').filter('[data-id=' + id + ']').children('span').text(data.address);
			}
		});
	}
	mainHub.client.finishOrder = function (id) {
		
		var order = inProgressOrders.find(function (item) {
			return item.Id == id;
		});
		inProgressOrders.splice(inProgressOrders.indexOf(order), 1);
		finishedOrders.push(order);
		var orderElement = $('[data-id = "' + id + '"]');
		$('#doneOrder').append(orderElement);


	}
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

	mainHub.client.cancelOrder = function(id){
		var order = newOrders.find(function (item) {
			return item.Id == id;
		});
		if (order) {
			newOrders.splice(newOrders.indexOf(order), 1);
		}
		else {
			var order = approvedOrders.find(function (item) {
				return item.Id == id;
			});
			if (order) {
				approvedOrders.splice(approvedOrders.indexOf(order), 1);
			}
			else {
				var order = inProgressOrders.find(function (item) {
					return item.Id == id;
				});
				if (order) {
					inProgressOrders.splice(inProgressOrders.indexOf(order), 1);
				}
			}
		}

		$('[data-id = "' + id + '"]').remove();
		
	}

	$.connection.hub.start().done(function () {

		//connect to hub group
		mainHub.server.connect("Operator");

	});

	window.updateOrderInfo = function () {
		$('#myModal').modal('toggle');
		mainHub.server.updateOrder(orderId);
	}
});
