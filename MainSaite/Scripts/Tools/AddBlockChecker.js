$(document).ready(function () {
	//we create element, that definitely blocked by adblock
	var test = document.createElement('div');
	test.innerHTML = '&nbsp;';
	//this class isn`t valid for addblock
	test.className = 'adsbox';
	document.body.appendChild(test);

	//and check it heigth. If addblick works, it hide our new block
	if (test.offsetHeight === 0) {
		$('#adblock-warning').fadeIn('fast');
	}
	//remove our test block
	test.remove();
});