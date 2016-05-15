$(document).ready(function () {

	var districtTemplate = Handlebars.compile($("#dsEditRow").html());
	var deletedDistrictsTemplate = Handlebars.compile($("#dsDeletedRow").html());


	//variables
	var data = encodedData;
	var deletedData = encodedDeleted;

	var sortDistrictType;
	var sortDeletedDistrictType;

	//Jquery UI and nested sortable

	$('#content').nestedSortable({
		handle: '.districtItem',
		items: 'li',
		doNotClear: true,
		placeholder: "ui-state-highlight",
		stop: function (event, ui) {
			var parent = ui.item.parent();
			if (!parent.prev().hasClass('folder') && !$(parent).id == 'content') {
				$('#content').nestedSortable('cancel');
			}
			else {
				parentId = parent.parent().attr('data-id');
				var district = getDistrictById(ui.item.attr('data-id'));
				if (parentId) {
					district.ParentId = parentId;
				}
				else {
					district.ParentId = null;
				}
				setParent(district);
			}
		}
	});

	//events for group
	$(document).on('click', '.folder', function (e) {
		$(this).siblings('ol').fadeToggle('fast');
		var icon = $(this).find('.glyphicon').first();
		if (icon.hasClass('glyphicon-folder-close')) {
			icon.switchClass('glyphicon-folder-close', 'glyphicon-folder-open', 1000);
		}
		else {
			icon.switchClass('glyphicon-folder-open', 'glyphicon-folder-close', 1000);
		}
		e.stopPropagation();
	});

	//events
	$(document).on('click', '#basket', showDeletedData);

	$(document).on('input', '#searcDhistrictName', function (e) {

		searchDistricts($(this).val());
	});

	$(document).on('click', '#carlink', function (e) {
		e.preventDefault();
		sortDistrictBy('name');
	});

	$(document).on('click', '#edit-button', function (e) {
		editItem($(this));
		e.stopPropagation();
	});

	$(document).on('click', '#delete-button', function (e) {
		deleteDistrict(this);
		e.stopPropagation();
	});

	$(document).on('click', '#show-button', function (e) {
		var result = getAllChildren($(this).attr('data-items-id'));
		clearSelection();
		scrollTo(0, $('#map').position().top);
		result.forEach(function (item) {
			if (item.Polygon) {
				item.Polygon.setEditable(true);
			}
		});
		$('#action-buttons').removeClass('open');
		e.stopPropagation();
	});

	$(document).on('input', '#searchDeletedDistrictName', function (e) {
		searchDeletedDistricts($(this).val());
	});

	$(document).on('click', '#sort-deleted-district', function () {
		e.preventDefault();
		sortDeletedDistrictBy('name');
	});

	$(document).on('click', '#restore-district', function () {
		restoreDistrict(this);
	});

	$(document).on('click', '#confirm-add', function () {
		validateAddDistrict();
	});

	$(document).on('click', '#edit-district', function () {
		validateEditDistrict();
	});

	$(document).on('click', '#add-group', function () {
		$('#add-group-modal').modal('show');
	});

	$(document).on('click', '#addGropConfirm', function () {
		addGroup();
	});

	// render non-deleted districts in template
	function renderData() {
		var render = districtTemplate({ data: data });
		$('#content').html(render);
		var elements = $('#content> li');
		//intial sorting (folders first)
		elements.sort(function (a, b) {
			if ($(a).children('.districtItem').hasClass('folder') ^ $(b).children('.districtItem').hasClass('folder')) {
				if ($(a).children('.districtItem').hasClass('folder')) {
					return -1;
				}
				else {
					return 1;
				}
			}
			else {
				var aName = $(a).find('a').first().text().toLowerCase();
				var bName = $(b).find('a').first().text().toLowerCase();
				return aName.localeCompare(bName);
			}
		}).each(function () {
			$('#content').append(this);
		});
		//set element according to the hierarchy
		elements.each(function (index, el) {
			var parentId = $(el).attr('parent-id');
			if (parentId) {
				var newElement = document.createElement('ol');
				$(newElement).append(el);
				var find = $("li[data-id=" + "'" + parentId + "']");
				find.append(newElement);
			}
		});

		//initialize start data
		data.forEach(function (item) {
			if (!item.Polygon && !item.IsFolder) {
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
	}

	//! function that opens basket modal window
	function showDeletedData(e) {
		$.ajax({
			url: "./DeletedDistricts", // <----------------------------------------------------!!
		}).done(function (result) {
			deletedData = result.deletedDistricts;
			renderDeletedData();
			$('#deletedList').modal('show');
		});
	};

	//render deleted districts in template
	function renderDeletedData() {
		$('#districtDeletedTable tbody').html(deletedDistrictsTemplate({ deletedData: deletedData }));
	};

	//! function that is part of dinamic search. Only for searching.
	function searchDistricts(e) {
		//var searchString = $('#editInputDistrictId').val();
		var searchString = e;
		$.ajax({
			url: "./SearchDistrict", // <----------------------------------------------------!!
			data: { parameter: searchString },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			clearMap();
			if (result.success && result != null) {
				data = result.resultDistricts;
				renderData();
			}
			else if (!result.success && result.districts != undefined) {
				data = result.districts;
				renderData();
			}
			else { return false; }
		});
	}

	//! function that is part of dinamic search of deleted districts. Only for searching.
	function searchDeletedDistricts(e) {
		var searchDeletedString = e;
		$.ajax({
			url: "./SearchDeletedDistrict", // <----------------------------------------------------!!
			data: { parameter: searchDeletedString },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				deletedData = result.resultDistricts;
				renderDeletedData();
			}
			else if (!result.success && result.districts != undefined) {
				deletedData = result.districts;
				renderDeletedData();
			}
			else { return false; }
		});
	};

	function clearMap() {
		data.forEach(function (item) {
			if (item.Polygon) {
				item.Polygon.setMap(null);
				item.Marker.setMap(null);
			}
		});
	}

	//! function for editing selected district info
	function editItem(e) {
		var itemId = $(e).attr('data-items-id'); // get model item id from template
		var name = $(e).attr('data-items-name'); // get model item name from template

		$('#newName').val(name); // set model item name to an input field

		$('#editInputDistrictId').val(itemId); /////addd id!!!

		$("#edit-item").modal('show'); // show modal

	}

	//! function for deleting selected district.
	function deleteDistrict(e) {
		var itemId = $(e).attr('data-items-id');
		var items = getAllChildren(itemId);
		$('#confirm-delete').modal('show');
		$('#confirm-delete .btn-ok').off("click.deleteADistrict").on("click.deleteADistrict", function () {

			var request = $.ajax({
				url: "./DeleteDistrict/", // <----------------------------------------------------!!
				data: { Id: itemId },
				method: "POST",
			}).done(function (result) {
				if (result.success) {
					items.forEach(function (item) {
						if (item.Polygon) {
							item.Marker.setMap(null);
							item.Polygon.setMap(null);
						}
						item.ParentId = null;
						deleteDistrictFromItems(data, item);
					});
					renderData();
				}
				else { getDistrictErrorMessage(); }


			});
			$('#confirm-delete').modal('hide');
			$('#confirm-delete .btn-ok').off("click.deleteADistrict");
		});
	};

	function getDistrictById(id) {
		return data.find(function (item) {
			return item.Id == id
		});
	};

	//delete item from array of items
	function deleteDistrictFromItems(items, item) {
		items.splice(items.indexOf(item), 1);
	};

	//! function that opens modal window with error message.
	function getDistrictErrorMessage() {
		$('#get-district-error-modal').modal('show');
	}

	//! function that is part of sorting non-deleted entries logic. Affected by search input value.
	function sortDistrictBy(e) {
		var searchDString = $('#searcDhistrictName').val();
		switch (sortDistrictType) {
			case "name":
				if (e == "name") {
					e = "name_desc";
					break;
				}
				else { break; }
			default:
				break;
		}
		sortDistrictType = e;
		$.ajax({
			url: "./SearchAndSort", // <----------------------------------------------------!!
			data: { search: searchDString, sort: e },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				clearMap();
				data = result.resultDistricts;
				renderData();
			}
			else { return false; }
		});
	};

	//! function that is part of sorting deleted entries logic. Affected by search input value.
	function sortDeletedDistrictBy(e) {
		var searchDeletedDString = $('#searchDeletedDistrictName').val();
		switch (sortDeletedDistrictType) {
			case "name":
				if (e == "name") {
					e = "name_desc";
					break;
				}
				else { break; }
			default:
				break;
		}
		sortDeletedDistrictType = e;
		$.ajax({
			url: "./DeletedSearchAndSort", // <----------------------------------------------------!!
			data: { search: searchDeletedDString, sort: e },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				deletedData = result.sortedDeletedDistricts;
				renderDeletedData();
			}
			else { return false; }
		});
	};

	//! function that restore distirct form deleted state
	function restoreDistrict(e) {
		var itemId = $(e).attr('data-items-id'); // get model item id from template
		var name = $(e).attr('data-items-name'); // get model item name from template

		$.ajax({
			url: "./RestoreDistrict/", // <----------------------------------------------------!!
			data: { id: itemId }
		}).done(function (result) {
			if (result.success && result != null) {
				var restored = deletedData.find(function (item) {
					return item.Id == itemId;
				});
				deleteDistrictFromItems(deletedData, restored);
				restored.Deleted = false;
				data.push(restored);
				renderDeletedData();
				renderData();
			}
			else { getDistrictErrorMessage(); }
		});
	};

	//! function for adding new districts. Gets data and sends it on server via ajax.
	function addDistrictConfirm() {
		var newDistrictName = $('#newDistrictName').val();
		//set map data
		var district = findDistrict(selectedDistrict);
		var coordinates = mapCoordinates(district.Polygon);
		district.Coordinates = coordinates;
		var data = {
			Name: newDistrictName,
			Deleted: false,
			Coordinates: setCoordinatesIndex(coordinates)
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
				district.IsFolder = false;
				renderData();
				document.getElementById("add-district-form").reset();
			}
			else {
				data.splice(data.indexOf(district), 1);
				getDistrictErrorMessage();
				document.getElementById("add-district-form").reset();
			}
		})
		.always(function () {
			$("#add-item").modal('hide');
		});
	};

	//! function for adding new districts. Opens modal window.
	function addDistrict() {
		$('#add-item').modal('show');
	};

	//! set rigth indexes for chenched or added item
	function setCoordinatesIndex(items) {
		items.forEach(function (item, index) {
			item.Index = index;
		});
		return items;
	};

	//! function for editing selected district info. Gathers all data and sends it on server via ajax.
	function editConfirmDistrict() {
		var id = $('#editInputDistrictId').val();
		var name = $('#newName').val();
		editDistrict(id, name);
	}

	function editDistrict(id, name) {
		var item = getDistrictById(id);
		$.ajax({
			url: "./EditDistrict/", // <----------------------------------------------------!!
			data: JSON.stringify({
				Id: id,
				Deleted: item.Deleted,
				Name: name,
				Coordinates: setCoordinatesIndex(item.Coordinates),
				ParentId: item.ParentId
			}),
			contentType: "application/json; charset=utf-8",
			type: "post",
			dataType: "json",
		}).done(function (result) {
			if (result.success && result != null) {
				item.Name = name;
				item.Marker.labelContent = name;
				item.Marker.setMap(map);
				renderData();
				document.getElementById("edit-district-form").reset();
			}
			else {
				getDistrictErrorMessage();
				document.getElementById("edit-district-form").reset();
			}
		})
		.always(function () {
			$("#edit-item").modal('hide');
		});
	}

	//! add new group from modal window
	function addGroup() {
		var newGroupName = $('#newGroupName').val();
		var group = {
			Name: newGroupName,
			Deleted: false,
			IsFolder: true
		};
		$.ajax({
			contentType: "application/json; charset=utf-8",
			type: "post",
			dataType: "json",
			url: "./AddDistrict", // <----------------------------------------------------!!
			data: JSON.stringify(group)
		}).done(function (result) {
			if (result.success && result != null) {
				group.Id = result.district.Id;
				data.push(group);
				renderData();
				document.getElementById("add-group-form").reset();
			}
			else {
				getDistrictErrorMessage();
				document.getElementById("add-group-form").reset();
			}
		})
		.always(function () {
			$("#add-group-modal").modal('hide');
		});
	}

	function setParent(district) {
		$.ajax({
			type: "post",
			url: "./SetParent", // <----------------------------------------------------!!
			data: {
				id: district.Id,
				parentId: district.ParentId
			}
		}).done(function (result) {
			if (result.success) {

			}
			else {
				getDistrictErrorMessage();
			}
		})
	}

	//return all subgroup and childs for group with current id
	function getAllChildren(id) {
		var district = getDistrictById(id);
		var districts = [];
		var directChilds = [];
		if (district.IsFolder) {
			data.forEach(function (item) {
				if (item.ParentId == district.Id) {
					directChilds.push(item);
				}
			});
			directChilds.forEach(function (item) {

				districts = districts.concat(getAllChildren(item.Id));
			});
			districts.push(district);
		}
		else {
			districts.push(district);
		}
		return districts;
	}

	//set global variables for validator
	window.editConfirmDistrict = editConfirmDistrict;
	window.addDistrictConfirm = addDistrictConfirm;

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
			addDistrict();
			drawingManager.setDrawingMode(null);

			var newShape = e;
			data.push({
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
		renderData();
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
				var edit = district.Coordinates[number];
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
		return data.find(function (item) {
			return item.Polygon == polygon
		});
	}

	function clearSelection() {
		if (selectedDistrict) {
			var edit = findDistrict(selectedDistrict);
			if (edit.Edited) {
				editDistrict(edit.Id, edit.Name);
			}
			selectedDistrict.setEditable(false);
			selectedDistrict = null;
		}
		data.forEach(function (item) {
			if (item.Polygon) {
				item.Polygon.setEditable(false);
			}
		});
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

});

