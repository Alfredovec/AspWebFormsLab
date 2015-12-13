function getGames(e) {
    if (e != null) {
        e.preventDefault();
    }
    $('#loading').show();
    $('#searchResult').hide();
    $.ajax({
        url: "/api/games",
        data: $('#searchForm').serialize(),
        dataType: "json",
        success: function (data) {
            console.log(data);
            $('#pagination').remove();
            $('#searchResult>table>tbody').html('');
            generateGameTable(data.Games);

            $('#searchResult').append(getPagination(data.AwaiblePages, data.PageNumber));
            $('#loading').hide();
            $('#searchResult').show();
            $('#searchForm input[type=submit]').attr('disabled', 'disabled');
            createPagination();
        }
    });
}

function generateGameTable(games) {
    var table = $('#searchResult>table>tbody');
    for (var i = 0; i < games.length; i++) {
        var tr = $('<tr><td>' + games[i].Key + '</td><td>' + games[i].Name + '</td><td>' + games[i].Description + '</td><td>' + $('#helpers .anchor').html().replace('______', games[i].Key) + '</td></tr>');
        table.append(tr);
    }
}

function getPagination(pages, currentPage) {
    var div = $('<div id="pagination"></div>');
    div.append('<button class="prev">&lt;&lt;' + $('#helpers .prev').text() + '</button> ');
    for (var i = 0; i < pages.length; i++) {
        var current = pages[i] == currentPage;
        div.append('<button class="' + (current ? "current" : "") + '" ' + (current ? "disabled" : "") + '>' + pages[i] + '</button> ');
        if (i != (pages.length - 1) && pages[i + 1] - 1 != pages[i]) {
            div.append('<span>...</span>');
        }
    }
    div.append('<button class="next">' + $('#helpers .next').text() + '&gt;&gt;</button>');
    return div;
}

$(document).ready(function () {
    getGames();
    $('#searchForm input[type=submit]').click(getGames);
    var maxPrice = $('#MaxPrice').val();

    $("#slider").slider({
        range: true,
        min: 0,
        max: maxPrice,
        values: [0, maxPrice],
        slide: function (event, ui) {
            jQuery("input#MinPrice").val(ui.values[0]);
            jQuery("input#MaxPrice").val(ui.values[1]);
            $('#searchForm input[type=submit]').removeAttr('disabled');
        }
    });

    $("input#MinPrice").change(function () {

        var value1 = jQuery("input#MinPrice").val();
        var value2 = jQuery("input#MaxPrice").val();
        if (parseInt(value1) > parseInt(value2)) {
            value1 = value2;
            jQuery("input#MinPrice").val(value1);
        }
        jQuery("#slider").slider("values", 0, value1);
    });


    $("input#MaxPrice").change(function () {

        var value1 = jQuery("input#MinPrice").val();
        var value2 = jQuery("input#MaxPrice").val();

        if (value2 > maxPrice) {
            value2 = maxPrice;
            jQuery("input#MaxPrice").val(maxPrice)
        }

        if (parseInt(value1) > parseInt(value2)) {
            value2 = value1;
            jQuery("input#MaxPrice").val(value2);
        }
        jQuery("#slider").slider("values", 1, value2);
    });

    $('.filter-container input, .filter-container select').change(function(e) {
        $('#searchForm input[type=submit]').removeAttr('disabled');
        $('#pagination button').attr('disabled', 'disabled');
        $('#PageNumber').val(1);
    });

    $('#PageSize').change(function(e) {
        $('#PageNumber').val(1);
        getGames();
    });

    createPagination();
});

function createPagination() {
    var bts = $('#pagination button:not(.prev, .next, .current)');
    bts.click(function(e) {
        var page = $(this).text().trim();
        $('#PageNumber').val(page);
        e.preventDefault();
        getGames();
    });
    $('#pagination button.prev').click(function (e) {
        var current = $('#pagination button.current').text().trim();
        if (current == '1') {
            e.preventDefault();
            return;
        }
        $('#PageNumber').val(--current);
        getGames(e);
    });
    $('#pagination button.next').click(function (e) {
        var last = $('#pagination button:not(.prev, .next)').last().text().trim();
        var current = $('#pagination button.current').text().trim();
        if (current == last) {
            e.preventDefault();
            return;
        }
        $('#PageNumber').val(++current);
        getGames(e);
    });
}