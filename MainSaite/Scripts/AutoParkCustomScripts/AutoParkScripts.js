var carParkTemplate = Handlebars.compile($("#carListRow").html());
var sortCounter;
$(document).ready(function () {
	carController.renderCarData();
});

var carController = {
	carData: { cars: encodedCarData },

	renderCarData: function () {
		$('#CarParkTable tbody').html(carParkTemplate(this.carData));
	},

	sortBy: function (e) {
		switch (sortCounter) {
			case "name":
				if (e == "name") {
					e = "name_desc";
					break;
				}
				else { break; }
			case "nickname":
				if (e == "nickname") {
					e = "nickname_desc";
					break;
				}
				else { break; }
			case "number":
				if (e == "number") {
					e = "number_desc";
					break;
				}
				else { break; }
			default:
				break;
		}
		sortCounter = e;
		$.ajax({
			url: SortCarPath,
			data: { parameter: e },
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				carController.carData.cars = result.DriversCars;
				carController.renderCarData();
			}
			else { return false; }
		});
	},

	addNewCar: function () {
		$('#add-car-modal').modal('show');
	},

	addCarConfirm: function () {
		var newCarName = $('#inputCarName').val();
		var newCarNickName = $('#inputCarNickName').val();
		var newCarNumber = $('#inputCarNumber').val();
		var newCarOccupation = $('#inputCarOccupation').val();
		var newCarClass = $('#inputCarClass').val();
		var newCarPetrolType = $('#inputCarPetrolType').val();
		var newCarPetrolConsumption = $('#inputCarPetrolConsumption').val();
		var newCarManufactureDate = $('#datetimepicker2').val();
		var newCarState = $('#inputCarState').val();
		$.ajax({
			url: AddCarPath, // <----------------------------------------------------!!
			data: {
				UserId: userId,
				OwnerId: ownerId,
				CarName: newCarName,
				CarNickName: newCarNickName,
				CarNumber: newCarNumber,
				CarOccupation: newCarOccupation,
				CarClass: newCarClass,
				CarPetrolType: newCarPetrolType,
				CarPetrolConsumption: newCarPetrolConsumption,
				CarManufactureDate: newCarManufactureDate,
				CarState: newCarState
			},
			cache: false,
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				carController.carData.cars = result.DriversCars;
				carController.renderCarData();
				document.getElementById("add-car-form").reset();
			}
			else {
				carController.getCarErrorMessage();
				document.getElementById("add-car-form").reset();
			}
		})
		.fail(function () { carController.getCarErrorMessage(); document.getElementById("add-car-form").reset(); })
		.always(function (result) {
			$('#datetimepicker1').datetimepicker('clear');
			$('#datetimepicker2').datetimepicker('clear');
			$('#add-car-modal').modal('hide');
		})
	},

	getCarErrorMessage: function () {
		$('#get-car-error-modal').modal('show');
	},

	getCarEdit: function (e) {
		//$('#edit-car-modal').modal('show');
		var oldCarsId = $(e).attr('data-cars-id'); // take car`s id and send it via ajax
		$.ajax({
			url: GetCarEditPath, // <----------------------------------------------------!!
			data: {
				Id: oldCarsId,
			},
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {
			if (result.success && result != null) {
				$('#newInputCarId').val(oldCarsId);
				$('#newInputCarName').val(result.carForEdit.CarName);
				$('#newInputCarNickName').val(result.carForEdit.CarNickName);
				$('#newInputCarNumber').val(result.carForEdit.CarNumber);
				$('#newInputCarOccupation').val(result.carForEdit.CarOccupation);
				$('#newInputCarClass').val(result.carForEdit.CarClass);
				$('#newInputCarPetrolType').val(result.carForEdit.CarPetrolType);
				$('#newInputCarPetrolConsumption').val(result.carForEdit.CarPetrolConsumption);

				var ParseThis = moment(result.carForEdit.CarManufactureDate);
				var noMili = new Date(ParseThis).toLocaleDateString("en");
				$('#datetimepicker1').val(noMili);
				$('#newInputCarState').val(result.carForEdit.CarState);
				$('#edit-car-modal').modal('show');//!!!
			}
			else {
				carController.getCarErrorMessage();
			}
		});
	},

	editCarConfirm: function () {
		var newInCarsId = $('#newInputCarId').val();
		var newInCarsName = $('#newInputCarName').val();
		var newInCarsNName = $('#newInputCarNickName').val();
		var newInCarsNumber = $('#newInputCarNumber').val();
		var newInCarsOccupation = $('#newInputCarOccupation').val();
		var newInCarsClass = $('#newInputCarClass').val();
		var newInCarsPT = $('#newInputCarPetrolType').val();
		var newInCarsPC = $('#newInputCarPetrolConsumption').val();
		var newInCarsMD = $('#datetimepicker1').val();
		var newInCarsState = $('#newInputCarState').val();
		$.ajax({
			url: EditCarPath, // <----------------------------------------------------!!
			data: {
				Id: newInCarsId,
				CarName: newInCarsName,
				CarNickName: newInCarsNName,
				CarNumber: newInCarsNumber,
				CarOccupation: newInCarsOccupation,
				CarClass: newInCarsClass,
				CarPetrolType: newInCarsPT,
				CarPetrolConsumption: newInCarsPC,
				CarManufactureDate: newInCarsMD,
				CarState: newInCarsState,
				UserId: userId,
				OwnerId: ownerId
			},
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {
			if (result.success && result != null) {
				carController.carData.cars = result.DriversCars;
				carController.renderCarData();
				document.getElementById("edit-car-form").reset();
			}
			else {
				carController.getCarErrorMessage();
				document.getElementById("edit-car-form").reset();
			}
		})
		.fail(function () { carController.getCarErrorMessage(); document.getElementById("edit-car-form").reset(); })
		.always(function () {
			$('#datetimepicker1').datetimepicker('clear');
			$('#datetimepicker2').datetimepicker('clear');
			$('#edit-car-modal .btn-ok').off("click.editCar");
			$('#edit-car-modal').modal('hide');
		})
	},

	getThisCarMain: function (e) {
		var carsId = $(e).attr('data-cars-id');
		var carsName = $(e).attr('data-cars-name');

		$('#car-modal-getMain .getMainCar-name').html(carsName);
		//$('#car-modal-getMain').modal('show'); // modal window

		//$('#car-modal-getMain .btn-ok').off("click.doMainCar").on("click.doMainCar", function () {
		//	$.ajax({
		//		url: SetCarStatusPath, // <----------------------------------------------------!!
		//		data: { Id: carsId },
		//		method: "POST",
		//		dataType: "JSON"
		//	}).done(function (result) {
		//		if (result.success && result != null) {
		//			carController.carData.cars = result.DriversCars;
		//			carController.renderCarData();
		//		}
		//		else {
		//			carController.getCarErrorMessage();
		//		}
		//	});
		//	$('#car-modal-getMain').modal('hide'); //modal window
		//	$('#car-modal-getMain .btn-ok').off("click.doMainCar");

		//	return false;
	    //});

		    $.ajax({
		        url: SetCarStatusPath, // <----------------------------------------------------!!
		        data: { Id: carsId },
		        method: "POST",
		        dataType: "JSON"
		    }).done(function (result) {
		        if (result.success && result != null) {
		            carController.carData.cars = result.DriversCars;
		            carController.renderCarData();
		        }
		        else {
		            carController.getCarErrorMessage();
		        }
		    });
		    $('#car-modal-getMain').modal('hide'); //modal window
		    $('#car-modal-getMain .btn-ok').off("click.doMainCar");
		    
            
		    if (window.Notification && Notification.permission !== "denied") {
		        Notification.requestPermission(function (status) {  // status is "granted", if accepted by user
		            var n = new Notification('Car changed!', {
		                body: 'Car changed!',
		            });
		        });
		    }


	},

	deleteCar: function (e) { //start
		var carsId = $(e).attr('data-cars-id'); //!!!
		var carsName = $(e).attr('data-cars-name');

		$('#car-modal-delete .deleteCar-name').html(carsName);
		$('#car-modal-delete').modal('show');

		$('#car-modal-delete .btn-ok').off("click.deleteCar").on("click.deleteCar", function () {
			$.ajax({
				url: DeleteCarPath, // <----------------------------------------------------!!
				data: { Id: carsId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				if (result.success && result != null) {
					carController.carData.cars = result.DriversCars;
					carController.renderCarData();
				}
				else {
					carController.getCarErrorMessage();
				}
			});
			$('#car-modal-delete').modal('hide');
			$('#car-modal-delete .btn-ok').off("click.deleteCar");
			return false;
		});
	}, //end

	giveCar: function (e) {
		var giftCarsId = $(e).attr('data-cars-id'); //!!!
		var giftCarName = $(e).attr('data-cars-name');

		$('#give-car-modal .giveAwayCar-name').html(giftCarName);
		$('#give-car-modal').modal('show');

		$('#give-car-modal .btn-ok').off("click.giveCar").on("click.giveCar", function () {
			var NewCarUserId = $('#giveAwayCarDrivers').val();

			$.ajax({
				url: GiveCarPath, // <----------------------------------------------------!!
				data: { CarId: giftCarsId, DriverId: NewCarUserId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				if (result.success && result != null) {
					carController.carData.cars = result.DriversCars;
					carController.renderCarData();
				}
				else { carController.getCarErrorMessage(); }
			});
			$('#give-car-modal').modal('hide');
			$('#give-car-modal .btn-ok').off("click.giveCar");
			return false;
		});
	},

	returnCar: function (e) {
		var BorrowedCarsId = $(e).attr('data-cars-id'); //!!!
		var BorrowedCarsName = $(e).attr('data-cars-name');
		var BorrowedCarsOwnerId = $(e).attr('data-cars-owner');

		$('#returnback-car-modal .borrowedCar-name').html(BorrowedCarsName);

		$('#returnback-car-modal').modal('show');

		$('#returnback-car-modal .btn-ok').off("click.returnCar").on("click.returnCar", function () {

			$.ajax({
				url: ReturnCarPath, // <----------------------------------------------------!!
				data: { CarId: BorrowedCarsId, RealOwnerId: BorrowedCarsOwnerId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				if (result.success && result != null) {
					carController.carData.cars = result.DriversCars;
					carController.renderCarData();
				}
				else { carController.getCarErrorMessage(); }
			});
			$('#returnback-car-modal').modal('hide');
			$('#returnback-car-modal .btn-ok').off("click.returnCar");
			return false;
		});
	},

	getCarDetails: function (e) {

		var detailCarsId = $(e).attr('data-cars-id');

		$.ajax({
			url: GetCarEditPath, // <----------------------------------------------------!!
			data: {
				Id: detailCarsId,
			},
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {
			if (result.success && result != null) {
				$('#details-car-modal .detailsCarName').html(result.carForEdit.CarName);
				$('#details-car-modal .detailsCarNickName').html(result.carForEdit.CarNickName);
				$('#details-car-modal .detailsCarNumber').html(result.carForEdit.CarNumber);
				$('#details-car-modal .detailsCarOccupation').html(result.carForEdit.CarOccupation);
				$('#details-car-modal .detailsCarClass').html(result.carForEdit.CarClassDescription);
				$('#details-car-modal .detailsCarPetrolType').html(result.carForEdit.CarPetrolTypeDescription);
				$('#details-car-modal .detailsCarPetrolConsumption').html(result.carForEdit.CarPetrolConsumption);

				var ParseThis = moment(result.carForEdit.CarManufactureDate);
				var noMili = new Date(ParseThis).toLocaleDateString("en");
				$('#details-car-modal .detailsdatetimepicker1').html(noMili);
				$('#details-car-modal .detailsCarState').html(result.carForEdit.CarStateDescription);

				$('#details-car-modal').modal('show');
			}
			else { carController.getCarErrorMessage(); }
		});
	},
};