var itemId;
var itemName;
var districtTemplate = Handlebars.compile($("#dsEditRow").html());
var deletedDistrictsTemplate = Handlebars.compile($("#dsDeletedRow").html());

var sortDistrictCounter;
var sortDeletedDistrictCounter;

$(document).ready(function () {
	jsController.orderAllCoordinates(jsController.data.items);
});

var jsController = {
	data: { items: encodedData },
	deletedData: { deleted: encodedDeleted },

	// render non-deleted districts in template
	renderData: function () {
		$('#districtEditTable tbody').html(districtTemplate(this.data));
		//initialize start data
		jsController.data.items.forEach(function (item) {
			if (!item.Polygon) {
				var path = item.Coordinates.map(function (item) {
					return {
						lat: item.Latitude,
						lng: item.Longitude,
					}
				});

				item.Polygon = new google.maps.Polygon({
					paths: path,
					map: map,
				});
				item.Polygon.setOptions(polygonOptions);
				item.Marker = createMarkerForPolygon(item.Polygon, item.Name);
				item.Changed = false;
				setEventsForPolygon(item.Polygon);
			}
		});

	},
	//! function that is part of dinamic search. Only for searching.
	searchDistricts: function (e) {
		//var searchString = $('#editInputDistrictId').val();
		var searchString = e;
		$.ajax({
			url: "./SearchDistrict", // <----------------------------------------------------!!
			data: { parameter: searchString },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			jsController.clearMap();
			if (result.success && result != null) {
				jsController.data.items = result.resultDistricts;
				jsController.orderAllCoordinates(jsController.data.items);
				jsController.renderData();
			}
			else if (!result.success && result.districts != undefined) {
				jsController.data.items = result.districts;
				jsController.orderAllCoordinates(jsController.data.items);
				jsController.renderData();
			}
			else { return false; }
		});
	},
	//! function that is part of dinamic search of deleted districts. Only for searching.
	searchDeletedDistricts: function (e) {
		var searchDeletedString = e;
		$.ajax({
			url: "./SearchDeletedDistrict", // <----------------------------------------------------!!
			data: { parameter: searchDeletedString },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				jsController.deletedData.deleted = result.resultDistricts;
				jsController.orderAllCoordinates(jsController.deletedData.deleted);
				jsController.renderDeletedData();
			}
			else if (!result.success && result.districts != undefined) {
				jsController.deletedData.deleted = result.districts;
				jsController.renderDeletedData();
			}
			else { return false; }
		});
	},


	//! function that is part of sorting non-deleted entries logic. Affected by search input value.
	sortDistrictBy: function (e) {
		var searchDString = $('#searcDhistrictName').val();
		switch (sortDistrictCounter) {
			case "name":
				if (e == "name") {
					e = "name_desc";
					break;
				}
				else { break; }
			default:
				break;
		}
		sortDistrictCounter = e;
		$.ajax({
			url: "./SearchAndSort", // <----------------------------------------------------!!
			data: { search: searchDString, sort: e },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				jsController.clearMap();
				jsController.data.items = result.resultDistricts;
				jsController.orderAllCoordinates(jsController.data.items);
				jsController.renderData();
			}
			else { return false; }
		});
	},
	//! function that is part of sorting deleted entries logic. Affected by search input value.
	sortDeletedDistrictBy: function (e) {
		var searchDeletedDString = $('#searchDeletedDistrictName').val();
		switch (sortDeletedDistrictCounter) {
			case "name":
				if (e == "name") {
					e = "name_desc";
					break;
				}
				else { break; }
			default:
				break;
		}
		sortDeletedDistrictCounter = e;
		$.ajax({
			url: "./DeletedSearchAndSort", // <----------------------------------------------------!!
			data: { search: searchDeletedDString, sort: e },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				jsController.deletedData.deleted = result.sortedDeletedDistricts;
				jsController.renderDeletedData();
			}
			else { return false; }
		});
	},

	//render deleted districts in template
	renderDeletedData: function () {
		$('#districtDeletedTable tbody').html(deletedDistrictsTemplate(this.deletedData));
	},

	//! function that opens basket modal window
	DistrictBasket: function (e) {
		$.ajax({
			url: "./DeletedDistricts", // <----------------------------------------------------!!
		}).done(function (result) {
			jsController.deletedData.deleted = result.deletedDistricts;
			jsController.orderAllCoordinates(jsController.deletedData.deleted);
			jsController.renderDeletedData();
			$('#deletedList').modal('show');
		});
	},

	//! function that restore distirct form deleted state
	restoreDistrict: function (e) {
		var itemId = $(e).attr('data-items-id'); // get model item id from template
		var name = $(e).attr('data-items-name'); // get model item name from template

		$.ajax({
			url: "./RestoreDistrict/", // <----------------------------------------------------!!
			data: { id: itemId }
		}).done(function (result) {
			if (result.success && result != null) {
				var restored = jsController.deletedData.deleted.find(function (item) {
					return item.Id == itemId;
				});
				jsController.deleteDistrictFromItems(jsController.deletedData.deleted, restored);
				restored.Deleted = false;
				jsController.data.items.push(restored);
				jsController.renderDeletedData();
				jsController.renderData();
			}
			else { jsController.getDistrictErrorMessage(); }
		});
	},
	//! function for adding new districts. Opens modal window.
	addDistrict: function () {
		$('#add-item').modal('show');
	},



	//! function for adding new districts. Gets data and sends it on server via ajax.
	addDistrictConfirm: function () {
		var newDistrictName = $('#newDistrictName').val();
		//set map data
		var district = findDistrict(selectedDistrict);
		var coordinates = mapCoordinates(district.Polygon);
		district.Coordinates = coordinates;
		var data = {
			Name: newDistrictName,
			Deleted: false,
			Coordinates: this.setCoordinatesIndex(coordinates)
		};

		$.ajax({
			contentType: "application/json; charset=utf-8",
			type: "post",
			dataType: "json",
			url: "./AddDistrict", // <----------------------------------------------------!!
			data: JSON.stringify(data)
		}).done(function (result) {
			if (result.success && result != null) {
				district.Id = result.district.Id;
				district.Name = newDistrictName;
				district.Marker = createMarkerForPolygon(district.Polygon, newDistrictName);
				district.Coordinates = result.district.Coordinates;
				jsController.renderData();
				document.getElementById("add-district-form").reset();
			}
			else {
				jsController.data.items.splice(jsController.data.items.indexOf(district), 1);
				jsController.getDistrictErrorMessage();
				document.getElementById("add-district-form").reset();
			}
		})
		.always(function () {
			$("#add-item").modal('hide');
		});
	},
	//! function for deleting selected district.
	deleteItem: function (e) {
		itemId = $(e).attr('data-items-id');
		itemName = $(e).attr('data-items-name');
		$('.item-name').html(itemName);
		$('#confirm-delete').modal('show');
		var del = jsController.getDistrictById(itemId);

		$('#confirm-delete .btn-ok').off("click.deleteADistrict").on("click.deleteADistrict", function () {
			var request = $.ajax({
				url: "./DeleteDistrict/", // <----------------------------------------------------!!
				data: { Id: itemId, Name: itemName },
				method: "POST",
			}).done(function (result) {
				if (result.success && result != null) {
					del.Marker.setMap(null);
					del.Polygon.setMap(null);
					jsController.deleteDistrictFromItems(jsController.data.items, del);
					jsController.renderData();
				}
				else { jsController.getDistrictErrorMessage(); }
			});
			$('#confirm-delete').modal('hide');
			$('#confirm-delete .btn-ok').off("click.deleteADistrict");
			return false;
		});
	},
	//! function for editing selected district info
	editItem: function (e) {
		var itemId = $(e).attr('data-items-id'); // get model item id from template
		var name = $(e).attr('data-items-name'); // get model item name from template

		$('#newName').val(name); // set model item name to an input field

		$('#editInputDistrictId').val(itemId); /////addd id!!!

		$("#edit-item").modal('show'); // show modal

	},
	//! function for editing selected district info. Gathers all data and sends it on server via ajax.
	editConfirmDistrict: function () {
		var id = $('#editInputDistrictId').val();
		var name = $('#newName').val();
		this.editDistrict(id, name);
	},

	editDistrict: function (id, name) {
		var item = this.getDistrictById(id);
		$.ajax({
			url: "./EditDistrict/", // <----------------------------------------------------!!
			data: JSON.stringify({
				Id: id,
				Deleted: item.Deleted,
				Name: name,
				Coordinates: this.setCoordinatesIndex(item.Coordinates)
			}),
			contentType: "application/json; charset=utf-8",
			type: "post",
			dataType: "json",
		}).done(function (result) {
			if (result.success && result != null) {
				item.Name = name;
				item.Marker.labelContent = name;
				item.Marker.setMap(map);
				jsController.renderData();
				document.getElementById("edit-district-form").reset();
			}
			else {
				jsController.getDistrictErrorMessage();
				document.getElementById("edit-district-form").reset();
			}
		})
		.always(function () {
			$("#edit-item").modal('hide');
		});
	},
	//! function that opens modal window with error message.
	getDistrictErrorMessage: function () {
		$('#get-district-error-modal').modal('show');
	},

	//! set rigth indexes for chenched or added item
	setCoordinatesIndex: function (items) {
		items.forEach(function (item, index) {
			item.Index = index;
		});
		return items;
	},

	getDistrictById: function (id) {
		return this.data.items.find(function (item) {
			if (item.Id == id) return true;
			return false;
		});
	},

	//! set all coordinates for items in right order 
	orderAllCoordinates: function (items) {
		items.forEach(function (item) {
			var orderedCoords = [];
			for (var i = 0; i < item.Coordinates.length; i++) {
				orderedCoords.push(item.Coordinates.find(function (f) { return f.Index == i }));
			}
			item.Coordinates = orderedCoords;
		});

	},

	clearMap: function () {
		this.data.items.forEach(function (item) {
			item.Polygon.setMap(null);
			item.Marker.setMap(null);
		});
	},

	//delete item from array of items
	deleteDistrictFromItems: function (items, item) {
		items.splice(items.indexOf(item), 1);
	}
};


