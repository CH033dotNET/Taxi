
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

	giveCar: function (e) {
		var giftCarsId = $(e).attr('data-cars-id'); //!!!
		var giftCarName = $(e).attr('data-cars-name');

		$('#give-car-modal .giveAwayCar-name').html(giftCarName);
		$('#give-car-modal').modal('show');

		$('#give-car-modal .btn-ok').on("click", function () {
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
			$('#give-car-modal .btn-ok').off("click");
			return false;
		});
	},

	returnCar: function (e) {
		var BorrowedCarsId = $(e).attr('data-cars-id'); //!!!
		var BorrowedCarsName = $(e).attr('data-cars-name');
		var BorrowedCarsOwnerId = $(e).attr('data-cars-owner');

		$('#returnback-car-modal .borrowedCar-name').html(BorrowedCarsName);

		$('#returnback-car-modal').modal('show');

		$('#returnback-car-modal .btn-ok').on("click", function () {

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
			$('#returnback-car-modal .btn-ok').off("click");
			return false;
		});
	},

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

	deleteCar: function (e) { //start
		var carsId = $(e).attr('data-cars-id'); //!!!
		var carsName = $(e).attr('data-cars-name');

		$('#car-modal-delete .deleteCar-name').html(carsName);
		$('#car-modal-delete').modal('show');

		$('#car-modal-delete .btn-ok').on("click", function () {
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
			$('#car-modal-delete .btn-ok').off("click");
			return false;
		});
	}, //end

	editCar: function (e) {
		var oldCarsId = $(e).attr('data-cars-id');
		var oldCarsName = $(e).attr('data-cars-name');
		var oldCarsNickName = $(e).attr('data-cars-nname');
		var oldCarsNumber = $(e).attr('data-cars-number');
		var oldCarsOccupation = $(e).attr('data-cars-occup');
		var oldCarsClass = $(e).attr('data-cars-class');
		var oldCarsPetrolType = $(e).attr('data-cars-petroltype');
		var oldCarsPetrolConsumption = $(e).attr('data-cars-consum');
		var oldCarsManufactureDate = $(e).attr('edit-cars-date');
		var oldCarsState = $(e).attr('data-cars-state');

		$('#newInputCarName').val(oldCarsName);
		$('#newInputCarNickName').val(oldCarsNickName);
		$('#newInputCarNumber').val(oldCarsNumber);
		$('#newInputCarOccupation').val(oldCarsOccupation);
		$('#newInputCarClass').val(oldCarsClass);
		$('#newInputCarPetrolType').val(oldCarsPetrolType);
		$('#newInputCarPetrolConsumption').val(oldCarsPetrolConsumption);
		$('#datetimepicker1').val(oldCarsManufactureDate);
		$('#newInputCarState').val(oldCarsState);

		$('#edit-car-modal').modal('show');

		$('#edit-car-modal .btn-ok').on("click", function () {
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

			$('#edit-car-modal').modal('hide');
			$('#edit-car-modal .btn-ok').off("click");
			return false;
		});
	},

	carDetails: function (e) {

		var carsId = $(e).attr('data-cars-id');
		var carsName = $(e).attr('data-cars-name');
		var carsNickName = $(e).attr('data-cars-nname');
		var carsNumber = $(e).attr('data-cars-number');
		var carsOccupation = $(e).attr('data-cars-occup');
		var carsClass = $(e).attr('data-cars-class');
		var carsPetrolType = $(e).attr('data-cars-petroltype');
		var carsPetrolConsumption = $(e).attr('data-cars-consum');
		var carsManufactureDate = $(e).attr('data-cars-date');
		var carsState = $(e).attr('data-cars-state');

		//$('#detailsCarName').val(carsName);
		//$('#detailsCarNickName').val(carsNickName);
		//$('#detailsCarNumber').val(carsNumber);
		//$('#detailsCarOccupation').val(carsOccupation);
		//$('#detailsCarClass').val(carsClass);
		//$('#detailsCarPetrolType').val(carsPetrolType);
		//$('#detailsCarPetrolConsumption').val(carsPetrolConsumption);
		//$('#detailsdatetimepicker1').val(carsManufactureDate);
		//$('#detailsCarState').val(carsState);

		$('#details-car-modal .detailsCarName').html(carsName);
		$('#details-car-modal .detailsCarNickName').html(carsNickName);
		$('#details-car-modal .detailsCarNumber').html(carsNumber);
		$('#details-car-modal .detailsCarOccupation').html(carsOccupation);
		$('#details-car-modal .detailsCarClass').html(carsClass);
		$('#details-car-modal .detailsCarPetrolType').html(carsPetrolType);
		$('#details-car-modal .detailsCarPetrolConsumption').html(carsPetrolConsumption);
		$('#details-car-modal .detailsdatetimepicker1').html(carsManufactureDate);
		$('#details-car-modal .detailsCarState').html(carsState);

		$('#details-car-modal').modal('show');
	},
};