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
	//render deleted districts in template
	renderDeletedData: function () {
		$('#districtDeletedTable tbody').html(deletedDistrictsTemplate(this.deletedData));
	},
	//! function that opens basket modal window
	DistrictBasket: function (e) {
		$.ajax({
			url: "/Settings/DeletedDistricts",
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
			url: "/Settings/RestoreDistrict/",
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
			url: "/Settings/GetAvailableDistricts/",
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
			url: "/Settings/AddDistrict",
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
			var c = 'yupiii';
			var request = $.ajax({
				url: "/Settings/DeleteDistrict/",
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
			url: "/Settings/EditDistrict/",
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