var saveRegName = /([A-Z]{0,1}[a-z]{1,10}\s{0,1}){1,2}/;

var regNickName = /[0-9]{2,4}/;
var regName = /^[A-Z]{1}[a-z]{1,10}[ ]{0,1}([A-Z]{1}[a-z]{2,10}){0,1}/;
var regNumber = /[A-Z]{2}\d{4}[A-Z]{2}/;
var regDate = /^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$/;

function checkWithReg(input, regExp) {
	var isValueOk = regExp.exec(input.value);
	if (!isValueOk) {
		return false;
	}
	else return true;
}

function regCheck(input) {
	var OK = regNickName.exec(input.value);
	if (!OK) {
		return false;
	}
	else return true;
}

function addCarFormValidate() {

	var isCarNameOK = checkWithReg(document.getElementById('inputCarName'), regName);
	var isCarNickOK = checkWithReg(document.getElementById('inputCarNickName'), regNickName);
	var isCarNumOK = checkWithReg(document.getElementById('inputCarNumber'), regNumber);
	var isCarDateOK = checkWithReg(document.getElementById('datetimepicker2'), regDate);
	if (!isCarNickOK || !isCarNameOK || !isCarNumOK || !isCarDateOK) return false;
	var a = $('#add-car-form').validator('validate');
	if (a.valid()) {
		carController.addCarConfirm();
	}
	else return false;
}

function editCarFormValidate() {

	var isNewNameOK = checkWithReg(document.getElementById('newInputCarName'), regName);
	var isNewNickOK = checkWithReg(document.getElementById('newInputCarNickName'), regNickName);
	var isNewNumOK = checkWithReg(document.getElementById('newInputCarNumber'), regNumber);
	var isNewDateOK = checkWithReg(document.getElementById('datetimepicker1'), regDate);
	if (!isNewNickOK || !isNewNameOK || !isNewNumOK || !isNewDateOK) return false;
	var b = $('#edit-car-form').validator('validate');
	if (b.valid()) {
		carController.editCarConfirm();
	}
	else return false;
}
