$(document).ready(function () {
	jsController.renderData();
});
var jsController = {
	data: { items: encodedData },
	deletedData: { deleted: encodedDeleted },

	renderData: function () {
		$('#districtEditTable tbody').html(districtTemplate(this.data));
	},

	renderDeletedData: function () {
		$('#districtDeletedTable tbody').html(deletedDistrictsTemplate(this.deletedData));
	},

	//! function that opens basket modal window
	DistrictBasket: function (e) {
		$('#deletedList').modal('show');
		$.ajax({
			url: "/Settings/DeletedDistricts",
		}).done(function (result) {
			jsController.deletedData.deleted = result;
			jsController.renderDeletedData();
		});
	},

	restoreDistrict: function (e) {
		var itemId = $(e).attr('data-items-id'); // get model item id from template
		var name = $(e).attr('data-items-name'); // get model item name from template

		$.ajax({
			url: "/Settings/RestoreDistrict/",
			data: { id: itemId }
		}).done(function (result) {
			jsController.deletedData.deleted = result;
			jsController.renderDeletedData();
			jsController.refreshData();
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

	//! function for adding new districts
	addDistrict: function () {
		var a = "breakpoint";
		$('#add-item').modal('show');

		//$('#add-item .btn-ok').off("click.addADistrict").on("click.addADistrict", function () {
		//	var d = "breakpoint!";
		//	var newDistrictName = $('#newDistrictName').val();
		//	$.ajax({
		//		url: "/Settings/AddDistrict",
		//		data: { name: newDistrictName }
		//	}).done(function (result) {
		//		jsController.data.items = result;
		//		jsController.renderData();
		//		document.getElementById("add-district-form").reset();
		//	});

		//	$("#add-item").modal('hide');
		//	$('#add-item .btn-ok').off("click.addADistrict");
		//});
	},

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
			else if (!result.success) {
				jsController.getDistrictErrorMessage();
				document.getElementById("add-district-form").reset();
			}
		})
		.always(function () {
			$("#add-item").modal('hide');
		});
	},

	//! function for deleting selected district
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
			}).success(function (result) {
				jsController.data.items = result;
				jsController.renderData();
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
		$("#edit-item").modal('show'); // show modal

		$('#editInputDistrictId').val(itemId); /////addd id!!!

	},

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
			else if (!result.success) {
				jsController.getDistrictErrorMessage();
				document.getElementById("edit-district-form").reset();
			}
		})
		.always(function () {
			$("#edit-item").modal('hide');
		});
	},

	getDistrictErrorMessage: function () {
		$('#get-district-error-modal').modal('show');
	}
};