//
//Logic for map!!!!!!!!!!
//

//init map
google.maps.event.addDomListener(window, 'load', initMap);


var selectedDistrict;
//array for all regions on map
var figures = [];
var map;
var polygonOptions = {
	strokeWeight: 0,
	fillOpacity: 0.45,
	editable: false,
	strokeWeight: 2,
	fillColor: "#FD8E00",
	strokeColor: "#FD8E00"
};


function initMap() {

	//create new instance of map
	var mapDiv = document.getElementById('map');
	map = new google.maps.Map(mapDiv, {
		center: { lat: 48.2756705, lng: 25.8885469 },
		zoom: 12
	});

	//set drawing options
	var drawingManager = new google.maps.drawing.DrawingManager({
		//allow to edit figures
		drawingMode: null,
		drawingControl: true,
		//add control buttons
		drawingControlOptions: {
			position: google.maps.ControlPosition.TOP_CENTER,
			drawingModes: [
			  google.maps.drawing.OverlayType.POLYGON,
			]
		},
		//optins for drawing objects
		polygonOptions: polygonOptions,
	});
	drawingManager.setMap(map);

	//event on creating new polygon 
	google.maps.event.addListener(drawingManager, 'polygoncomplete', function (e) {
		jsController.addDistrict();
		drawingManager.setDrawingMode(null);

		var newShape = e;
		jsController.data.items.push({
			Deleted: false,
			Polygon: newShape,
			Name: '',
			Marker: null,
			Changed: false
		});
		setSelection(newShape);
		setEventsForPolygon(newShape);
	})

	//only one figure can be chosen
	google.maps.event.addListener(drawingManager, 'drawingmode_changed', clearSelection);
	google.maps.event.addListener(map, 'click', function () {
		clearSelection();
	});
	jsController.renderData();
}

