function OnBegin() {
    $('#result').hide();
	$('#form0 input[type=submit]').attr('disabled', 'disabled');
}

function OnComplete(request, status) {
    $('#result').show();
    $('#form0 input[type=submit]').removeAttr('disabled');
}