
$(document).ready(function () {
	carController.renderCarData();
});

var carController = {
	carData: { cars: encodedCarData },

	renderCarData: function () { //start
		$('#CarParkTable tbody').html(carParkTemplate(this.carData));
	},//end

	addNewCar: function () { //start
		$('#add-car-modal').modal('show');
	},//end

	addConfirm: function () { //start
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
			url: "/Cars/AddNewCar/",
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
			dataType: "JSON"
		}).done(function (result) {
			carController.carData.cars = result;
			carController.renderCarData();
			document.getElementById("add-car-form").reset();
		});
		$('#datetimepicker1').datetimepicker('clear');
		$('#datetimepicker2').datetimepicker('clear');
		$('#add-car-modal').modal('hide');
	},//end


	addConfirm2: function () { //start
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
			url: "/Cars/AddNewCar2/",
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
			dataType: "JSON",
		}).done(function (result) {
			if (result.success && result != null) {
				carController.carData.cars = result;
				carController.renderCarData();
				document.getElementById("add-car-form").reset();
			}
			else {
				carController.getErrorMessage();
			}
		})
		.fail(function () { alert("SHEEET!") })
		.always(function () {
			$('#datetimepicker1').datetimepicker('clear');
			$('#datetimepicker2').datetimepicker('clear');
			$('#add-car-modal').modal('hide');
		})
	},//end

	getErrorMessage: function () {
		$('#get-car-error-modal').modal('show');
		$('#get-car-error-modal .btn-try').off("click.errorCar").on("click.errorCar", function () {
			$('#get-car-error-modal .btn-try').off("click.errorCar");
			$('#get-car-error-modal').modal('hide');
			return false;
		});
	},

	getCarEdit: function (e) {

		$('#edit-car-modal').modal('show');
		var oldCarsId = $(e).attr('data-cars-id'); // take car`s id and send it via ajax

		$.ajax({
			url: "/Cars/GetCarForEdit/",
			data: {
				Id: oldCarsId,
			},
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {

			$('#newInputCarName').val(result.CarName);
			$('#newInputCarNickName').val(result.CarNickName);
			$('#newInputCarNumber').val(result.CarNumber);
			$('#newInputCarOccupation').val(result.CarOccupation);
			$('#newInputCarClass').val(result.CarClass);
			$('#newInputCarPetrolType').val(result.CarPetrolType);
			$('#newInputCarPetrolConsumption').val(result.CarPetrolConsumption);

			var ParseThis = moment(result.CarManufactureDate);
			var noMili = new Date(ParseThis).toLocaleDateString("en");
			$('#datetimepicker1').val(noMili);
			$('#newInputCarState').val(result.CarState);

			$('#edit-car-modal .btn-ok').off("click.editCar").on("click.editCar", function () {
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
					url: "/Cars/EditCar/",
					data: {
						Id: oldCarsId,
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
					carController.carData.cars = result;
					carController.renderCarData();
				});
				$('#datetimepicker1').datetimepicker('clear');
				$('#datetimepicker2').datetimepicker('clear');
				$('#edit-car-modal .btn-ok').off("click.editCar");
				$('#edit-car-modal').modal('hide');
				return false;
			});
		});
	},

	deleteCar: function (e) { //start
		var carsId = $(e).attr('data-cars-id'); //!!!
		var carsName = $(e).attr('data-cars-name');

		$('#car-modal-delete .deleteCar-name').html(carsName);
		$('#car-modal-delete').modal('show');

		$('#car-modal-delete .btn-ok').off("click.deleteCar").on("click.deleteCar", function () {
			$.ajax({
				url: "/Cars/DeleteCar/",
				data: { Id: carsId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				carController.carData.cars = result;
				carController.renderCarData();
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
				url: "/Cars/GiveCar/",
				data: { CarId: giftCarsId, DriverId: NewCarUserId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				carController.carData.cars = result;
				carController.renderCarData();
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
				url: "/Cars/ReturnCar/",
				data: { CarId: BorrowedCarsId, RealOwnerId: BorrowedCarsOwnerId },
				method: "POST",
				dataType: "JSON"
			}).done(function (result) {
				carController.carData.cars = result;
				carController.renderCarData();
			});

			$('#returnback-car-modal').modal('hide');
			$('#returnback-car-modal .btn-ok').off("click.returnCar");
			return false;
		});
	},

	getCarDetails: function (e) {
		$('#details-car-modal').modal('show');

		var detailCarsId = $(e).attr('data-cars-id');

		$.ajax({
			url: "/Cars/GetCarForEdit/",
			data: {
				Id: detailCarsId,
			},
			method: "POST",
			dataType: "JSON"
		}).done(function (result) {
			$('#details-car-modal .detailsCarName').html(result.CarName);
			$('#details-car-modal .detailsCarNickName').html(result.CarNickName);
			$('#details-car-modal .detailsCarNumber').html(result.CarNumber);
			$('#details-car-modal .detailsCarOccupation').html(result.CarOccupation);
			$('#details-car-modal .detailsCarClass').html(result.CarClassDescription);
			$('#details-car-modal .detailsCarPetrolType').html(result.CarPetrolTypeDescription);
			$('#details-car-modal .detailsCarPetrolConsumption').html(result.CarPetrolConsumption);

			var ParseThis = moment(result.CarManufactureDate);
			var noMili = new Date(ParseThis).toLocaleDateString("en");
			$('#details-car-modal .detailsdatetimepicker1').html(noMili);
			$('#details-car-modal .detailsCarState').html(result.CarStateDescription);
		});
	},
};