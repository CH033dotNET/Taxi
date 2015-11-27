var itemId;
var itemName;
var districtTemplate = Handlebars.compile($("#dsEditRow").html());
var deletedDistrictsTemplate = Handlebars.compile($("#dsDeletedRow").html());

var sortDistrictCounter;
var sortDeletedDistrictCounter;

$(document).ready(function () {
	jsController.renderData();
});
var jsController = {
	data: { items: encodedData },
	deletedData: { deleted: encodedDeleted },

	// render non-deleted districts in template
	renderData: function () {
		$('#districtEditTable tbody').html(districtTemplate(this.data));
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
			if (result.success && result != null) {
				jsController.data.items = result.resultDistricts;
				jsController.renderData();
			}
			else if (!result.success && result.districts != undefined ) {
				jsController.data.items = result.districts;
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
				jsController.data.items = result.resultDistricts;
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
				jsController.deletedData.deleted = result.deletedDistricts;
				jsController.data.items = result.workingDistricts;
				jsController.renderDeletedData();
				jsController.renderData();
			}
			else { jsController.getDistrictErrorMessage(); }
		});
	},

	refreshData: function (e) {
		$.ajax({
			url: "./GetAvailableDistricts/", // <----------------------------------------------------!!
		}).done(function (result) {
			jsController.data.items = result;
			jsController.renderData();
		});
	},
	//! function for adding new districts. Opens modal window.
	addDistrict: function () {
		var a = "breakpoint";
		$('#add-item').modal('show');
	},
	//! function for adding new districts. Gets data and sends it on server via ajax.
	addDistrictConfirm: function () {
		var d = "breakpoint!";
		var newDistrictName = $('#newDistrictName').val();
		$.ajax({
			url: "./AddDistrict", // <----------------------------------------------------!!
			data: { Name: newDistrictName, Deleted: false }
		}).done(function (result) {
			if (result.success && result != null) {
				jsController.data.items = result.districts;
				jsController.renderData();
				document.getElementById("add-district-form").reset();
			}
			else {
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

		$('#confirm-delete .btn-ok').off("click.deleteADistrict").on("click.deleteADistrict", function () {
			var request = $.ajax({
				url: "./DeleteDistrict/", // <----------------------------------------------------!!
				data: { Id: itemId, Name: itemName },
				method: "POST",
			}).done(function (result) {
				if (result.success && result != null) {
					jsController.data.items = result.districts;
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
		var datId = $('#editInputDistrictId').val();
		var c = "breakpoint";
		var newName = $('#newName').val();
		$.ajax({
			url: "./EditDistrict/", // <----------------------------------------------------!!
			data: { id: datId, name: newName },
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {
			if (result.success && result != null) {
				jsController.data.items = result.districts;
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
	}
};