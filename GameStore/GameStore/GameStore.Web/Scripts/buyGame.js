$(document).ready(function() {
    $('#buyButton').click(function(e) {
        e.preventDefault();
        var dataToSend = {
            gameId: $('#helpers .id').text(),
            quantity: 1
        }
        $.ajax({
            url: "/api/orders?gameId=" + $('#helpers .id').text() + "&quantity=1",
            method: "put",
            success: function(data) {
                getBasket();
            }
        });
        location.href = "#basket";
    });
});