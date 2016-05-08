var regDistName = /([A-Z]{0,1}[a-z]{1,10}\s{0,1}){1,4}/;
var regDistNameV1 = /^([A-Z]{0,1}[a-z]{1,10}[ ]{0,1}){1,4}|([А-Я]{0,1}[а-я]{1,10}[ ]{0,1}){1,4}/;
var regDistNameV2 = /^(([A-Z][^0-9]){0,1}([a-z][^0-9]){1,10}([ ][^0-9]){0,1}){1,4}|(([А-Я][^0-9]){0,1}([а-я][^0-9]){1,10}([ ][^0-9]){0,1}){1,4}/;
var regDistNameV3 = /^((([A-Z]){0,1}([a-z]){1,10}([ ]){0,1}[^0-9]){1,4}|(([А-Я]){0,1}([а-я]){1,10}([ ]){0,1}[^0-9]){1,4})/;

function checDistkWithReg(input, regExp) {
	var isValueOk = regExp.exec(input.value);
	if (!isValueOk) {
		return false;
	}
	else return true;
}

function validateAddDistrict() {
	var isNameOK = checDistkWithReg(document.getElementById('newDistrictName'), regDistNameV3);
	if (!isNameOK) return false;
	var validThisData = $('#add-district-form').validator('validate');
	if (validThisData.valid()) {
		addDistrictConfirm();
	}
	else return false;
}

function validateEditDistrict() {
	var isNewNameOK = checDistkWithReg(document.getElementById('newName'), regDistNameV3);
	if (!isNewNameOK) return false;
	var validThisData = $('#edit-district-form').validator('validate');
	if (validThisData.valid()) {
		editConfirmDistrict();
	}
	else return false;
}
