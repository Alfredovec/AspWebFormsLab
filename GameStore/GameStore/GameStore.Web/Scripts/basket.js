$(document).ready(function() {
    getBasket();
});

function getBasket() {
    $.ajax({
        url: $('#basketUrl').text(),
        success: function(data) {
            $('#basket').html('');
            $('#basket').html(data);
        }
    });
}