//events for polygons
function setEventsForPolygon(shape) {
	google.maps.event.addListener(shape, 'click', function () {
		setSelection(shape);
	});
	shape.getPaths().forEach(function (item) {
		var district = findDistrict(shape);
		google.maps.event.addListener(item, 'insert_at', function (number) {
			var newCoord = item.getAt(number);
			district.Coordinates.splice(number, 0, {
				Latitude: newCoord.lat(),
				Longitude: newCoord.lng()
			});
			district.Edited = true;
		});
		google.maps.event.addListener(item, 'set_at', function (number, path) {
			var edit = district.Coordinates.find(function (i) {
				var lat = path.lat();
				var lng = path.lng();
				if (i.Latitude == path.lat() && i.Longitude == path.lng()) {
					return true;
				}
				return false;
			});
			var el = item.getAt(number);
			edit.Latitude = el.lat();
			edit.Longitude = el.lng();
			district.Edited = true;

		});
		google.maps.event.addListener(item, 'remove_at', function () {
			district.Edited = true;
		});
	});
}

//if don`t press add button
$('#cencelAddDistrict').click(function () {
	deleteselectedDistrict();
});

//map coordinates according to DB
function mapCoordinates(polygon) {
	return getAllLatLngs(polygon).map(function (item) {
		return {
			Latitude: item.lat(),
			Longitude: item.lng()
		}
	});
}


function createMarkerForPolygon(figure, name) {
	//create label for name
	var marker = new MarkerWithLabel({
		position: getPolygonCenter(figure),
		draggable: false,
		raiseOnDrag: false,
		map: map,
		labelContent: name,
		labelAnchor: new google.maps.Point(-20, 0),
		labelClass: "labels", // the CSS class for the label
		labelStyle: { opacity: 1.0 },
		icon: " ", //without icon
		visible: false,
	});
	//show name when cursor are in figure
	google.maps.event.addListener(figure, "mousemove", function (event) {
		marker.setPosition(event.latLng);
		marker.setVisible(true);
	});
	//hide name
	google.maps.event.addListener(figure, "mouseout", function (event) {
		marker.setVisible(false);
	});
	return marker;
}

function findDistrict(polygon) {
	return jsController.data.items.find(function (item) {
		if (item.Polygon == polygon) {
			return true;
		}
		else {
			return false;
		}
	});
}

function clearSelection() {
	if (selectedDistrict) {
		var edit = findDistrict(selectedDistrict);
		if (edit.Edited) {
			jsController.editDistrict(edit.Id, edit.Name);
		}
		selectedDistrict.setEditable(false);
		selectedDistrict = null;
	}
}

function setSelection(shape) {
	clearSelection();
	selectedDistrict = shape;
	shape.setEditable(true);
}

function getAllLatLngs(polygon) {
	var latLngs = [];
	var paths = polygon.getPaths();
	paths.forEach(function (item) {
		item.forEach(function (ll) {
			latLngs.push(ll);
		});
	});
	return latLngs;
}

function getPolygonCenter(poly) {
	var bounds = new google.maps.LatLngBounds();
	getAllLatLngs(poly).forEach(function (item) {
		bounds.extend(item);
	});

	return bounds.getCenter();
}
